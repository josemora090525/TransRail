using System.Drawing;
using System.Windows.Forms;
using TransRail.Domain.Entities;
using TransRail.Domain.Enums;
using TransRail.Presentation.Controls;
using TransRail.Presentation.Presenters;
using TransRail.Presentation.Theme;
using TransRail.Presentation.Views;

namespace TransRail.Presentation.Forms;

public sealed class WagonManagementForm : TransRailFormBase, IWagonView
{
    private readonly TextBox _txtCodigoVagon = new();
    private readonly TextBox _txtCodigoTren = new();
    private readonly ComboBox _cmbTipo = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly NumericUpDown _numCapacidad = new() { Minimum = 1, Maximum = 1000, Value = 80 };
    private readonly NumericUpDown _numPeso = new() { Minimum = 0, Maximum = 200000, DecimalPlaces = 2 };
    private readonly TextBox _txtBusqueda = new();
    private readonly TextBox _txtFiltroTren = new();
    private readonly DataGridView _grid = new();
    private readonly WagonPresenter _presenter;
    private IReadOnlyCollection<Vagon> _cache = Array.Empty<Vagon>();

    public WagonManagementForm()
    {
        Text = "Gesti\u00f3n de vagones";
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(1160, 690);
        MinimumSize = new Size(980, 620);
        BackColor = TransRailTheme.PrimaryDark;
        ForeColor = TransRailTheme.WhiteSoft;
        Font = TransRailTheme.NormalFont;

        _cmbTipo.DataSource = Enum.GetValues<TipoVagon>();

        var split = TransRailFormLayout.CreateModuleSplit(330, 290, 650);
        split.Panel1.Controls.Add(BuildLeftPanel());
        split.Panel2.Controls.Add(BuildGrid());
        Controls.Add(split);

        _presenter = new WagonPresenter(this, AppServices.ManageWagonUseCase);
        Shown += async (_, _) => await _presenter.RefrescarAsync();
    }

    public string CodigoVagon => _txtCodigoVagon.Text.Trim();
    public string CodigoTren => _txtCodigoTren.Text.Trim();
    public TipoVagon TipoVagon => (TipoVagon)_cmbTipo.SelectedItem!;
    public int Capacidad => (int)_numCapacidad.Value;
    public double PesoMaximoKg => (double)_numPeso.Value;
    public string CodigoBusqueda => _txtBusqueda.Text.Trim();
    public string CodigoTrenFiltro => _txtFiltroTren.Text.Trim();

    public event EventHandler? SaveRequested;
    public event EventHandler? DeleteRequested;
    public event EventHandler? SearchRequested;
    public event EventHandler? FilterByTrainRequested;
    public event EventHandler? RefreshRequested;

    public void BindVagones(IReadOnlyCollection<Vagon> vagones)
    {
        _cache = vagones;
        _grid.DataSource = vagones.Select(x => new
        {
            x.CodigoVagon,
            x.CodigoTren,
            Tipo = x.Tipo.ToString(),
            x.Capacidad,
            x.PesoMaximoKg
        }).ToList();
    }

    public void FillForm(Vagon vagon)
    {
        _txtCodigoVagon.Text = vagon.CodigoVagon;
        _txtCodigoTren.Text = vagon.CodigoTren;
        _cmbTipo.SelectedItem = vagon.Tipo;
        _numCapacidad.Value = Math.Clamp(vagon.Capacidad, _numCapacidad.Minimum, _numCapacidad.Maximum);
        _numPeso.Value = Math.Clamp((decimal)vagon.PesoMaximoKg, _numPeso.Minimum, _numPeso.Maximum);
        _txtBusqueda.Text = vagon.CodigoVagon;
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

        panel.Controls.Add(MakeLabel("C\u00f3digo del vag\u00f3n"));
        panel.Controls.Add(_txtCodigoVagon);
        panel.Controls.Add(MakeLabel("C\u00f3digo del tren"));
        panel.Controls.Add(_txtCodigoTren);
        panel.Controls.Add(MakeLabel("Tipo de vag\u00f3n"));
        panel.Controls.Add(_cmbTipo);
        panel.Controls.Add(MakeLabel("Capacidad"));
        panel.Controls.Add(_numCapacidad);
        panel.Controls.Add(MakeLabel("Peso m\u00e1ximo (kg)"));
        panel.Controls.Add(_numPeso);
        panel.Controls.Add(MakeLabel("Buscar por c\u00f3digo"));
        panel.Controls.Add(_txtBusqueda);
        panel.Controls.Add(MakeLabel("Filtrar por tren"));
        panel.Controls.Add(_txtFiltroTren);

        var actions = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = true };

        var btnGuardar = new TransRailButton { Text = "Guardar", Width = 148 };
        btnGuardar.Click += (_, _) => SaveRequested?.Invoke(this, EventArgs.Empty);
        actions.Controls.Add(btnGuardar);

        var btnBuscar = new TransRailButton { Text = "Buscar", Width = 148 };
        btnBuscar.Click += (_, _) => SearchRequested?.Invoke(this, EventArgs.Empty);
        actions.Controls.Add(btnBuscar);

        var btnEliminar = new TransRailButton { Text = "Eliminar", Width = 148 };
        btnEliminar.Click += (_, _) => DeleteRequested?.Invoke(this, EventArgs.Empty);
        actions.Controls.Add(btnEliminar);

        var btnFiltrar = new TransRailButton { Text = "Filtrar tren", Width = 148 };
        btnFiltrar.Click += (_, _) => FilterByTrainRequested?.Invoke(this, EventArgs.Empty);
        actions.Controls.Add(btnFiltrar);

        var btnActualizar = new TransRailButton { Text = "Actualizar lista", Width = 148 };
        btnActualizar.Click += (_, _) => RefreshRequested?.Invoke(this, EventArgs.Empty);
        actions.Controls.Add(btnActualizar);

        panel.Controls.Add(actions);
        panel.Controls.Add(new Label
        {
            Text = "Doble clic en una fila para editar.",
            AutoSize = true,
            ForeColor = TransRailTheme.WhiteSoft,
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
        _grid.CellDoubleClick += (_, e) =>
        {
            if (e.RowIndex < 0 || e.RowIndex >= _grid.Rows.Count)
            {
                return;
            }

            var codigo = _grid.Rows[e.RowIndex].Cells["CodigoVagon"]?.Value?.ToString();
            if (string.IsNullOrWhiteSpace(codigo))
            {
                return;
            }

            var vagon = _cache.FirstOrDefault(x => x.CodigoVagon.Equals(codigo, StringComparison.OrdinalIgnoreCase));
            if (vagon is not null)
            {
                FillForm(vagon);
            }
        };
        return _grid;
    }

    private static Label MakeLabel(string text)
    {
        return new Label
        {
            Text = text,
            AutoSize = true,
            ForeColor = TransRailTheme.WhiteSoft,
            TextAlign = ContentAlignment.BottomLeft
        };
    }
}
