namespace TransRail.Presentation.Forms;

public sealed class TrainListForm : ModuleLauncherForm
{
    public TrainListForm()
        : base(
            "TransRail - Listar trenes",
            "Listado de trenes",
            "El listado de trenes está disponible en el módulo principal de trenes.",
            () => new TrainManagementForm())
    {
    }
}
