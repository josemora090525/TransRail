namespace TransRail.Presentation.Forms;

public sealed class RouteCreateForm : ModuleLauncherForm
{
    public RouteCreateForm()
        : base(
            "TransRail - Crear ruta",
            "Crear ruta",
            "El alta de rutas se centraliza en el módulo de rutas.",
            () => new RouteManagementForm())
    {
    }
}
