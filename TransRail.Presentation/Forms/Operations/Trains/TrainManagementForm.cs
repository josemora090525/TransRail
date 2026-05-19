using System.Drawing;
using System.Windows.Forms;
using TransRail.Domain.Entities;
using TransRail.Presentation.Controls;
using TransRail.Presentation.Presenters;
using TransRail.Presentation.Theme;
using TransRail.Presentation.Views;

namespace TransRail.Presentation.Forms;

public sealed class TrainManagementForm : TransRailFormBase, ITrainView
{
    private readonly TextBox _txtCodigo = new();
    private readonly TextBox _txtNumero = new();
    private readonly TextBox _txtNombre = new();
    private readonly NumericUpDown _numCapacidad = new() { Minimum = 1, Maximum = 100 };
    private readonly NumericUpDown _numKm = new() { Minimum = 0, Maximum = 1000000 };
    private readonly CheckBox _chkEnCirculacion = new() { Text = "En circulaci\u00f3n", ForeColor = TransRailTheme.WhiteSoft, AutoSize = true };
    private readonly DataGridView _grid = new();
    private readonly TrainPresenter _presenter;

    public TrainManagementForm()
    {
        Text = "Gesti\u00f3n de trenes";
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(1160, 690);
        MinimumSize = new Size(980, 620);
        BackColor = TransRailTheme.PrimaryDark;
        ForeColor = TransRailTheme.WhiteSoft;
        Font = TransRailTheme.NormalFont;

        var split = TransRailFormLayout.CreateModuleSplit(315, 280, 640);
        split.Panel1.Controls.Add(BuildLeftPanel());
        split.Panel2.Controls.Add(BuildGrid());
        Controls.Add(split);

        _presenter = new TrainPresenter(this, AppServices.ManageTrainUseCase);
        Shown += async (_, _) => await _presenter.RefrescarAsync();
    }

    public string CodigoTren => _txtCodigo.Text.Trim();
    public string NumeroOperativo => _txtNumero.Text.Trim();
    public string NombreTren => _txtNombre.Text.Trim();
    public int CapacidadVagones => (int)_numCapacidad.Value;
    public int Kilometraje => (int)_numKm.Value;
    public bool EnCirculacion => _chkEnCirculacion.Checked;
    public event EventHandler? CreateRequested;
    public event EventHandler? RefreshRequested;

    public void BindTrenes(IReadOnlyCollection<Tren> trenes)
    {
        _grid.DataSource = trenes.Select(x => new
        {
            x.CodigoTren,
            x.NumeroOperativo,
            x.Nombre,
            x.CapacidadVagones,
            x.Kilometraje,
            x.EnCirculacion
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

        panel.Controls.Add(MakeLabel("C\u00f3digo del tren"));
        panel.Controls.Add(_txtCodigo);
        panel.Controls.Add(MakeLabel("N\u00famero operativo"));
        panel.Controls.Add(_txtNumero);
        panel.Controls.Add(MakeLabel("Nombre"));
        panel.Controls.Add(_txtNombre);
        panel.Controls.Add(MakeLabel("Capacidad de vagones"));
        panel.Controls.Add(_numCapacidad);
        panel.Controls.Add(MakeLabel("Kilometraje"));
        panel.Controls.Add(_numKm);
        panel.Controls.Add(_chkEnCirculacion);

        var actions = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = true };
        var btnCrear = new TransRailButton { Text = "Guardar tren", Width = 148 };
        btnCrear.Click += (_, _) => CreateRequested?.Invoke(this, EventArgs.Empty);
        actions.Controls.Add(btnCrear);

        var btnRefresh = new TransRailButton { Text = "Actualizar lista", Width = 148 };
        btnRefresh.Click += (_, _) => RefreshRequested?.Invoke(this, EventArgs.Empty);
        actions.Controls.Add(btnRefresh);

        panel.Controls.Add(actions);
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
