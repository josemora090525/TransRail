using System.Drawing;
using System.Windows.Forms;
using TransRail.Presentation.Theme;

namespace TransRail.Presentation.Controls;

public static class TransRailFormLayout
{
    public static SplitContainer CreateModuleSplit(int leftPanelWidth, int leftPanelMinWidth, int rightPanelMinWidth = 600)
    {
        var split = new SplitContainer
        {
            SplitterWidth = 6,
            Size = new System.Drawing.Size(leftPanelWidth + rightPanelMinWidth + 64, 720),
            Dock = DockStyle.Fill,
            FixedPanel = FixedPanel.Panel1,
            BackColor = TransRailTheme.SurfaceAlt
        };

        split.Panel1MinSize = leftPanelMinWidth;
        split.Panel2MinSize = rightPanelMinWidth;

        void ApplyPreferredDistance()
        {
            var availableWidth = split.ClientSize.Width;
            if (availableWidth <= 0)
            {
                return;
            }

            var maximumLeftWidth = availableWidth - rightPanelMinWidth - split.SplitterWidth;
            if (maximumLeftWidth < leftPanelMinWidth)
            {
                return;
            }

            var desired = Math.Min(leftPanelWidth, maximumLeftWidth);
            desired = Math.Max(desired, leftPanelMinWidth);

            if (split.SplitterDistance != desired)
            {
                split.SplitterDistance = desired;
            }
        }

        split.HandleCreated += (_, _) => ApplyPreferredDistance();
        split.SizeChanged += (_, _) => ApplyPreferredDistance();
        split.Layout += (_, _) => ApplyPreferredDistance();

        return split;
    }

    public static Panel CreateCenteredScrollHost(Control content, Color backColor, int contentWidth, Padding? padding = null)
    {
        var host = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = backColor,
            AutoScroll = true,
            Padding = padding ?? new Padding(18)
        };

        content.Dock = DockStyle.None;
        content.Anchor = AnchorStyles.Top;
        content.Width = contentWidth;
        content.Margin = Padding.Empty;
        host.Controls.Add(content);

        void Reposition()
        {
            content.Left = Math.Max(host.Padding.Left, (host.ClientSize.Width - content.Width) / 2);
            content.Top = host.Padding.Top;
        }

        host.HandleCreated += (_, _) => Reposition();
        host.SizeChanged += (_, _) => Reposition();
        host.Layout += (_, _) => Reposition();

        return host;
    }

    public static void BindWrapWidth(Label label, Control container, int horizontalPadding = 0)
    {
        void Apply()
        {
            var maxWidth = Math.Max(160, container.ClientSize.Width - horizontalPadding);
            label.MaximumSize = new Size(maxWidth, 0);
        }

        container.HandleCreated += (_, _) => Apply();
        container.SizeChanged += (_, _) => Apply();
        container.Layout += (_, _) => Apply();
    }
}
