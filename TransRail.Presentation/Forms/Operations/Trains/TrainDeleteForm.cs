namespace TransRail.Presentation.Forms;

public sealed class TrainDeleteForm : ModuleLauncherForm
{
    public TrainDeleteForm()
        : base(
            "TransRail - Eliminar tren",
            "Eliminar tren",
            "La eliminación de trenes se realiza desde el módulo de trenes.",
            () => new TrainManagementForm())
    {
    }
}
