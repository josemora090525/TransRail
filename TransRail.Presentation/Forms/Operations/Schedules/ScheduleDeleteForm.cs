namespace TransRail.Presentation.Forms;

public sealed class ScheduleDeleteForm : ModuleLauncherForm
{
    public ScheduleDeleteForm()
        : base(
            "TransRail - Eliminar horario",
            "Eliminar horario",
            "La eliminación de horarios está centralizada en el módulo de horarios.",
            () => new ScheduleManagementForm())
    {
    }
}
