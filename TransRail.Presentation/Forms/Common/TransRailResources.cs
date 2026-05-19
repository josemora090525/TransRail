using System.Drawing;
using System.Windows.Forms;

namespace TransRail.Presentation.Forms;

public static class TransRailResources
{
    public static Image? TryLoadBitmap(string fileName, int width = 0, int height = 0)
    {
        var path = ResolvePath(fileName);
        if (path is null)
        {
            return null;
        }

        using var image = Image.FromFile(path);
        if (width > 0 && height > 0)
        {
            return new Bitmap(image, new Size(width, height));
        }

        return new Bitmap(image);
    }

    public static void ApplyButtonImage(Button button, string fileName, int size = 22)
    {
        var image = TryLoadBitmap(fileName, size, size);
        if (image is null)
        {
            return;
        }

        button.Image = image;
        button.ImageAlign = ContentAlignment.MiddleLeft;
        button.TextImageRelation = TextImageRelation.ImageBeforeText;
        button.Padding = new Padding(16, 0, 16, 0);
    }

    public static PictureBox CreatePictureBox(string fileName, int width, int height)
    {
        return new PictureBox
        {
            Width = width,
            Height = height,
            SizeMode = PictureBoxSizeMode.Zoom,
            Image = TryLoadBitmap(fileName, width, height),
            Margin = new Padding(0)
        };
    }

    private static string? ResolvePath(string fileName)
    {
        var candidates = new[]
        {
            Path.Combine(AppContext.BaseDirectory, "Resources", fileName),
            Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Resources", fileName))
        };

        return candidates.FirstOrDefault(File.Exists);
    }
}
