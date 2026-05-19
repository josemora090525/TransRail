namespace TransRail.Presentation.Forms;

public sealed class WagonUpdateForm : ModuleLauncherForm
{
    public WagonUpdateForm()
        : base(
            "TransRail - Modificar vagón",
            "Modificar vagón",
            "La actualización de vagones se realiza desde el módulo de vagones.",
            () => new WagonManagementForm())
    {
    }
}
