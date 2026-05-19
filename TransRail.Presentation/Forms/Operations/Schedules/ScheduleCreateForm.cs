namespace TransRail.Presentation.Forms;

public sealed class ScheduleCreateForm : ModuleLauncherForm
{
    public ScheduleCreateForm()
        : base(
            "TransRail - Crear horario",
            "Crear horario",
            "La creación de horarios se realiza desde el módulo de horarios.",
            () => new ScheduleManagementForm())
    {
    }
}
