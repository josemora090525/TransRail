namespace TransRail.Presentation.Forms;

public sealed class TrainSearchForm : ModuleLauncherForm
{
    public TrainSearchForm()
        : base(
            "TransRail - Buscar tren",
            "Buscar tren",
            "La búsqueda de trenes se realiza desde el módulo de trenes.",
            () => new TrainManagementForm())
    {
    }
}
