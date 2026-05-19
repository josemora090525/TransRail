using System.Drawing;
using System.Windows.Forms;
using TransRail.Domain.Entities;
using TransRail.Presentation.Controls;
using TransRail.Presentation.Presenters;
using TransRail.Presentation.Theme;
using TransRail.Presentation.Views;

namespace TransRail.Presentation.Forms;

public sealed class PassengerProfileForm : TransRailFormBase, IPassengerProfileView
{
    private readonly TextBox _txtNombres = new();
    private readonly TextBox _txtApellidos = new();
    private readonly TextBox _txtCorreo = new();
    private readonly TextBox _txtDireccion = new() { Multiline = true, Height = 82, ScrollBars = ScrollBars.Vertical };
    private readonly ComboBox _cmbTipoIdentificacion = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly TextBox _txtNumeroIdentificacion = new();
    private readonly TextBox _txtTelefono = new();
    private readonly TextBox _txtNombreContacto = new();
    private readonly TextBox _txtApellidoContacto = new();
    private readonly TextBox _txtTelefonoContacto = new();
    private readonly TextBox _txtEquipajeDeMano = new() { Multiline = true, Height = 82, ScrollBars = ScrollBars.Vertical };
    private readonly CheckBox _chkAdultoMayor = new() { Text = "Adulto mayor", ForeColor = TransRailTheme.WhiteSoft, AutoSize = true };
    private readonly CheckBox _chkDiscapacidad = new() { Text = "Discapacidad", ForeColor = TransRailTheme.WhiteSoft, AutoSize = true };
    private readonly Label _lblResumen = new() { AutoSize = true, ForeColor = TransRailTheme.InkDark, Font = TransRailTheme.NormalFont };
    private readonly PassengerProfilePresenter _presenter;

    public PassengerProfileForm()
    {
        Text = "Mis datos";
        Size = new Size(1200, 740);
        MinimumSize = new Size(1020, 660);
        BackColor = TransRailTheme.PrimaryDark;
        ForeColor = TransRailTheme.WhiteSoft;
        Font = TransRailTheme.NormalFont;

        _cmbTipoIdentificacion.Items.AddRange(["CC", "TI", "CE", "Pasaporte"]);

        var split = TransRailFormLayout.CreateModuleSplit(340, 320, 700);
        split.Panel1.Controls.Add(BuildFormPanel());
        split.Panel2.Controls.Add(BuildSummaryPanel());
        Controls.Add(split);

        _presenter = new PassengerProfilePresenter(this, AppServices.PassengerPortalUseCase, AppServices.UserSession);
        Shown += async (_, _) => await _presenter.LoadAsync();
    }

    public string Nombres => _txtNombres.Text.Trim();
    public string Apellidos => _txtApellidos.Text.Trim();
    public string Correo => _txtCorreo.Text.Trim();
    public string Direccion => _txtDireccion.Text.Trim();
    public string TipoIdentificacion => _cmbTipoIdentificacion.SelectedItem?.ToString() ?? "CC";
    public string NumeroIdentificacion => _txtNumeroIdentificacion.Text.Trim();
    public string Telefono => _txtTelefono.Text.Trim();
    public string NombreContacto => _txtNombreContacto.Text.Trim();
    public string ApellidoContacto => _txtApellidoContacto.Text.Trim();
    public string TelefonoContacto => _txtTelefonoContacto.Text.Trim();
    public string EquipajeDeMano => _txtEquipajeDeMano.Text.Trim();
    public bool EsAdultoMayor => _chkAdultoMayor.Checked;
    public bool TieneDiscapacidad => _chkDiscapacidad.Checked;

    public event EventHandler? SaveRequested;

    public void LoadPassenger(Pasajero pasajero)
    {
        _txtNombres.Text = pasajero.Nombres;
        _txtApellidos.Text = pasajero.Apellidos;
        _txtCorreo.Text = pasajero.Correo;
        _txtDireccion.Text = pasajero.Direccion;
        _cmbTipoIdentificacion.SelectedItem = pasajero.TipoIdentificacion;
        if (_cmbTipoIdentificacion.SelectedIndex < 0)
        {
            _cmbTipoIdentificacion.SelectedItem = "CC";
        }

        _txtNumeroIdentificacion.Text = pasajero.NumeroDocumento;
        _txtTelefono.Text = pasajero.Telefono;
        _txtNombreContacto.Text = pasajero.NombreContacto;
        _txtApellidoContacto.Text = pasajero.ApellidoContacto;
        _txtTelefonoContacto.Text = pasajero.TelefonoContacto;
        _txtEquipajeDeMano.Text = pasajero.EquipajeDeMano;
        _chkAdultoMayor.Checked = pasajero.EsAdultoMayor;
        _chkDiscapacidad.Checked = pasajero.TieneDiscapacidad;

        _lblResumen.Text =
            $"Nombre completo\n{pasajero.NombreCompleto}\n\n" +
            $"Correo\n{pasajero.Correo}\n\n" +
            $"Identificaci\u00f3n\n{pasajero.TipoIdentificacion} {pasajero.NumeroDocumento}\n\n" +
            $"Tel\u00e9fono\n{pasajero.Telefono}\n\n" +
            $"Contacto de emergencia\n{pasajero.NombreContacto} {pasajero.ApellidoContacto}\n{pasajero.TelefonoContacto}\n\n" +
            $"Equipaje de mano\n{(string.IsNullOrWhiteSpace(pasajero.EquipajeDeMano) ? "Sin registrar" : pasajero.EquipajeDeMano)}";
    }

    public void ShowMessage(string message)
    {
        MessageBox.Show(this, message, "TransRail", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private Control BuildFormPanel()
    {
        var form = new TableLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 1,
            Width = 300,
            Margin = Padding.Empty
        };

        form.Controls.Add(MakeLabel("Nombres"));
        form.Controls.Add(BuildInput(_txtNombres));
        form.Controls.Add(MakeLabel("Apellidos"));
        form.Controls.Add(BuildInput(_txtApellidos));
        form.Controls.Add(MakeLabel("Correo"));
        form.Controls.Add(BuildInput(_txtCorreo));
        form.Controls.Add(MakeLabel("Direcci\u00f3n"));
        form.Controls.Add(BuildInput(_txtDireccion));
        form.Controls.Add(MakeLabel("Tipo de identificaci\u00f3n"));
        form.Controls.Add(BuildInput(_cmbTipoIdentificacion));
        form.Controls.Add(MakeLabel("N\u00famero de identificaci\u00f3n"));
        form.Controls.Add(BuildInput(_txtNumeroIdentificacion));
        form.Controls.Add(MakeLabel("N\u00famero de tel\u00e9fono"));
        form.Controls.Add(BuildInput(_txtTelefono));
        form.Controls.Add(MakeLabel("Nombre del contacto"));
        form.Controls.Add(BuildInput(_txtNombreContacto));
        form.Controls.Add(MakeLabel("Apellido del contacto"));
        form.Controls.Add(BuildInput(_txtApellidoContacto));
        form.Controls.Add(MakeLabel("N\u00famero del contacto"));
        form.Controls.Add(BuildInput(_txtTelefonoContacto));
        form.Controls.Add(MakeLabel("Equipaje de mano que llevar\u00e1s"));
        form.Controls.Add(BuildInput(_txtEquipajeDeMano));
        _chkAdultoMayor.Margin = new Padding(0, 4, 0, 2);
        _chkDiscapacidad.Margin = new Padding(0, 2, 0, 8);
        form.Controls.Add(_chkAdultoMayor);
        form.Controls.Add(_chkDiscapacidad);

        var btnGuardar = new TransRailButton
        {
            Text = "Guardar mis datos",
            Width = 300,
            Height = 46,
            Margin = new Padding(0, 8, 0, 0)
        };
        btnGuardar.Click += (_, _) => SaveRequested?.Invoke(this, EventArgs.Empty);
        form.Controls.Add(btnGuardar);

        return TransRailFormLayout.CreateCenteredScrollHost(form, TransRailTheme.Surface, 300);
    }

    private Control BuildSummaryPanel()
    {
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = TransRailTheme.SurfaceAlt,
            Padding = new Padding(22),
            ColumnCount = 1,
            RowCount = 3
        };
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 96));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        panel.Controls.Add(new Label
        {
            Text = "Tu perfil de viaje",
            Dock = DockStyle.Fill,
            Font = TransRailTheme.SectionFont,
            ForeColor = TransRailTheme.InkDark,
            TextAlign = ContentAlignment.MiddleLeft
        }, 0, 0);

        panel.Controls.Add(new Label
        {
            Text = "Aqu\u00ed puedes mantener actualizada tu informaci\u00f3n personal, el contacto de emergencia y el equipaje de mano para futuras compras.",
            Dock = DockStyle.Fill,
            ForeColor = TransRailTheme.InkDark,
            Font = TransRailTheme.SubtitleFont
        }, 0, 1);

        var card = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Padding = new Padding(20)
        };
        var stack = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true
        };
        stack.Controls.Add(_lblResumen);
        card.Controls.Add(stack);
        panel.Controls.Add(card, 0, 2);
        TransRailFormLayout.BindWrapWidth(_lblResumen, stack, 10);

        return panel;
    }

    private static Control BuildInput(Control control)
    {
        control.Width = 300;
        control.Margin = new Padding(0, 4, 0, 10);
        return control;
    }

    private static Label MakeLabel(string text)
    {
        return new Label
        {
            Text = text,
            ForeColor = TransRailTheme.WhiteSoft,
            AutoSize = true,
            Margin = new Padding(0)
        };
    }
}
