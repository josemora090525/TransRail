namespace TransRail.Presentation.Forms;

public sealed class EmployeeMenuForm : WorkspaceMenuFormBase
{
    public EmployeeMenuForm()
        : base("TransRail - Men\u00fa de empleado")
    {
        ConfigureHeader(
            "Empleado",
            "Acceso operativo a horarios, rutas, pasajeros, boletos, abordaje y equipaje.",
            "Vector.png");

        SetWelcomeMessage(
            "\u00c1rea de trabajo",
            "Selecciona un m\u00f3dulo y el contenido se cargar\u00e1 aqu\u00ed mismo, sin abrir otra ventana.");

        AddModule("Horarios", "Frame.png", () => new ScheduleManagementForm());
        AddModule("Rutas", "gis_route-start.png", () => new RouteManagementForm());
        AddModule("Pasajeros", "qlementine-icons_user-16.png", () => new PassengerManagementForm());
        AddModule("Boletos", "ticket.generated", () => new TicketForm());
        AddModule("Abordaje", "mdi_user.png", () => new BoardingManagementForm());
        AddModule("Equipaje", "Group 2.png", () => new LuggageManagementForm());
    }
}
