namespace TransRail.Presentation.Forms;

public sealed class PassengerDataForm : ModuleLauncherForm
{
    public PassengerDataForm()
        : base(
            "TransRail - Datos del pasajero",
            "Datos del pasajero",
            "La captura y edición de los datos del pasajero se centraliza en el módulo de pasajeros.",
            () => new PassengerManagementForm())
    {
    }
}
