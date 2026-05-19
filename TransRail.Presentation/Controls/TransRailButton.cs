using System.Drawing;
using System.Windows.Forms;
using TransRail.Presentation.Theme;

namespace TransRail.Presentation.Controls;

public sealed class TransRailButton : Button
{
    public TransRailButton()
    {
        BackColor = TransRailTheme.AccentGreen;
        ForeColor = Color.Black;
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        FlatAppearance.MouseOverBackColor = Color.FromArgb(186, 255, 71);
        FlatAppearance.MouseDownBackColor = Color.FromArgb(143, 226, 15);
        Font = new Font(TransRailTheme.NormalFont, FontStyle.Bold);
        Height = 42;
        Width = 150;
        Padding = new Padding(12, 8, 12, 8);
        TextAlign = ContentAlignment.MiddleCenter;
        AutoEllipsis = false;
        UseVisualStyleBackColor = false;
    }
}
