using System.Drawing;
using System.Windows.Forms;
using TransRail.Presentation.Controls;
using TransRail.Presentation.Theme;

namespace TransRail.Presentation.Forms;

public abstract class WorkspaceMenuFormBase : TransRailFormBase
{
    private readonly Label _titleLabel;
    private readonly Label _subtitleLabel;
    private readonly PictureBox _headerImage;
    private readonly Label _workspaceTitleLabel;
    private readonly FlowLayoutPanel _modulePanel;
    private readonly Panel _workspacePanel;
    private readonly Label _workspaceInfoLabel;
    private string _welcomeTitle = string.Empty;
    private string _welcomeMessage = string.Empty;
    private Form? _activeModule;

    protected WorkspaceMenuFormBase(string windowTitle)
    {
        Text = windowTitle;
        Size = new Size(1520, 900);
        MinimumSize = new Size(1260, 760);
        BackColor = TransRailTheme.PrimaryDark;
        ForeColor = TransRailTheme.WhiteSoft;
        Font = TransRailTheme.NormalFont;

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            Padding = new Padding(18)
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 384f));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

        var sidebar = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = TransRailTheme.Surface,
            Padding = new Padding(18),
            ColumnCount = 1,
            RowCount = 4,
            Margin = new Padding(0, 0, 12, 0)
        };
        sidebar.RowStyles.Add(new RowStyle(SizeType.Absolute, 184f));
        sidebar.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        sidebar.RowStyles.Add(new RowStyle(SizeType.Absolute, 56f));
        sidebar.RowStyles.Add(new RowStyle(SizeType.Absolute, 56f));

        var header = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2
        };
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 64f));

        var headerText = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2
        };
        headerText.RowStyles.Add(new RowStyle(SizeType.Absolute, 56f));
        headerText.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

        _titleLabel = new Label
        {
            Dock = DockStyle.Fill,
            ForeColor = TransRailTheme.AccentGreen,
            Font = TransRailTheme.MenuTitleFont,
            TextAlign = ContentAlignment.MiddleLeft,
            AutoEllipsis = false
        };
        _subtitleLabel = new Label
        {
            Dock = DockStyle.Fill,
            ForeColor = TransRailTheme.WhiteSoft,
            Font = TransRailTheme.SubtitleFont
        };

        _headerImage = new PictureBox
        {
            Dock = DockStyle.Fill,
            SizeMode = PictureBoxSizeMode.Zoom,
            Margin = new Padding(6, 0, 0, 0)
        };

        headerText.Controls.Add(_titleLabel, 0, 0);
        headerText.Controls.Add(_subtitleLabel, 0, 1);
        header.Controls.Add(headerText, 0, 0);
        header.Controls.Add(_headerImage, 1, 0);
        sidebar.Controls.Add(header, 0, 0);

        _modulePanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            WrapContents = true,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(0, 10, 0, 0)
        };
        sidebar.Controls.Add(_modulePanel, 0, 1);

        var btnInicio = new TransRailButton
        {
            Text = "Inicio",
            Dock = DockStyle.Fill
        };
        btnInicio.Click += (_, _) => ShowHome();
        sidebar.Controls.Add(btnInicio, 0, 2);

        var btnCerrarSesion = new TransRailButton
        {
            Text = "Cerrar sesi\u00f3n",
            Dock = DockStyle.Fill
        };
        btnCerrarSesion.Click += (_, _) =>
        {
            AppServices.UserSession.Clear();
            Close();
        };
        sidebar.Controls.Add(btnCerrarSesion, 0, 3);

        var workspaceCard = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = TransRailTheme.SurfaceAlt,
            Padding = new Padding(18),
            ColumnCount = 1,
            RowCount = 2
        };
        workspaceCard.RowStyles.Add(new RowStyle(SizeType.Absolute, 68f));
        workspaceCard.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

        _workspaceTitleLabel = new Label
        {
            Dock = DockStyle.Fill,
            ForeColor = TransRailTheme.InkDark,
            Font = TransRailTheme.SectionFont,
            TextAlign = ContentAlignment.MiddleLeft
        };
        workspaceCard.Controls.Add(_workspaceTitleLabel, 0, 0);

        _workspacePanel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White
        };

        _workspaceInfoLabel = new Label
        {
            Dock = DockStyle.Fill,
            ForeColor = TransRailTheme.InkDark,
            Font = TransRailTheme.SubtitleFont,
            TextAlign = ContentAlignment.MiddleCenter
        };
        _workspacePanel.Controls.Add(_workspaceInfoLabel);
        workspaceCard.Controls.Add(_workspacePanel, 0, 1);

        root.Controls.Add(sidebar, 0, 0);
        root.Controls.Add(workspaceCard, 1, 0);
        Controls.Add(root);
    }

    protected void ConfigureHeader(string title, string subtitle, string imageName)
    {
        _titleLabel.Text = title;
        _subtitleLabel.Text = subtitle;
        _headerImage.Image = TransRailImages.TryLoad(imageName, new Size(56, 56));
    }

    protected void AddModule(string text, string imageName, Func<Form> formFactory)
    {
        var button = new TransRailButton
        {
            Text = text,
            Size = new Size(154, 114),
            Margin = new Padding(0, 0, 12, 12),
            TextImageRelation = TextImageRelation.ImageAboveText,
            TextAlign = ContentAlignment.BottomCenter,
            ImageAlign = ContentAlignment.TopCenter,
            Image = TransRailImages.TryLoad(imageName, new Size(52, 52)),
            Font = TransRailTheme.MenuButtonFont,
            Padding = new Padding(8, 10, 8, 12)
        };

        button.Click += (_, _) => ShowEmbeddedModule(text, formFactory);
        _modulePanel.Controls.Add(button);
    }

    protected void SetWelcomeMessage(string title, string message)
    {
        _welcomeTitle = title;
        _welcomeMessage = message;
        _workspaceTitleLabel.Text = title;
        _workspaceInfoLabel.Text = message;
    }

    protected void ShowHome()
    {
        if (_activeModule is not null)
        {
            var module = _activeModule;
            _activeModule = null;
            module.Close();
        }

        _workspaceTitleLabel.Text = _welcomeTitle;
        _workspaceInfoLabel.Text = _welcomeMessage;
        _workspacePanel.Controls.Clear();
        _workspacePanel.Controls.Add(_workspaceInfoLabel);
    }

    private void ShowEmbeddedModule(string title, Func<Form> formFactory)
    {
        if (_activeModule is not null)
        {
            var previous = _activeModule;
            _activeModule = null;
            previous.Close();
        }

        var module = formFactory();
        module.TopLevel = false;
        module.FormBorderStyle = FormBorderStyle.None;
        module.Dock = DockStyle.Fill;
        module.ShowInTaskbar = false;
        module.WindowState = FormWindowState.Normal;
        module.FormClosed += (_, _) =>
        {
            if (ReferenceEquals(_activeModule, module))
            {
                _activeModule = null;
                ShowHome();
            }
        };

        _workspacePanel.Controls.Clear();
        _workspaceTitleLabel.Text = title;
        _workspacePanel.Controls.Add(module);
        _activeModule = module;
        module.Show();
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        if (_activeModule is not null && !_activeModule.IsDisposed)
        {
            _activeModule.Dispose();
            _activeModule = null;
        }

        AppServices.UserSession.Clear();
        base.OnFormClosed(e);
    }
}
