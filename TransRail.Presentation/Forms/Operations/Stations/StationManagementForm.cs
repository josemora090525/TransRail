using System.Drawing;
using System.Windows.Forms;
using TransRail.Domain.Entities;
using TransRail.Presentation.Controls;
using TransRail.Presentation.Presenters;
using TransRail.Presentation.Theme;
using TransRail.Presentation.Views;

namespace TransRail.Presentation.Forms;

public sealed class StationManagementForm : TransRailFormBase, IStationView
{
    private readonly TextBox _txtCodigo = new();
    private readonly TextBox _txtNombre = new();
    private readonly TextBox _txtCiudad = new();
    private readonly DataGridView _grid = new();
    private readonly StationPresenter _presenter;

    public StationManagementForm()
    {
        Text = "Gesti\u00f3n de estaciones";
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(1080, 660);
        MinimumSize = new Size(920, 580);
        BackColor = TransRailTheme.PrimaryDark;
        ForeColor = TransRailTheme.WhiteSoft;
        Font = TransRailTheme.NormalFont;

        var split = TransRailFormLayout.CreateModuleSplit(300, 260, 600);
        split.Panel1.Controls.Add(BuildLeftPanel());
        split.Panel2.Controls.Add(BuildGrid());
        Controls.Add(split);

        _presenter = new StationPresenter(this, AppServices.ManageStationUseCase);
        Shown += async (_, _) => await _presenter.RefrescarAsync();
    }

    public string CodigoEstacion => _txtCodigo.Text.Trim().ToUpperInvariant();
    public string NombreEstacion => _txtNombre.Text.Trim();
    public string CiudadEstacion => _txtCiudad.Text.Trim();
    public event EventHandler? CreateRequested;
    public event EventHandler? RefreshRequested;

    public void BindEstaciones(IReadOnlyCollection<Estacion> estaciones)
    {
        _grid.DataSource = estaciones.Select(x => new
        {
            x.CodigoEstacion,
            x.Nombre,
            x.Ciudad
        }).ToList();
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

        panel.Controls.Add(MakeLabel("C\u00f3digo de la estaci\u00f3n"));
        panel.Controls.Add(_txtCodigo);
        panel.Controls.Add(MakeLabel("Nombre"));
        panel.Controls.Add(_txtNombre);
        panel.Controls.Add(MakeLabel("Ciudad"));
        panel.Controls.Add(_txtCiudad);

        var actions = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = true };
        var btnGuardar = new TransRailButton { Text = "Guardar estaci\u00f3n", Width = 150 };
        btnGuardar.Click += (_, _) => CreateRequested?.Invoke(this, EventArgs.Empty);
        actions.Controls.Add(btnGuardar);
        var btnRefresh = new TransRailButton { Text = "Actualizar lista", Width = 150 };
        btnRefresh.Click += (_, _) => RefreshRequested?.Invoke(this, EventArgs.Empty);
        actions.Controls.Add(btnRefresh);
        panel.Controls.Add(actions);

        panel.Controls.Add(new Label
        {
            Text = "Sugerencia: conserva los c\u00f3digos A..K para la matriz de distancias.",
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
