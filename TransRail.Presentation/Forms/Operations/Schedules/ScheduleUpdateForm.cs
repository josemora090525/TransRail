namespace TransRail.Presentation.Forms;

public sealed class ScheduleUpdateForm : ModuleLauncherForm
{
    public ScheduleUpdateForm()
        : base(
            "TransRail - Modificar horario",
            "Modificar horario",
            "La actualización de horarios se realiza desde el módulo de horarios.",
            () => new ScheduleManagementForm())
    {
    }
}
