namespace TransRail.Presentation.Forms;

public sealed class RouteDeleteForm : ModuleLauncherForm
{
    public RouteDeleteForm()
        : base(
            "TransRail - Eliminar ruta",
            "Eliminar ruta",
            "La eliminación de rutas se realiza en el módulo de rutas.",
            () => new RouteManagementForm())
    {
    }
}
