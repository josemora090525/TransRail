using System.Drawing;
using System.Windows.Forms;
using TransRail.Domain.Entities;
using TransRail.Presentation.Controls;
using TransRail.Presentation.Presenters;
using TransRail.Presentation.Theme;
using TransRail.Presentation.Views;

namespace TransRail.Presentation.Forms;

public sealed class RouteManagementForm : TransRailFormBase, IRouteView
{
    private readonly TextBox _txtCodigo = new();
    private readonly TextBox _txtOrigen = new();
    private readonly TextBox _txtDestino = new();
    private readonly NumericUpDown _numDistancia = new() { Minimum = 1, Maximum = 5000, Value = 30 };
    private readonly TextBox _txtCalcOrigen = new();
    private readonly TextBox _txtCalcDestino = new();
    private readonly Label _lblCalculo = new() { ForeColor = TransRailTheme.WhiteSoft, AutoSize = true };
    private readonly DataGridView _grid = new();
    private readonly RoutePresenter _presenter;

    public RouteManagementForm()
    {
        Text = "Gesti\u00f3n de rutas";
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(1220, 720);
        MinimumSize = new Size(1020, 630);
        BackColor = TransRailTheme.PrimaryDark;
        ForeColor = TransRailTheme.WhiteSoft;
        Font = TransRailTheme.NormalFont;

        var split = TransRailFormLayout.CreateModuleSplit(365, 320, 700);
        split.Panel1.Controls.Add(BuildLeftPanel());
        split.Panel2.Controls.Add(BuildGrid());
        Controls.Add(split);

        _presenter = new RoutePresenter(this, AppServices.ManageRouteUseCase);
        Shown += async (_, _) => await _presenter.RefrescarAsync();
    }

    public string CodigoRuta => _txtCodigo.Text.Trim();
    public string CodigoOrigen => _txtOrigen.Text.Trim().ToUpperInvariant();
    public string CodigoDestino => _txtDestino.Text.Trim().ToUpperInvariant();
    public int DistanciaKm => (int)_numDistancia.Value;
    public string CalculoOrigen => _txtCalcOrigen.Text.Trim().ToUpperInvariant();
    public string CalculoDestino => _txtCalcDestino.Text.Trim().ToUpperInvariant();
    public event EventHandler? CreateRequested;
    public event EventHandler? RefreshRequested;
    public event EventHandler? CalculateRequested;

    public void BindRutas(IReadOnlyCollection<Ruta> rutas)
    {
        _grid.DataSource = rutas.Select(x => new
        {
            x.CodigoRuta,
            Origen = x.CodigoEstacionOrigen,
            Destino = x.CodigoEstacionDestino,
            x.DistanciaKm,
            x.Activa
        }).ToList();
    }

    public void ShowRouteCalculation(int distancia, IReadOnlyList<string> ruta)
    {
        if (distancia == int.MaxValue || ruta.Count == 0)
        {
            _lblCalculo.Text = "No hay una ruta disponible para esos c\u00f3digos.";
            return;
        }

        _lblCalculo.Text = $"Distancia m\u00ednima: {distancia} km | Ruta: {string.Join(" -> ", ruta)}";
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

        panel.Controls.Add(MakeLabel("C\u00f3digo de la ruta"));
        panel.Controls.Add(_txtCodigo);
        panel.Controls.Add(MakeLabel("C\u00f3digo de estaci\u00f3n de origen (A..K)"));
        panel.Controls.Add(_txtOrigen);
        panel.Controls.Add(MakeLabel("C\u00f3digo de estaci\u00f3n de destino (A..K)"));
        panel.Controls.Add(_txtDestino);
        panel.Controls.Add(MakeLabel("Distancia (km)"));
        panel.Controls.Add(_numDistancia);

        var actions = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = true };
        var btnGuardar = new TransRailButton { Text = "Guardar ruta", Width = 158 };
        btnGuardar.Click += (_, _) => CreateRequested?.Invoke(this, EventArgs.Empty);
        actions.Controls.Add(btnGuardar);

        var btnRefresh = new TransRailButton { Text = "Actualizar lista", Width = 158 };
        btnRefresh.Click += (_, _) => RefreshRequested?.Invoke(this, EventArgs.Empty);
        actions.Controls.Add(btnRefresh);
        panel.Controls.Add(actions);

        panel.Controls.Add(MakeLabel("C\u00e1lculo: origen (A..K)"));
        panel.Controls.Add(_txtCalcOrigen);
        panel.Controls.Add(MakeLabel("C\u00e1lculo: destino (A..K)"));
        panel.Controls.Add(_txtCalcDestino);
        var btnCalc = new TransRailButton { Text = "Calcular ruta corta", Width = 158 };
        btnCalc.Click += (_, _) => CalculateRequested?.Invoke(this, EventArgs.Empty);
        panel.Controls.Add(btnCalc);
        panel.Controls.Add(_lblCalculo);

        panel.Controls.Add(MakeLabel("Matriz base A-K (km)"));
        panel.Controls.Add(new TextBox
        {
            ReadOnly = true,
            Multiline = true,
            Height = 160,
            Dock = DockStyle.Top,
            Text = "A-B:30 | A-C:40 | A-D:50 | A-F:50\r\n" +
                   "D-E:20 | E-F:65 | F-G:80 | G-H:30\r\n" +
                   "G-I:145 | C-I:80 | C-J:120 | C-K:110\r\n" +
                   "El grafo es no dirigido (ida y vuelta).",
            BackColor = Color.White,
            ForeColor = Color.Black,
            ScrollBars = ScrollBars.Vertical
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
