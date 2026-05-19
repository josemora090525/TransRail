using System.Drawing;
using System.Windows.Forms;
using TransRail.Domain.Entities;
using TransRail.Presentation.Controls;
using TransRail.Presentation.Presenters;
using TransRail.Presentation.Theme;
using TransRail.Presentation.Views;

namespace TransRail.Presentation.Forms;

public sealed class EmployeeManagementForm : TransRailFormBase, IEmployeeView
{
    private readonly TextBox _txtCodigo = new();
    private readonly TextBox _txtNombre = new();
    private readonly TextBox _txtDocumento = new();
    private readonly TextBox _txtCorreo = new();
    private readonly TextBox _txtContrasena = new();
    private readonly TextBox _txtBusqueda = new();
    private readonly DataGridView _grid = new();
    private readonly EmployeePresenter _presenter;
    private IReadOnlyCollection<Empleado> _cache = Array.Empty<Empleado>();

    public EmployeeManagementForm()
    {
        Text = "Gesti\u00f3n de empleados";
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(1120, 670);
        MinimumSize = new Size(940, 590);
        BackColor = TransRailTheme.PrimaryDark;
        ForeColor = TransRailTheme.WhiteSoft;
        Font = TransRailTheme.NormalFont;

        _txtContrasena.PasswordChar = '*';

        var split = TransRailFormLayout.CreateModuleSplit(320, 280, 620);
        split.Panel1.Controls.Add(BuildLeftPanel());
        split.Panel2.Controls.Add(BuildGrid());
        Controls.Add(split);

        _presenter = new EmployeePresenter(this, AppServices.ManageEmployeeUseCase);
        Shown += async (_, _) => await _presenter.RefrescarAsync();
    }

    public string CodigoEmpleado => _txtCodigo.Text.Trim();
    public string NombreCompleto => _txtNombre.Text.Trim();
    public string NumeroDocumento => _txtDocumento.Text.Trim();
    public string Correo => _txtCorreo.Text.Trim();
    public string Contrasena => _txtContrasena.Text.Trim();
    public string CodigoBusqueda => _txtBusqueda.Text.Trim();

    public event EventHandler? SaveRequested;
    public event EventHandler? DeleteRequested;
    public event EventHandler? SearchRequested;
    public event EventHandler? RefreshRequested;

    public void BindEmpleados(IReadOnlyCollection<Empleado> empleados)
    {
        _cache = empleados;
        _grid.DataSource = empleados.Select(x => new
        {
            x.CodigoUsuario,
            x.NombreCompleto,
            x.NumeroDocumento,
            x.Correo
        }).ToList();
    }

    public void FillForm(Empleado empleado)
    {
        _txtCodigo.Text = empleado.CodigoUsuario;
        _txtNombre.Text = empleado.NombreCompleto;
        _txtDocumento.Text = empleado.NumeroDocumento;
        _txtCorreo.Text = empleado.Correo;
        _txtContrasena.Text = empleado.Contrasena;
        _txtBusqueda.Text = empleado.CodigoUsuario;
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

        panel.Controls.Add(MakeLabel("C\u00f3digo del empleado"));
        panel.Controls.Add(_txtCodigo);
        panel.Controls.Add(MakeLabel("Nombre completo"));
        panel.Controls.Add(_txtNombre);
        panel.Controls.Add(MakeLabel("N\u00famero de documento"));
        panel.Controls.Add(_txtDocumento);
        panel.Controls.Add(MakeLabel("Correo"));
        panel.Controls.Add(_txtCorreo);
        panel.Controls.Add(MakeLabel("Contrase\u00f1a"));
        panel.Controls.Add(_txtContrasena);
        panel.Controls.Add(MakeLabel("Buscar por c\u00f3digo"));
        panel.Controls.Add(_txtBusqueda);

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

            var codigo = _grid.Rows[e.RowIndex].Cells["CodigoUsuario"]?.Value?.ToString();
            if (string.IsNullOrWhiteSpace(codigo))
            {
                return;
            }

            var empleado = _cache.FirstOrDefault(x => x.CodigoUsuario.Equals(codigo, StringComparison.OrdinalIgnoreCase));
            if (empleado is not null)
            {
                FillForm(empleado);
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
