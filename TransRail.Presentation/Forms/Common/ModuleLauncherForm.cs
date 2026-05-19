using System.Drawing;
using System.Windows.Forms;
using TransRail.Presentation.Controls;
using TransRail.Presentation.Theme;

namespace TransRail.Presentation.Forms;

public abstract class ModuleLauncherForm : TransRailFormBase
{
    private readonly Func<Form> _moduleFactory;

    protected ModuleLauncherForm(string formTitle, string heading, string description, Func<Form> moduleFactory)
    {
        _moduleFactory = moduleFactory;

        Text = formTitle;
        Size = new Size(1180, 760);
        MinimumSize = new Size(960, 640);
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
        shell.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 820f));
        shell.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
        shell.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
        shell.RowStyles.Add(new RowStyle(SizeType.Absolute, 440f));
        shell.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));

        var card = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            BackColor = TransRailTheme.Surface,
            Margin = Padding.Empty
        };
        card.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48f));
        card.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52f));

        card.Controls.Add(BuildHeroPanel(heading), 0, 0);
        card.Controls.Add(BuildContentPanel(heading, description), 1, 0);
        shell.Controls.Add(card, 1, 1);

        Controls.Add(shell);
    }

    private Control BuildHeroPanel(string heading)
    {
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = TransRailTheme.PrimaryDark,
            Padding = new Padding(24),
            RowCount = 4,
            ColumnCount = 1
        };
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 84f));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 64f));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 44f));

        panel.Controls.Add(new Label
        {
            Text = "TransRail",
            Dock = DockStyle.Fill
        }, 0, 0);

        var title = (Label)panel.Controls[0];
        title.ForeColor = TransRailTheme.AccentGreen;
        title.Font = TransRailTheme.HeroFont;
        title.TextAlign = ContentAlignment.MiddleLeft;

        panel.Controls.Add(new Label
        {
            Text = heading,
            Dock = DockStyle.Fill,
            ForeColor = TransRailTheme.WhiteSoft,
            Font = TransRailTheme.SectionFont,
            TextAlign = ContentAlignment.MiddleLeft
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
            Text = "La navegación entre pantallas conserva el modo maximizado y el mismo estilo visual.",
            Dock = DockStyle.Fill,
            ForeColor = TransRailTheme.WhiteSoft,
            Font = TransRailTheme.SubtitleFont,
            TextAlign = ContentAlignment.MiddleLeft
        }, 0, 3);

        return panel;
    }

    private Control BuildContentPanel(string heading, string description)
    {
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = TransRailTheme.SurfaceAlt,
            Padding = new Padding(28),
            RowCount = 6,
            ColumnCount = 1
        };
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 56f));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 108f));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 54f));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 54f));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 36f));

        panel.Controls.Add(new Label
        {
            Text = heading,
            Dock = DockStyle.Fill,
            ForeColor = TransRailTheme.InkDark,
            Font = TransRailTheme.SectionFont,
            TextAlign = ContentAlignment.MiddleLeft
        }, 0, 0);

        panel.Controls.Add(new Label
        {
            Text = description,
            Dock = DockStyle.Fill,
            ForeColor = TransRailTheme.InkDark,
            Font = TransRailTheme.SubtitleFont,
            AutoSize = false
        }, 0, 1);

        panel.Controls.Add(new Label
        {
            Text = "Este formulario se conserva como acceso guiado hacia el módulo funcional principal.",
            Dock = DockStyle.Fill,
            ForeColor = TransRailTheme.InkDark,
            Font = TransRailTheme.NormalFont,
            Padding = new Padding(0, 12, 0, 0)
        }, 0, 2);

        var btnAbrir = new TransRailButton
        {
            Text = "Abrir módulo principal",
            Dock = DockStyle.Fill
        };
        btnAbrir.Click += (_, _) => OpenMainModule();
        panel.Controls.Add(btnAbrir, 0, 3);

        var btnCerrar = new TransRailButton
        {
            Text = "Cerrar",
            Dock = DockStyle.Fill
        };
        btnCerrar.Click += (_, _) => Close();
        panel.Controls.Add(btnCerrar, 0, 4);

        panel.Controls.Add(new Label
        {
            Text = "Los cambios de datos se siguen guardando en los archivos JSON del módulo activo.",
            Dock = DockStyle.Fill,
            ForeColor = TransRailTheme.InkDark,
            Font = TransRailTheme.SubtitleFont
        }, 0, 5);

        return panel;
    }

    private void OpenMainModule()
    {
        OpenManagedDialog(_moduleFactory);
    }
}
