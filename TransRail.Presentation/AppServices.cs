using TransRail.Application.Services;
using TransRail.Application.UseCases.Auth;
using TransRail.Application.UseCases.Boarding;
using TransRail.Application.UseCases.Employee;
using TransRail.Application.UseCases.Luggage;
using TransRail.Application.UseCases.Passenger;
using TransRail.Application.UseCases.Payment;
using TransRail.Application.UseCases.Route;
using TransRail.Application.UseCases.Schedule;
using TransRail.Application.UseCases.Station;
using TransRail.Application.UseCases.Ticket;
using TransRail.Application.UseCases.Train;
using TransRail.Application.UseCases.Wagon;

namespace TransRail.Presentation;

public static class AppServices
{
    public static AuthService AuthService { get; set; } = default!;
    public static TrenService TrenService { get; set; } = default!;
    public static RutaService RutaService { get; set; } = default!;
    public static EstacionService EstacionService { get; set; } = default!;
    public static HorarioService HorarioService { get; set; } = default!;
    public static PasajeroService PasajeroService { get; set; } = default!;
    public static EmpleadoService EmpleadoService { get; set; } = default!;
    public static VagonService VagonService { get; set; } = default!;
    public static BoletoService BoletoService { get; set; } = default!;
    public static EquipajeService EquipajeService { get; set; } = default!;
    public static PagoService PagoService { get; set; } = default!;
    public static AbordajeService AbordajeService { get; set; } = default!;

    public static LoginUseCase LoginUseCase { get; set; } = default!;
    public static ManageTrainUseCase ManageTrainUseCase { get; set; } = default!;
    public static ManageRouteUseCase ManageRouteUseCase { get; set; } = default!;
    public static ManageStationUseCase ManageStationUseCase { get; set; } = default!;
    public static ManageScheduleUseCase ManageScheduleUseCase { get; set; } = default!;
    public static ManagePassengerUseCase ManagePassengerUseCase { get; set; } = default!;
    public static PassengerPortalUseCase PassengerPortalUseCase { get; set; } = default!;
    public static ManageEmployeeUseCase ManageEmployeeUseCase { get; set; } = default!;
    public static ManageWagonUseCase ManageWagonUseCase { get; set; } = default!;
    public static TicketPurchaseUseCase TicketPurchaseUseCase { get; set; } = default!;
    public static RegisterPaymentUseCase RegisterPaymentUseCase { get; set; } = default!;
    public static ManageBoardingQueueUseCase ManageBoardingQueueUseCase { get; set; } = default!;
    public static LuggageOperationsUseCase LuggageOperationsUseCase { get; set; } = default!;
    public static UserSession UserSession { get; set; } = default!;
}
