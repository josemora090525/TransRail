using System.Drawing;
using System.Windows.Forms;
using TransRail.Application.DTOs;
using TransRail.Presentation.Controls;
using TransRail.Presentation.Presenters;
using TransRail.Presentation.Theme;
using TransRail.Presentation.Views;

namespace TransRail.Presentation.Forms;

public sealed class PassengerRoutesForm : TransRailFormBase, IPassengerRoutesView
{
    private readonly ComboBox _cmbOrigen = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly ComboBox _cmbDestino = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly DataGridView _grid = new();
    private readonly Label _lblHeadline = new() { AutoSize = true, ForeColor = TransRailTheme.AccentGreen, Font = TransRailTheme.TitleFont };
    private readonly Label _lblDistance = new() { AutoSize = true, ForeColor = TransRailTheme.InkDark, Font = TransRailTheme.SectionFont };
    private readonly Label _lblPath = new() { AutoSize = true, ForeColor = TransRailTheme.InkDark, Font = TransRailTheme.SubtitleFont };
    private readonly Label _lblStatus = new() { AutoSize = true, ForeColor = TransRailTheme.InkDark, Font = new Font(TransRailTheme.NormalFont, FontStyle.Bold) };
    private readonly PassengerRoutesPresenter _presenter;
    private string _selectedScheduleCode = string.Empty;

    public PassengerRoutesForm()
    {
        Text = "Rutas disponibles";
        Size = new Size(1220, 740);
        MinimumSize = new Size(1040, 660);
        BackColor = TransRailTheme.PrimaryDark;
        ForeColor = TransRailTheme.WhiteSoft;
        Font = TransRailTheme.NormalFont;

        var split = TransRailFormLayout.CreateModuleSplit(320, 300, 720);
        split.Panel1.Controls.Add(BuildFiltersPanel());
        split.Panel2.Controls.Add(BuildResultsPanel());
        Controls.Add(split);

        _presenter = new PassengerRoutesPresenter(this, AppServices.PassengerPortalUseCase);
        Shown += async (_, _) => await _presenter.InitializeAsync();
    }

    public string CodigoOrigen => (_cmbOrigen.SelectedItem as PassengerStationOptionDto)?.CodigoEstacion ?? string.Empty;
    public string CodigoDestino => (_cmbDestino.SelectedItem as PassengerStationOptionDto)?.CodigoEstacion ?? string.Empty;
    public string CodigoHorarioSeleccionado => _selectedScheduleCode;

    public event EventHandler? SearchRequested;
    public event EventHandler? SelectRequested;

    public void BindStations(IReadOnlyCollection<PassengerStationOptionDto> estaciones)
    {
        _cmbOrigen.DataSource = estaciones.ToList();
        _cmbOrigen.DisplayMember = nameof(PassengerStationOptionDto.Etiqueta);
        _cmbDestino.DataSource = estaciones.ToList();
        _cmbDestino.DisplayMember = nameof(PassengerStationOptionDto.Etiqueta);
    }

    public void BindSchedules(IReadOnlyCollection<PassengerScheduleOptionDto> horarios)
    {
        _selectedScheduleCode = string.Empty;
        _grid.DataSource = horarios.Select(x => new
        {
            x.CodigoHorario,
            x.CodigoRuta,
            x.Origen,
            x.Destino,
            Fecha = x.Fecha.ToString("yyyy-MM-dd"),
            Salida = x.HoraSalida.ToString("HH:mm"),
            Llegada = x.HoraLlegada.ToString("HH:mm"),
            x.DistanciaKm,
            RutaDirecta = x.EsRutaDirecta ? "S\u00ed" : "Con tramo"
        }).ToList();
    }

    public void ShowRouteSummary(PassengerRouteSearchResultDto result, string selectedScheduleText)
    {
        _lblHeadline.Text = string.IsNullOrWhiteSpace(result.EtiquetaOrigen)
            ? "A\u00fan no has buscado una ruta"
            : string.IsNullOrWhiteSpace(result.EtiquetaDestino)
                ? result.EtiquetaOrigen
            : $"{result.EtiquetaOrigen} -> {result.EtiquetaDestino}";

        _lblDistance.Text = result.DistanciaKm > 0
            ? $"Distancia total calculada: {result.DistanciaKm} km"
            : "A\u00fan no hay una distancia calculada.";

        _lblPath.Text = result.Recorrido.Count > 0
            ? $"Recorrido calculado con Dijkstra: {string.Join(" -> ", result.Recorrido)}"
            : "A\u00fan no hay un recorrido calculado.";

        _lblStatus.Text = selectedScheduleText;
    }

    public void ShowMessage(string message)
    {
        MessageBox.Show(this, message, "TransRail", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private Control BuildFiltersPanel()
    {
        var form = new TableLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 1,
            Width = 270,
            Margin = Padding.Empty
        };

        form.Controls.Add(MakeLabel("Ciudad o estaci\u00f3n de origen"));
        form.Controls.Add(BuildInput(_cmbOrigen));
        form.Controls.Add(MakeLabel("Ciudad o estaci\u00f3n de destino"));
        form.Controls.Add(BuildInput(_cmbDestino));

        var btnBuscar = new TransRailButton
        {
            Text = "Buscar rutas",
            Width = 270,
            Height = 46,
            Margin = new Padding(0, 12, 0, 0)
        };
        btnBuscar.Click += (_, _) => SearchRequested?.Invoke(this, EventArgs.Empty);
        form.Controls.Add(btnBuscar);

        var btnSeleccionar = new TransRailButton
        {
            Text = "Elegir horario",
            Width = 270,
            Height = 46,
            Margin = new Padding(0, 8, 0, 0)
        };
        btnSeleccionar.Click += (_, _) => SelectRequested?.Invoke(this, EventArgs.Empty);
        form.Controls.Add(btnSeleccionar);

        var note = new Label
        {
            Text = "Busca por origen y destino. Si no existe ruta directa, ver\u00e1s el trayecto m\u00e1s corto calculado con Dijkstra.",
            ForeColor = TransRailTheme.WhiteSoft,
            AutoSize = true,
            Margin = new Padding(0, 14, 0, 0)
        };
        note.MaximumSize = new Size(270, 0);
        form.Controls.Add(note);

        return TransRailFormLayout.CreateCenteredScrollHost(form, TransRailTheme.Surface, 270);
    }

    private Control BuildResultsPanel()
    {
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = TransRailTheme.SurfaceAlt,
            Padding = new Padding(18),
            ColumnCount = 1,
            RowCount = 2
        };
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 210));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var summaryCard = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Padding = new Padding(22)
        };
        var summaryStack = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true
        };
        _lblHeadline.Margin = new Padding(0, 0, 0, 8);
        _lblDistance.Margin = new Padding(0, 0, 0, 8);
        _lblPath.Margin = new Padding(0, 0, 0, 12);
        _lblStatus.Margin = new Padding(0);
        summaryStack.Controls.Add(_lblHeadline);
        summaryStack.Controls.Add(_lblDistance);
        summaryStack.Controls.Add(_lblPath);
        summaryStack.Controls.Add(_lblStatus);
        summaryCard.Controls.Add(summaryStack);
        TransRailFormLayout.BindWrapWidth(_lblHeadline, summaryStack, 10);
        TransRailFormLayout.BindWrapWidth(_lblDistance, summaryStack, 10);
        TransRailFormLayout.BindWrapWidth(_lblPath, summaryStack, 10);
        TransRailFormLayout.BindWrapWidth(_lblStatus, summaryStack, 10);
        panel.Controls.Add(summaryCard, 0, 0);

        _grid.Dock = DockStyle.Fill;
        _grid.ReadOnly = true;
        _grid.AllowUserToAddRows = false;
        _grid.AllowUserToDeleteRows = false;
        _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _grid.MultiSelect = false;
        TransRailGridStyler.ApplyStandardStyle(_grid);
        _grid.SelectionChanged += (_, _) =>
        {
            if (_grid.SelectedRows.Count == 0)
            {
                _selectedScheduleCode = string.Empty;
                return;
            }

            _selectedScheduleCode = _grid.SelectedRows[0].Cells["CodigoHorario"]?.Value?.ToString() ?? string.Empty;
        };
        panel.Controls.Add(_grid, 0, 1);

        return panel;
    }

    private static Control BuildInput(Control control)
    {
        control.Width = 270;
        control.Margin = new Padding(0, 4, 0, 10);
        return control;
    }

    private static Label MakeLabel(string text)
    {
        return new Label
        {
            Text = text,
            AutoSize = true,
            ForeColor = TransRailTheme.WhiteSoft,
            Margin = new Padding(0, 0, 0, 0)
        };
    }
}
