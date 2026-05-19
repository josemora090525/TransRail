namespace TransRail.Presentation.Forms;

public sealed class AdminMenuForm : WorkspaceMenuFormBase
{
    public AdminMenuForm()
        : base("TransRail - Men\u00fa de administrador")
    {
        ConfigureHeader(
            "Administrador",
            "Gestiona trenes, vagones, estaciones, rutas, horarios, pasajeros, empleados, boletos, abordaje y equipaje.",
            "wpf_administrator.png");

        SetWelcomeMessage(
            "Centro de operaciones",
            "Selecciona un m\u00f3dulo en la barra lateral para trabajar sin salir de esta misma ventana.");

        AddModule("Trenes", "Group 2.png", () => new TrainManagementForm());
        AddModule("Vagones", "Group 2.png", () => new WagonManagementForm());
        AddModule("Estaciones", "arcticons_oeffistations.png", () => new StationManagementForm());
        AddModule("Rutas", "gis_route-start.png", () => new RouteManagementForm());
        AddModule("Horarios", "Frame.png", () => new ScheduleManagementForm());
        AddModule("Pasajeros", "qlementine-icons_user-16.png", () => new PassengerManagementForm());
        AddModule("Empleados", "Vector.png", () => new EmployeeManagementForm());
        AddModule("Boletos", "ticket.generated", () => new TicketForm());
        AddModule("Abordaje", "mdi_user.png", () => new BoardingManagementForm());
        AddModule("Equipaje", "Group 2.png", () => new LuggageManagementForm());
    }
}
