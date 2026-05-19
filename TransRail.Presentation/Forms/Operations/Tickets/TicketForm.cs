using System.Drawing;
using System.Windows.Forms;
using TransRail.Domain.Entities;
using TransRail.Domain.Enums;
using TransRail.Presentation.Controls;
using TransRail.Presentation.Presenters;
using TransRail.Presentation.Theme;
using TransRail.Presentation.Views;

namespace TransRail.Presentation.Forms;

public sealed class TicketForm : TransRailFormBase, ITicketView
{
    private readonly TextBox _txtCodigoBoleto = new();
    private readonly TextBox _txtCodigoPasajero = new();
    private readonly TextBox _txtCodigoHorario = new();
    private readonly ComboBox _cmbTipo = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly NumericUpDown _numDistancia = new() { Minimum = 1, Maximum = 5000, Value = 30 };
    private readonly NumericUpDown _numPrecio = new() { Minimum = 0, Maximum = 1000000, DecimalPlaces = 2, Increment = 100 };
    private readonly TextBox _txtBusqueda = new();
    private readonly DataGridView _grid = new();
    private readonly TicketPresenter _presenter;
    private IReadOnlyCollection<Boleto> _cache = Array.Empty<Boleto>();

    public TicketForm()
    {
        Text = "Gesti\u00f3n de boletos";
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(1180, 700);
        MinimumSize = new Size(990, 620);
        BackColor = TransRailTheme.PrimaryDark;
        ForeColor = TransRailTheme.WhiteSoft;
        Font = TransRailTheme.NormalFont;

        _cmbTipo.DataSource = Enum.GetValues<TipoBoleto>();

        var split = TransRailFormLayout.CreateModuleSplit(350, 320, 680);
        split.Panel1.Controls.Add(BuildLeftPanel());
        split.Panel2.Controls.Add(BuildGrid());
        Controls.Add(split);

        _presenter = new TicketPresenter(this, AppServices.TicketPurchaseUseCase);
        Shown += async (_, _) => await _presenter.RefrescarAsync();
    }

    public string CodigoBoleto => _txtCodigoBoleto.Text.Trim();
    public string CodigoPasajero => _txtCodigoPasajero.Text.Trim();
    public string CodigoHorario => _txtCodigoHorario.Text.Trim();
    public TipoBoleto TipoBoleto => (TipoBoleto)_cmbTipo.SelectedItem!;
    public decimal Precio => _numPrecio.Value;
    public int DistanciaKm => (int)_numDistancia.Value;
    public string CodigoBusqueda => _txtBusqueda.Text.Trim();

    public event EventHandler? SaveRequested;
    public event EventHandler? DeleteRequested;
    public event EventHandler? SearchRequested;
    public event EventHandler? RefreshRequested;
    public event EventHandler? CalculatePriceRequested;
    public event EventHandler? ShowHistoryRequested;
    public event EventHandler? ShowHistoryReverseRequested;

    public void BindBoletos(IReadOnlyCollection<Boleto> boletos)
    {
        _cache = boletos;
        _grid.DataSource = boletos.Select(x => new
        {
            x.CodigoBoleto,
            x.CodigoPasajero,
            x.CodigoHorario,
            Tipo = x.TipoBoleto.ToString(),
            x.Precio,
            FechaCompra = x.FechaCompraUtc.ToLocalTime().ToString("yyyy-MM-dd HH:mm")
        }).ToList();
    }

    public void FillForm(Boleto boleto)
    {
        _txtCodigoBoleto.Text = boleto.CodigoBoleto;
        _txtCodigoPasajero.Text = boleto.CodigoPasajero;
        _txtCodigoHorario.Text = boleto.CodigoHorario;
        _cmbTipo.SelectedItem = boleto.TipoBoleto;
        _numPrecio.Value = Math.Clamp(boleto.Precio, _numPrecio.Minimum, _numPrecio.Maximum);
        _txtBusqueda.Text = boleto.CodigoBoleto;
    }

    public void SetPrecio(decimal precio)
    {
        _numPrecio.Value = Math.Clamp(precio, _numPrecio.Minimum, _numPrecio.Maximum);
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

        panel.Controls.Add(MakeLabel("C\u00f3digo del boleto"));
        panel.Controls.Add(_txtCodigoBoleto);
        panel.Controls.Add(MakeLabel("C\u00f3digo del pasajero"));
        panel.Controls.Add(_txtCodigoPasajero);
        panel.Controls.Add(MakeLabel("C\u00f3digo del horario"));
        panel.Controls.Add(_txtCodigoHorario);
        panel.Controls.Add(MakeLabel("Tipo de boleto"));
        panel.Controls.Add(_cmbTipo);
        panel.Controls.Add(MakeLabel("Distancia (km) para calcular el precio"));
        panel.Controls.Add(_numDistancia);
        panel.Controls.Add(MakeLabel("Precio"));
        panel.Controls.Add(_numPrecio);
        panel.Controls.Add(MakeLabel("Buscar por c\u00f3digo"));
        panel.Controls.Add(_txtBusqueda);

        var actions = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = true };

        var btnCalcular = new TransRailButton { Text = "Calcular precio", Width = 155 };
        btnCalcular.Click += (_, _) => CalculatePriceRequested?.Invoke(this, EventArgs.Empty);
        actions.Controls.Add(btnCalcular);

        var btnGuardar = new TransRailButton { Text = "Guardar", Width = 155 };
        btnGuardar.Click += (_, _) => SaveRequested?.Invoke(this, EventArgs.Empty);
        actions.Controls.Add(btnGuardar);

        var btnBuscar = new TransRailButton { Text = "Buscar", Width = 155 };
        btnBuscar.Click += (_, _) => SearchRequested?.Invoke(this, EventArgs.Empty);
        actions.Controls.Add(btnBuscar);

        var btnEliminar = new TransRailButton { Text = "Eliminar", Width = 155 };
        btnEliminar.Click += (_, _) => DeleteRequested?.Invoke(this, EventArgs.Empty);
        actions.Controls.Add(btnEliminar);

        var btnActualizar = new TransRailButton { Text = "Actualizar lista", Width = 155 };
        btnActualizar.Click += (_, _) => RefreshRequested?.Invoke(this, EventArgs.Empty);
        actions.Controls.Add(btnActualizar);

        var btnHistorial = new TransRailButton { Text = "Ver historial", Width = 155 };
        btnHistorial.Click += (_, _) => ShowHistoryRequested?.Invoke(this, EventArgs.Empty);
        actions.Controls.Add(btnHistorial);

        var btnHistorialRev = new TransRailButton { Text = "Orden inverso", Width = 155 };
        btnHistorialRev.Click += (_, _) => ShowHistoryReverseRequested?.Invoke(this, EventArgs.Empty);
        actions.Controls.Add(btnHistorialRev);

        panel.Controls.Add(actions);
        panel.Controls.Add(new Label
        {
            Text = "Usa la distancia de la ruta para calcular el precio autom\u00e1ticamente.",
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

            var codigo = _grid.Rows[e.RowIndex].Cells["CodigoBoleto"]?.Value?.ToString();
            if (string.IsNullOrWhiteSpace(codigo))
            {
                return;
            }

            var boleto = _cache.FirstOrDefault(x => x.CodigoBoleto.Equals(codigo, StringComparison.OrdinalIgnoreCase));
            if (boleto is not null)
            {
                FillForm(boleto);
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
