namespace TransRail.Presentation.Forms;

public sealed class ScheduleSearchForm : ModuleLauncherForm
{
    public ScheduleSearchForm()
        : base(
            "TransRail - Buscar horario",
            "Buscar horario",
            "La búsqueda de horarios por tren o fecha se realiza desde el módulo de horarios.",
            () => new ScheduleManagementForm())
    {
    }
}
