using System.Drawing;
using System.Windows.Forms;

namespace TransRail.Presentation.Forms;

public static class TransRailWindowStateManager
{
    private static Rectangle _lastNormalBounds = Rectangle.Empty;

    public static void Attach(Form form)
    {
        form.Load += (_, _) => ApplyPreferredState(form);
        form.Resize += (_, _) => Capture(form);
        form.Move += (_, _) => Capture(form);
        form.Shown += (_, _) => Capture(form);
    }

    public static void ApplyPreferredState(Form form)
    {
        if (!form.TopLevel)
        {
            return;
        }

        if (_lastNormalBounds != Rectangle.Empty)
        {
            form.Bounds = _lastNormalBounds;
        }

        // Every screen opens maximized when navigating between modules.
        form.WindowState = FormWindowState.Maximized;
    }

    private static void Capture(Form form)
    {
        if (!form.TopLevel)
        {
            return;
        }

        if (form.WindowState == FormWindowState.Normal)
        {
            _lastNormalBounds = form.Bounds;
        }
    }
}
