using System.Windows.Forms;

namespace TransRail.Presentation.Forms;

public abstract class TransRailFormBase : Form
{
    protected TransRailFormBase()
    {
        AutoScaleMode = AutoScaleMode.Dpi;
        DoubleBuffered = true;
        StartPosition = FormStartPosition.CenterScreen;
        TransRailWindowStateManager.Attach(this);
    }

    protected DialogResult OpenManagedDialog(Form dialog)
    {
        TransRailWindowStateManager.ApplyPreferredState(dialog);
        return dialog.ShowDialog(this);
    }

    protected DialogResult OpenManagedDialog(Func<Form> dialogFactory)
    {
        using var dialog = dialogFactory();
        return OpenManagedDialog(dialog);
    }

    protected void OpenManagedScreen(Form nextScreen)
    {
        Hide();
        nextScreen.FormClosed += (_, _) =>
        {
            if (!IsDisposed)
            {
                Show();
                TransRailWindowStateManager.ApplyPreferredState(this);
            }
        };
        TransRailWindowStateManager.ApplyPreferredState(nextScreen);
        nextScreen.Show(this);
    }

    protected void OpenManagedScreen(Func<Form> screenFactory)
    {
        OpenManagedScreen(screenFactory());
    }
}
