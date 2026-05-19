namespace TransRail.Presentation.Forms;

public sealed class WagonCreateForm : ModuleLauncherForm
{
    public WagonCreateForm()
        : base(
            "TransRail - Crear vagón",
            "Crear vagón",
            "La creación de vagones se gestiona desde el módulo principal de vagones.",
            () => new WagonManagementForm())
    {
    }
}
