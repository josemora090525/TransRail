using System.Drawing;
using System.Windows.Forms;
using TransRail.Domain.Entities;
using TransRail.Presentation.Controls;
using TransRail.Presentation.Presenters;
using TransRail.Presentation.Theme;
using TransRail.Presentation.Views;

namespace TransRail.Presentation.Forms;

public sealed class ScheduleManagementForm : TransRailFormBase, IScheduleView
{
    private readonly TextBox _txtCodigoHorario = new();
    private readonly TextBox _txtCodigoTren = new();
    private readonly TextBox _txtCodigoRuta = new();
    private readonly DateTimePicker _dtFecha = new() { Format = DateTimePickerFormat.Short };
    private readonly DateTimePicker _dtHoraSalida = new() { Format = DateTimePickerFormat.Time, ShowUpDown = true };
    private readonly DateTimePicker _dtHoraLlegada = new() { Format = DateTimePickerFormat.Time, ShowUpDown = true };
    private readonly TextBox _txtFiltroTren = new();
    private readonly DataGridView _grid = new();
    private readonly SchedulePresenter _presenter;

    public ScheduleManagementForm()
    {
        Text = "Gesti\u00f3n de horarios";
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(1200, 700);
        MinimumSize = new Size(1000, 620);
        BackColor = TransRailTheme.PrimaryDark;
        ForeColor = TransRailTheme.WhiteSoft;
        Font = TransRailTheme.NormalFont;

        var split = TransRailFormLayout.CreateModuleSplit(340, 300, 680);
        split.Panel1.Controls.Add(BuildLeftPanel());
        split.Panel2.Controls.Add(BuildGrid());
        Controls.Add(split);

        _presenter = new SchedulePresenter(this, AppServices.ManageScheduleUseCase);
        Shown += async (_, _) => await _presenter.RefrescarAsync();
    }

    public string CodigoHorario => _txtCodigoHorario.Text.Trim();
    public string CodigoTren => _txtCodigoTren.Text.Trim();
    public string CodigoRuta => _txtCodigoRuta.Text.Trim();
    public DateOnly Fecha => DateOnly.FromDateTime(_dtFecha.Value.Date);
    public TimeOnly HoraSalida => TimeOnly.FromDateTime(_dtHoraSalida.Value);
    public TimeOnly HoraLlegada => TimeOnly.FromDateTime(_dtHoraLlegada.Value);
    public string CodigoTrenFiltro => _txtFiltroTren.Text.Trim();

    public event EventHandler? CreateRequested;
    public event EventHandler? RefreshRequested;
    public event EventHandler? FilterByTrainRequested;

    public void BindHorarios(IReadOnlyCollection<Horario> horarios)
    {
        _grid.DataSource = horarios
            .OrderBy(x => x.Key)
            .Select(x => new
            {
                x.CodigoHorario,
                x.CodigoTren,
                x.CodigoRuta,
                Fecha = x.Fecha.ToString("yyyy-MM-dd"),
                Salida = x.HoraSalida.ToString("HH:mm"),
                Llegada = x.HoraLlegada.ToString("HH:mm")
            })
            .ToList();
    }

    public void ShowMessage(string message)
    {
        MessageBox.Show(this, message, "TransRail", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private Control BuildLeftPanel()
    {
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            Padding = new Padding(14),
            ColumnCount = 1,
            BackColor = TransRailTheme.Surface
        };

        panel.Controls.Add(MakeLabel("C\u00f3digo del horario"));
        panel.Controls.Add(_txtCodigoHorario);
        panel.Controls.Add(MakeLabel("C\u00f3digo del tren"));
        panel.Controls.Add(_txtCodigoTren);
        panel.Controls.Add(MakeLabel("C\u00f3digo de la ruta"));
        panel.Controls.Add(_txtCodigoRuta);
        panel.Controls.Add(MakeLabel("Fecha"));
        panel.Controls.Add(_dtFecha);
        panel.Controls.Add(MakeLabel("Hora de salida"));
        panel.Controls.Add(_dtHoraSalida);
        panel.Controls.Add(MakeLabel("Hora de llegada"));
        panel.Controls.Add(_dtHoraLlegada);
        panel.Controls.Add(MakeLabel("Filtrar por c\u00f3digo del tren"));
        panel.Controls.Add(_txtFiltroTren);

        var actions = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = true };
        var btnGuardar = new TransRailButton { Text = "Guardar horario", Width = 154 };
        btnGuardar.Click += (_, _) => CreateRequested?.Invoke(this, EventArgs.Empty);
        actions.Controls.Add(btnGuardar);
        var btnFiltrar = new TransRailButton { Text = "Filtrar", Width = 154 };
        btnFiltrar.Click += (_, _) => FilterByTrainRequested?.Invoke(this, EventArgs.Empty);
        actions.Controls.Add(btnFiltrar);
        var btnRefresh = new TransRailButton { Text = "Actualizar lista", Width = 154 };
        btnRefresh.Click += (_, _) => RefreshRequested?.Invoke(this, EventArgs.Empty);
        actions.Controls.Add(btnRefresh);
        panel.Controls.Add(actions);

        panel.Controls.Add(new Label
        {
            Text = "Nota: el orden cronol\u00f3gico se maneja internamente.",
            ForeColor = TransRailTheme.WhiteSoft,
            AutoSize = true,
            Padding = new Padding(0, 8, 0, 0)
        });

        return panel;
    }

    private Control BuildGrid()
    {
        _grid.Dock = DockStyle.Fill;
        _grid.ReadOnly = true;
        _grid.AllowUserToAddRows = false;
        _grid.AllowUserToDeleteRows = false;
        _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _grid.MultiSelect = false;
        TransRailGridStyler.ApplyStandardStyle(_grid);
        return _grid;
    }

    private static Label MakeLabel(string text)
    {
        return new Label
        {
            Text = text,
            ForeColor = TransRailTheme.WhiteSoft,
            AutoSize = true,
            TextAlign = ContentAlignment.BottomLeft
        };
    }
}
