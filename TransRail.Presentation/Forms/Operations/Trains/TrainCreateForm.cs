namespace TransRail.Presentation.Forms;

public sealed class TrainCreateForm : ModuleLauncherForm
{
    public TrainCreateForm()
        : base(
            "TransRail - Crear tren",
            "Crear tren",
            "La creación de trenes se gestiona desde el módulo principal de trenes.",
            () => new TrainManagementForm())
    {
    }
}
