using System.Drawing;

namespace TransRail.Presentation.Theme;

public static class TransRailTheme
{
    public static readonly Color PrimaryDark = Color.FromArgb(13, 43, 58);
    public static readonly Color AccentGreen = Color.FromArgb(157, 255, 0);
    public static readonly Color WhiteSoft = Color.FromArgb(245, 248, 250);
    public static readonly Color Surface = Color.FromArgb(28, 66, 85);
    public static readonly Color SurfaceAlt = Color.FromArgb(234, 239, 242);
    public static readonly Color InkDark = Color.FromArgb(22, 29, 33);
    public static readonly Font TitleFont = new("Segoe UI", 20, FontStyle.Bold);
    public static readonly Font MenuTitleFont = new("Segoe UI", 20, FontStyle.Bold);
    public static readonly Font MenuButtonFont = new("Segoe UI", 10.5f, FontStyle.Bold);
    public static readonly Font HeroFont = new("Segoe UI", 28, FontStyle.Bold);
    public static readonly Font SectionFont = new("Segoe UI", 14, FontStyle.Bold);
    public static readonly Font SubtitleFont = new("Segoe UI", 11, FontStyle.Regular);
    public static readonly Font NormalFont = new("Segoe UI", 10, FontStyle.Regular);
}
