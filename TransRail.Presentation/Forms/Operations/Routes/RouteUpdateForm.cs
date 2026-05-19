namespace TransRail.Presentation.Forms;

public sealed class RouteUpdateForm : ModuleLauncherForm
{
    public RouteUpdateForm()
        : base(
            "TransRail - Modificar ruta",
            "Modificar ruta",
            "La actualización de rutas se gestiona en el módulo principal de rutas.",
            () => new RouteManagementForm())
    {
    }
}
