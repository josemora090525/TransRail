namespace TransRail.Presentation.Forms;

public sealed class WagonDeleteForm : ModuleLauncherForm
{
    public WagonDeleteForm()
        : base(
            "TransRail - Eliminar vagón",
            "Eliminar vagón",
            "La eliminación de vagones se realiza desde el módulo de vagones.",
            () => new WagonManagementForm())
    {
    }
}
