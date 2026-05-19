using System.Drawing;
using System.Windows.Forms;
using TransRail.Domain.Entities;
using TransRail.Presentation.Controls;
using TransRail.Presentation.Presenters;
using TransRail.Presentation.Theme;
using TransRail.Presentation.Views;

namespace TransRail.Presentation.Forms;

public sealed class LuggageManagementForm : TransRailFormBase, IEquipajeView
{
    private readonly TextBox _txtCodigo = new();
    private readonly TextBox _txtBoleto = new();
    private readonly TextBox _txtVagon = new();
    private readonly NumericUpDown _numPeso = new() { Minimum = 0.1m, Maximum = 200, DecimalPlaces = 2, Increment = 0.5m };
    private readonly TextBox _txtDescripcion = new();
    private readonly TextBox _txtBusqueda = new();
    private readonly TextBox _txtFiltroVagon = new();
    private readonly Label _lblPila = new() { AutoSize = true, ForeColor = TransRailTheme.WhiteSoft };
    private readonly DataGridView _grid = new();
    private readonly EquipajePresenter _presenter;
    private IReadOnlyCollection<Equipaje> _cache = Array.Empty<Equipaje>();

    public LuggageManagementForm()
    {
        Text = "Gesti\u00f3n de equipaje";
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(1160, 690);
        MinimumSize = new Size(980, 620);
        BackColor = TransRailTheme.PrimaryDark;
        ForeColor = TransRailTheme.WhiteSoft;
        Font = TransRailTheme.NormalFont;

        var split = TransRailFormLayout.CreateModuleSplit(360, 320, 660);
        split.Panel1.Controls.Add(BuildLeftPanel());
        split.Panel2.Controls.Add(BuildGrid());
        Controls.Add(split);

        _presenter = new EquipajePresenter(this, AppServices.LuggageOperationsUseCase);
        Shown += async (_, _) => await _presenter.RefreshAsync();
    }

    public string CodigoEquipaje => _txtCodigo.Text.Trim();
    public string CodigoBoleto => _txtBoleto.Text.Trim();
    public string CodigoVagonCarga => _txtVagon.Text.Trim();
    public double PesoKg => (double)_numPeso.Value;
    public string Descripcion => _txtDescripcion.Text.Trim();
    public string CodigoBusqueda => _txtBusqueda.Text.Trim();
    public string CodigoVagonFiltro => _txtFiltroVagon.Text.Trim();

    public event EventHandler? SaveRequested;
    public event EventHandler? DeleteRequested;
    public event EventHandler? RefreshRequested;
    public event EventHandler? SearchRequested;
    public event EventHandler? FilterByVagonRequested;
    public event EventHandler? BuildStackRequested;

    public void BindEquipajes(IReadOnlyCollection<Equipaje> equipajes)
    {
        _cache = equipajes;
        _grid.DataSource = equipajes.Select(x => new
        {
            x.CodigoEquipaje,
            x.CodigoBoleto,
            x.CodigoVagonCarga,
            x.PesoKg,
            x.Descripcion
        }).ToList();
    }

    public void FillForm(Equipaje equipaje)
    {
        _txtCodigo.Text = equipaje.CodigoEquipaje;
        _txtBoleto.Text = equipaje.CodigoBoleto;
        _txtVagon.Text = equipaje.CodigoVagonCarga;
        _numPeso.Value = Math.Clamp((decimal)equipaje.PesoKg, _numPeso.Minimum, _numPeso.Maximum);
        _txtDescripcion.Text = equipaje.Descripcion;
        _txtBusqueda.Text = equipaje.CodigoEquipaje;
    }

    public void ShowStackInfo(string stackSummary)
    {
        _lblPila.Text = stackSummary;
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

        panel.Controls.Add(MakeLabel("C\u00f3digo del equipaje"));
        panel.Controls.Add(_txtCodigo);
        panel.Controls.Add(MakeLabel("C\u00f3digo del boleto"));
        panel.Controls.Add(_txtBoleto);
        panel.Controls.Add(MakeLabel("C\u00f3digo del vag\u00f3n de carga"));
        panel.Controls.Add(_txtVagon);
        panel.Controls.Add(MakeLabel("Peso (kg)"));
        panel.Controls.Add(_numPeso);
        panel.Controls.Add(MakeLabel("Descripci\u00f3n"));
        panel.Controls.Add(_txtDescripcion);
        panel.Controls.Add(MakeLabel("Buscar por c\u00f3digo"));
        panel.Controls.Add(_txtBusqueda);
        panel.Controls.Add(MakeLabel("Filtrar por vag\u00f3n"));
        panel.Controls.Add(_txtFiltroVagon);

        var actions = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = true };

        var btnGuardar = new TransRailButton { Text = "Guardar", Width = 150 };
        btnGuardar.Click += (_, _) => SaveRequested?.Invoke(this, EventArgs.Empty);
        actions.Controls.Add(btnGuardar);

        var btnBuscar = new TransRailButton { Text = "Buscar", Width = 150 };
        btnBuscar.Click += (_, _) => SearchRequested?.Invoke(this, EventArgs.Empty);
        actions.Controls.Add(btnBuscar);

        var btnEliminar = new TransRailButton { Text = "Eliminar", Width = 150 };
        btnEliminar.Click += (_, _) => DeleteRequested?.Invoke(this, EventArgs.Empty);
        actions.Controls.Add(btnEliminar);

        var btnFiltrar = new TransRailButton { Text = "Filtrar vag\u00f3n", Width = 150 };
        btnFiltrar.Click += (_, _) => FilterByVagonRequested?.Invoke(this, EventArgs.Empty);
        actions.Controls.Add(btnFiltrar);

        var btnPila = new TransRailButton { Text = "Construir pila", Width = 150 };
        btnPila.Click += (_, _) => BuildStackRequested?.Invoke(this, EventArgs.Empty);
        actions.Controls.Add(btnPila);

        var btnRefresh = new TransRailButton { Text = "Actualizar lista", Width = 150 };
        btnRefresh.Click += (_, _) => RefreshRequested?.Invoke(this, EventArgs.Empty);
        actions.Controls.Add(btnRefresh);

        panel.Controls.Add(actions);
        panel.Controls.Add(_lblPila);
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
        _grid.CellDoubleClick += (_, e) =>
        {
            if (e.RowIndex < 0 || e.RowIndex >= _grid.Rows.Count)
            {
                return;
            }

            var codigo = _grid.Rows[e.RowIndex].Cells["CodigoEquipaje"]?.Value?.ToString();
            if (string.IsNullOrWhiteSpace(codigo))
            {
                return;
            }

            var equipaje = _cache.FirstOrDefault(x => x.CodigoEquipaje.Equals(codigo, StringComparison.OrdinalIgnoreCase));
            if (equipaje is not null)
            {
                FillForm(equipaje);
            }
        };
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
