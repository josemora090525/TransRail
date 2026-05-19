using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TransRail.Presentation.Theme;

public static class TransRailImages
{
    private static readonly ConcurrentDictionary<string, Image?> Cache = new(StringComparer.OrdinalIgnoreCase);

    public static Image? TryLoad(string fileName)
    {
        return TryLoad(fileName, null);
    }

    public static Image? TryLoad(string fileName, Size? desiredSize)
    {
        var sizeSuffix = desiredSize is null ? string.Empty : $"|{desiredSize.Value.Width}x{desiredSize.Value.Height}";
        var cacheKey = $"{fileName}{sizeSuffix}";
        return Cache.GetOrAdd(cacheKey, static key => LoadFromCacheKey(key));
    }

    private static Image? LoadFromCacheKey(string cacheKey)
    {
        var parts = cacheKey.Split('|', StringSplitOptions.RemoveEmptyEntries);
        var fileName = parts[0];
        Size? desiredSize = null;

        if (parts.Length > 1)
        {
            var dimensions = parts[1].Split('x', StringSplitOptions.RemoveEmptyEntries);
            if (dimensions.Length == 2 &&
                int.TryParse(dimensions[0], out var width) &&
                int.TryParse(dimensions[1], out var height))
            {
                desiredSize = new Size(width, height);
            }
        }

        if (fileName.Equals("ticket.generated", StringComparison.OrdinalIgnoreCase))
        {
            return CreateTicketIcon(desiredSize ?? new Size(56, 56));
        }

        try
        {
            var fullPath = Path.Combine(AppContext.BaseDirectory, "Resources", fileName);
            if (!File.Exists(fullPath))
            {
                return null;
            }

            using var stream = File.OpenRead(fullPath);
            using var original = Image.FromStream(stream);
            return desiredSize is null
                ? new Bitmap(original)
                : new Bitmap(original, desiredSize.Value);
        }
        catch
        {
            return null;
        }
    }

    private static Image CreateTicketIcon(Size desiredSize)
    {
        var bitmap = new Bitmap(desiredSize.Width, desiredSize.Height);
        using var graphics = Graphics.FromImage(bitmap);
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        graphics.Clear(Color.Transparent);

        using var ticketBrush = new SolidBrush(Color.White);
        using var detailPen = new Pen(Color.FromArgb(28, 66, 85), Math.Max(2f, desiredSize.Width / 20f))
        {
            DashStyle = DashStyle.Dash
        };
        using var accentBrush = new SolidBrush(Color.FromArgb(28, 66, 85));

        var rect = new RectangleF(
            desiredSize.Width * 0.12f,
            desiredSize.Height * 0.2f,
            desiredSize.Width * 0.76f,
            desiredSize.Height * 0.6f);

        using var path = new GraphicsPath();
        path.StartFigure();
        path.AddArc(rect.Left, rect.Top, desiredSize.Width * 0.18f, desiredSize.Height * 0.18f, 180, 90);
        path.AddLine(rect.Left + desiredSize.Width * 0.09f, rect.Top, rect.Right - desiredSize.Width * 0.09f, rect.Top);
        path.AddArc(rect.Right - desiredSize.Width * 0.18f, rect.Top, desiredSize.Width * 0.18f, desiredSize.Height * 0.18f, 270, 90);
        path.AddLine(rect.Right, rect.Top + desiredSize.Height * 0.09f, rect.Right, rect.Bottom - desiredSize.Height * 0.09f);
        path.AddArc(rect.Right - desiredSize.Width * 0.18f, rect.Bottom - desiredSize.Height * 0.18f, desiredSize.Width * 0.18f, desiredSize.Height * 0.18f, 0, 90);
        path.AddLine(rect.Right - desiredSize.Width * 0.09f, rect.Bottom, rect.Left + desiredSize.Width * 0.09f, rect.Bottom);
        path.AddArc(rect.Left, rect.Bottom - desiredSize.Height * 0.18f, desiredSize.Width * 0.18f, desiredSize.Height * 0.18f, 90, 90);
        path.CloseFigure();

        graphics.FillPath(ticketBrush, path);

        var previousMode = graphics.CompositingMode;
        graphics.CompositingMode = CompositingMode.SourceCopy;
        graphics.FillEllipse(
            Brushes.Transparent,
            rect.Left - desiredSize.Width * 0.05f,
            desiredSize.Height * 0.41f,
            desiredSize.Width * 0.1f,
            desiredSize.Height * 0.18f);
        graphics.FillEllipse(
            Brushes.Transparent,
            rect.Right - desiredSize.Width * 0.05f,
            desiredSize.Height * 0.41f,
            desiredSize.Width * 0.1f,
            desiredSize.Height * 0.18f);
        graphics.CompositingMode = previousMode;

        graphics.DrawLine(
            detailPen,
            desiredSize.Width * 0.34f,
            desiredSize.Height * 0.26f,
            desiredSize.Width * 0.34f,
            desiredSize.Height * 0.74f);
        graphics.FillEllipse(
            accentBrush,
            desiredSize.Width * 0.52f,
            desiredSize.Height * 0.38f,
            desiredSize.Width * 0.18f,
            desiredSize.Height * 0.18f);
        graphics.FillRectangle(
            accentBrush,
            desiredSize.Width * 0.52f,
            desiredSize.Height * 0.62f,
            desiredSize.Width * 0.18f,
            desiredSize.Height * 0.06f);

        return bitmap;
    }
}
