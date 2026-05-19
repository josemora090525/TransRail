namespace TransRail.Presentation.Forms;

public sealed class RouteSearchForm : ModuleLauncherForm
{
    public RouteSearchForm()
        : base(
            "TransRail - Buscar ruta",
            "Buscar ruta",
            "La búsqueda de rutas y el cálculo de ruta mínima se encuentran en el módulo de rutas.",
            () => new RouteManagementForm())
    {
    }
}
