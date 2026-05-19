using System.Windows.Forms;
using TransRail.Application.Interfaces;
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
using TransRail.Infrastructure.Persistence.Json;
using TransRail.Infrastructure.Repositories;
using TransRail.Infrastructure.Seed;
using TransRail.Presentation.Forms;

namespace TransRail.Presentation;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        ConfigureServices();
        SeedData();
        System.Windows.Forms.Application.Run(new LoginForm());
    }

    private static void ConfigureServices()
    {
        IJsonStorage storage = new JsonStorage();

        var usuarioRepository = new JsonUsuarioRepository(storage);
        var pasajeroRepository = new JsonPasajeroRepository(storage);
        var empleadoRepository = new JsonEmpleadoRepository(storage);
        var estacionRepository = new JsonEstacionRepository(storage);
        var rutaRepository = new JsonRutaRepository(storage);
        var trenRepository = new JsonTrenRepository(storage);
        var vagonRepository = new JsonVagonRepository(storage);
        var horarioRepository = new JsonHorarioRepository(storage);
        var boletoRepository = new JsonBoletoRepository(storage);
        var equipajeRepository = new JsonEquipajeRepository(storage);
        var pagoRepository = new JsonPagoRepository(storage);

        AppServices.AuthService = new AuthService(usuarioRepository);
        AppServices.EstacionService = new EstacionService(estacionRepository);
        AppServices.RutaService = new RutaService(rutaRepository, estacionRepository);
        AppServices.TrenService = new TrenService(trenRepository);
        AppServices.HorarioService = new HorarioService(horarioRepository);
        AppServices.PasajeroService = new PasajeroService(pasajeroRepository, usuarioRepository);
        AppServices.EmpleadoService = new EmpleadoService(empleadoRepository, usuarioRepository);
        AppServices.VagonService = new VagonService(vagonRepository);
        AppServices.BoletoService = new BoletoService(boletoRepository);
        AppServices.EquipajeService = new EquipajeService(equipajeRepository);
        AppServices.PagoService = new PagoService(pagoRepository);
        AppServices.AbordajeService = new AbordajeService();
        AppServices.UserSession = new UserSession();

        AppServices.LoginUseCase = new LoginUseCase(AppServices.AuthService);
        AppServices.ManageTrainUseCase = new ManageTrainUseCase(AppServices.TrenService);
        AppServices.ManageRouteUseCase = new ManageRouteUseCase(AppServices.RutaService);
        AppServices.ManageStationUseCase = new ManageStationUseCase(AppServices.EstacionService);
        AppServices.ManageScheduleUseCase = new ManageScheduleUseCase(AppServices.HorarioService);
        AppServices.ManagePassengerUseCase = new ManagePassengerUseCase(AppServices.PasajeroService);
        AppServices.PassengerPortalUseCase = new PassengerPortalUseCase(
            AppServices.PasajeroService,
            AppServices.EstacionService,
            AppServices.RutaService,
            AppServices.HorarioService,
            AppServices.VagonService,
            AppServices.BoletoService,
            AppServices.EquipajeService,
            AppServices.PagoService);
        AppServices.ManageEmployeeUseCase = new ManageEmployeeUseCase(AppServices.EmpleadoService);
        AppServices.ManageWagonUseCase = new ManageWagonUseCase(AppServices.VagonService);
        AppServices.TicketPurchaseUseCase = new TicketPurchaseUseCase(AppServices.BoletoService);
        AppServices.RegisterPaymentUseCase = new RegisterPaymentUseCase(AppServices.PagoService);
        AppServices.ManageBoardingQueueUseCase = new ManageBoardingQueueUseCase(AppServices.AbordajeService);
        AppServices.LuggageOperationsUseCase = new LuggageOperationsUseCase(AppServices.EquipajeService);
    }

    private static void SeedData()
    {
        IJsonStorage storage = new JsonStorage();
        var seeder = new DataSeeder(
            new JsonUsuarioRepository(storage),
            new JsonPasajeroRepository(storage),
            new JsonEmpleadoRepository(storage),
            new JsonEstacionRepository(storage),
            new JsonRutaRepository(storage),
            new JsonTrenRepository(storage),
            new JsonVagonRepository(storage),
            new JsonHorarioRepository(storage),
            new JsonBoletoRepository(storage),
            new JsonEquipajeRepository(storage),
            new JsonPagoRepository(storage));

        seeder.SeedAsync().GetAwaiter().GetResult();
    }
}
