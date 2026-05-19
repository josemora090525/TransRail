using System.Drawing;
using System.Windows.Forms;
using TransRail.Presentation.Controls;
using TransRail.Presentation.Presenters;
using TransRail.Presentation.Theme;
using TransRail.Presentation.Views;

namespace TransRail.Presentation.Forms;

public sealed class LoginForm : TransRailFormBase, ILoginView
{
    private readonly TextBox _txtCorreo = new();
    private readonly TextBox _txtContraseña = new();
    private readonly LoginPresenter _presenter;

    public LoginForm()
    {
        Text = "TransRail - Inicio de sesión";
        Size = new Size(1280, 760);
        MinimumSize = new Size(1040, 700);
        BackColor = TransRailTheme.PrimaryDark;
        ForeColor = TransRailTheme.WhiteSoft;
        Font = TransRailTheme.NormalFont;

        var shell = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 3,
            Padding = new Padding(24)
        };
        shell.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
        shell.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 980f));
        shell.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
        shell.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
        shell.RowStyles.Add(new RowStyle(SizeType.Absolute, 560f));
        shell.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));

        var card = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            BackColor = TransRailTheme.Surface
        };
        card.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 54f));
        card.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 46f));

        card.Controls.Add(BuildHeroPanel(), 0, 0);
        card.Controls.Add(BuildLoginPanel(), 1, 0);

        shell.Controls.Add(card, 1, 1);
        Controls.Add(shell);

        _txtCorreo.PlaceholderText = "correo@transrail.local";
        _txtContraseña.PasswordChar = '*';

        _presenter = new LoginPresenter(this, AppServices.LoginUseCase);
    }

    public string Correo => _txtCorreo.Text.Trim();
    public string Contrasena => _txtContraseña.Text.Trim();
    public event EventHandler? LoginRequested;

    public void ShowMessage(string message)
    {
        MessageBox.Show(this, message, "TransRail", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    public void OpenAdminMenu()
    {
        OpenManagedScreen(new AdminMenuForm());
    }

    public void OpenEmployeeMenu()
    {
        OpenManagedScreen(new EmployeeMenuForm());
    }

    public void OpenPassengerMenu()
    {
        OpenManagedScreen(new PassengerMenuForm());
    }

    private Control BuildHeroPanel()
    {
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = TransRailTheme.PrimaryDark,
            Padding = new Padding(30),
            ColumnCount = 1,
            RowCount = 5
        };
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 92f));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 68f));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 74f));

        panel.Controls.Add(new Label
        {
            Text = "TransRail",
            ForeColor = TransRailTheme.AccentGreen,
            Font = TransRailTheme.HeroFont,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft
        }, 0, 0);

        panel.Controls.Add(new Label
        {
            Text = "Sistema de gestión ferroviaria para operaciones, rutas, horarios, abordaje y equipaje.",
            ForeColor = TransRailTheme.WhiteSoft,
            Font = TransRailTheme.SubtitleFont,
            Dock = DockStyle.Fill
        }, 0, 1);

        panel.Controls.Add(new PictureBox
        {
            Dock = DockStyle.Fill,
            Image = TransRailImages.TryLoad("image 1.png"),
            SizeMode = PictureBoxSizeMode.Zoom,
            Margin = new Padding(0, 12, 0, 12)
        }, 0, 2);

        panel.Controls.Add(new Label
        {
            Text = "Acceso por rol",
            ForeColor = TransRailTheme.AccentGreen,
            Font = TransRailTheme.SectionFont,
            Dock = DockStyle.Fill
        }, 0, 3);

        panel.Controls.Add(new Label
        {
            Text = "Administrador, empleado y pasajero pueden iniciar sesión desde aquí.",
            ForeColor = TransRailTheme.WhiteSoft,
            Font = TransRailTheme.NormalFont,
            Dock = DockStyle.Fill
        }, 0, 4);

        return panel;
    }

    private Control BuildLoginPanel()
    {
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = TransRailTheme.SurfaceAlt,
            Padding = new Padding(32),
            ColumnCount = 1,
            RowCount = 10
        };
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 58f));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 70f));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 28f));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 44f));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 28f));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 44f));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 56f));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 16f));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 108f));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

        panel.Controls.Add(new Label
        {
            Text = "Inicio de sesión",
            ForeColor = TransRailTheme.InkDark,
            Font = TransRailTheme.SectionFont,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft
        }, 0, 0);

        panel.Controls.Add(new Label
        {
            Text = "Ingresa con el correo y la contraseña del rol correspondiente. La sesión abre siempre en pantalla maximizada y conserva el mismo comportamiento al navegar.",
            ForeColor = TransRailTheme.InkDark,
            Font = TransRailTheme.SubtitleFont,
            Dock = DockStyle.Fill
        }, 0, 1);

        panel.Controls.Add(MakeDarkLabel("Correo"), 0, 2);
        _txtCorreo.Dock = DockStyle.Fill;
        panel.Controls.Add(_txtCorreo, 0, 3);

        panel.Controls.Add(MakeDarkLabel("Contraseña"), 0, 4);
        _txtContraseña.Dock = DockStyle.Fill;
        panel.Controls.Add(_txtContraseña, 0, 5);

        var btnLogin = new TransRailButton
        {
            Text = "Iniciar sesión",
            Dock = DockStyle.Fill
        };
        btnLogin.Click += (_, _) => LoginRequested?.Invoke(this, EventArgs.Empty);
        panel.Controls.Add(btnLogin, 0, 6);

        panel.Controls.Add(new Panel { Dock = DockStyle.Fill, Height = 1 }, 0, 7);

        panel.Controls.Add(new Label
        {
            Text = "Las credenciales de acceso se administran fuera de esta pantalla por seguridad.",
            ForeColor = TransRailTheme.InkDark,
            Font = TransRailTheme.NormalFont,
            Dock = DockStyle.Fill,
            Padding = new Padding(0, 8, 0, 0)
        }, 0, 8);

        return panel;
    }

    private static Label MakeDarkLabel(string text)
    {
        return new Label
        {
            Text = text,
            ForeColor = TransRailTheme.InkDark,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.BottomLeft
        };
    }
}
