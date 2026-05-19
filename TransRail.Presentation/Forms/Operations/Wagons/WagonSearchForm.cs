namespace TransRail.Presentation.Forms;

public sealed class WagonSearchForm : ModuleLauncherForm
{
    public WagonSearchForm()
        : base(
            "TransRail - Buscar vagón",
            "Buscar vagón",
            "La búsqueda de vagones se realiza desde el módulo principal de vagones.",
            () => new WagonManagementForm())
    {
    }
}
