namespace TransRail.Presentation.Forms;

public sealed class RouteListForm : ModuleLauncherForm
{
    public RouteListForm()
        : base(
            "TransRail - Listar rutas",
            "Listado de rutas",
            "El listado de rutas está disponible en el módulo principal de rutas.",
            () => new RouteManagementForm())
    {
    }
}
