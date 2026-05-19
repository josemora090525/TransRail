using TransRail.Application.DTOs;
using TransRail.Application.Services;
using TransRail.Domain.Entities;
using TransRail.Domain.Enums;

namespace TransRail.Application.UseCases.Passenger;

public sealed class PassengerPortalUseCase
{
    private readonly PasajeroService _pasajeroService;
    private readonly EstacionService _estacionService;
    private readonly RutaService _rutaService;
    private readonly HorarioService _horarioService;
    private readonly VagonService _vagonService;
    private readonly BoletoService _boletoService;
    private readonly EquipajeService _equipajeService;
    private readonly PagoService _pagoService;
    private PassengerPurchaseDraft _draft = new();

    public PassengerPortalUseCase(
        PasajeroService pasajeroService,
        EstacionService estacionService,
        RutaService rutaService,
        HorarioService horarioService,
        VagonService vagonService,
        BoletoService boletoService,
        EquipajeService equipajeService,
        PagoService pagoService)
    {
        _pasajeroService = pasajeroService;
        _estacionService = estacionService;
        _rutaService = rutaService;
        _horarioService = horarioService;
        _vagonService = vagonService;
        _boletoService = boletoService;
        _equipajeService = equipajeService;
        _pagoService = pagoService;
    }

    public async Task<Pasajero> GetOrCreatePassengerAsync(
        string correoSesion,
        string codigoSesion,
        string nombreSesion,
        CancellationToken cancellationToken = default)
    {
        var passenger = await _pasajeroService.GetByCorreoAsync(correoSesion, cancellationToken)
            ?? await _pasajeroService.GetByCodigoAsync(codigoSesion, cancellationToken);

        if (passenger is null)
        {
            passenger = new Pasajero
            {
                CodigoUsuario = codigoSesion,
                Correo = correoSesion,
                NombreCompleto = nombreSesion
            };
        }

        NormalizePassenger(passenger, nombreSesion);
        return passenger;
    }

    public Task SavePassengerAsync(Pasajero pasajero, CancellationToken cancellationToken = default)
    {
        NormalizePassenger(pasajero, pasajero.NombreCompleto);
        _draft.EquipajeDeMano = pasajero.EquipajeDeMano;
        return _pasajeroService.UpsertAsync(pasajero, cancellationToken);
    }

    public async Task<IReadOnlyCollection<PassengerStationOptionDto>> GetStationOptionsAsync(CancellationToken cancellationToken = default)
    {
        var estaciones = await _estacionService.GetAllAsync(cancellationToken);
        return estaciones
            .OrderBy(x => x.Ciudad)
            .ThenBy(x => x.Nombre)
            .Select(x => new PassengerStationOptionDto(x.CodigoEstacion, x.Nombre, x.Ciudad))
            .ToArray();
    }

    public async Task<PassengerRouteSearchResultDto> SearchRoutesAsync(
        string codigoOrigen,
        string codigoDestino,
        CancellationToken cancellationToken = default)
    {
        var estaciones = (await _estacionService.GetAllAsync(cancellationToken))
            .ToDictionary(x => x.CodigoEstacion, StringComparer.OrdinalIgnoreCase);
        var rutas = (await _rutaService.GetAllAsync(cancellationToken)).Where(x => x.Activa).ToArray();
        var horarios = await _horarioService.GetAllOrdenadosAsync(cancellationToken);
        var calculo = await _rutaService.CalcularRutaMasCortaAsync(codigoOrigen, codigoDestino, cancellationToken);

        var codigosRutaDirecta = rutas
            .Where(x => IsSameConnection(x.CodigoEstacionOrigen, x.CodigoEstacionDestino, codigoOrigen, codigoDestino))
            .Select(x => x.CodigoRuta)
            .ToArray();

        var rutaMasCorta = ExtractRouteCodesFromPath(calculo.Ruta, rutas);
        var candidateRouteCodes = codigosRutaDirecta.Length > 0 ? codigosRutaDirecta : rutaMasCorta;

        var scheduleOptions = horarios
            .Where(x => candidateRouteCodes.Contains(x.CodigoRuta, StringComparer.OrdinalIgnoreCase))
            .Select(x =>
            {
                var ruta = rutas.First(r => r.CodigoRuta.Equals(x.CodigoRuta, StringComparison.OrdinalIgnoreCase));
                return new PassengerScheduleOptionDto(
                    x.CodigoHorario,
                    x.CodigoRuta,
                    BuildStationLabel(estaciones, ruta.CodigoEstacionOrigen),
                    BuildStationLabel(estaciones, ruta.CodigoEstacionDestino),
                    x.Fecha,
                    x.HoraSalida,
                    x.HoraLlegada,
                    ruta.DistanciaKm,
                    codigosRutaDirecta.Contains(x.CodigoRuta, StringComparer.OrdinalIgnoreCase));
            })
            .OrderBy(x => x.Fecha)
            .ThenBy(x => x.HoraSalida)
            .ToArray();

        return new PassengerRouteSearchResultDto(
            codigoOrigen,
            codigoDestino,
            BuildStationLabel(estaciones, codigoOrigen),
            BuildStationLabel(estaciones, codigoDestino),
            calculo.Distancia,
            calculo.Ruta,
            scheduleOptions);
    }

    public async Task SelectScheduleAsync(
        string codigoOrigen,
        string codigoDestino,
        string codigoHorario,
        CancellationToken cancellationToken = default)
    {
        var searchResult = await SearchRoutesAsync(codigoOrigen, codigoDestino, cancellationToken);
        var selected = searchResult.HorariosDisponibles
            .FirstOrDefault(x => x.CodigoHorario.Equals(codigoHorario, StringComparison.OrdinalIgnoreCase));

        if (selected is null)
        {
            throw new InvalidOperationException("No se encontr\u00f3 el horario seleccionado.");
        }

        _draft.CodigoOrigen = searchResult.CodigoOrigen;
        _draft.CodigoDestino = searchResult.CodigoDestino;
        _draft.EtiquetaOrigen = searchResult.EtiquetaOrigen;
        _draft.EtiquetaDestino = searchResult.EtiquetaDestino;
        _draft.DistanciaKm = searchResult.DistanciaKm;
        _draft.Recorrido = searchResult.Recorrido;
        _draft.CodigoHorario = selected.CodigoHorario;
        _draft.CodigoRuta = selected.CodigoRuta;
        _draft.FechaViaje = selected.Fecha;
        _draft.HoraSalida = selected.HoraSalida;
        _draft.HoraLlegada = selected.HoraLlegada;
    }

    public PassengerPurchaseDraftDto GetDraft()
    {
        return new PassengerPurchaseDraftDto(
            _draft.CodigoOrigen,
            _draft.CodigoDestino,
            _draft.EtiquetaOrigen,
            _draft.EtiquetaDestino,
            _draft.DistanciaKm,
            string.Join(" -> ", _draft.Recorrido),
            _draft.CodigoHorario,
            _draft.CodigoRuta,
            _draft.FechaViaje,
            _draft.HoraSalida,
            _draft.HoraLlegada,
            _draft.EquipajeDescripcion,
            _draft.EquipajePesoKg,
            _draft.EquipajeDeMano,
            _draft.TipoBoleto,
            _draft.MetodoPago,
            _boletoService.CalcularPrecio(_draft.DistanciaKm, _draft.TipoBoleto));
    }

    public void UpdateLuggage(string equipajeDeMano, string equipajeDescripcion, double equipajePesoKg)
    {
        _draft.EquipajeDeMano = equipajeDeMano.Trim();
        _draft.EquipajeDescripcion = equipajeDescripcion.Trim();
        _draft.EquipajePesoKg = equipajePesoKg;
    }

    public void UpdatePayment(TipoBoleto tipoBoleto, MetodoPago metodoPago)
    {
        _draft.TipoBoleto = tipoBoleto;
        _draft.MetodoPago = metodoPago;
    }

    public async Task<PassengerPurchaseSummaryDto> BuildCheckoutSummaryAsync(
        string correoSesion,
        string codigoSesion,
        string nombreSesion,
        CancellationToken cancellationToken = default)
    {
        var passenger = await GetOrCreatePassengerAsync(correoSesion, codigoSesion, nombreSesion, cancellationToken);
        var draft = GetDraft();
        ValidateDraft(draft);

        return new PassengerPurchaseSummaryDto(
            passenger,
            BuildNextCode("BOL", await _boletoService.GetAllAsync(cancellationToken)),
            BuildNextCode("PAG", await _pagoService.GetAllAsync(cancellationToken)),
            string.IsNullOrWhiteSpace(draft.EquipajeDescripcion) ? null : BuildNextCode("EQ", await _equipajeService.GetAllAsync(cancellationToken)),
            draft.EtiquetaOrigen,
            draft.EtiquetaDestino,
            draft.DistanciaKm,
            draft.RecorridoTexto,
            draft.CodigoRuta,
            draft.CodigoHorario,
            draft.FechaViaje!.Value,
            draft.HoraSalida!.Value,
            draft.HoraLlegada!.Value,
            draft.EquipajeDescripcion,
            draft.EquipajePesoKg,
            draft.EquipajeDeMano,
            draft.TipoBoleto,
            draft.MetodoPago,
            draft.PrecioCalculado);
    }

    public async Task<PassengerPurchaseSummaryDto> ConfirmPurchaseAsync(
        string correoSesion,
        string codigoSesion,
        string nombreSesion,
        CancellationToken cancellationToken = default)
    {
        var passenger = await GetOrCreatePassengerAsync(correoSesion, codigoSesion, nombreSesion, cancellationToken);
        NormalizePassenger(passenger, nombreSesion);
        await SavePassengerAsync(passenger, cancellationToken);

        var summary = await BuildCheckoutSummaryAsync(correoSesion, codigoSesion, nombreSesion, cancellationToken);

        var boleto = new Boleto
        {
            CodigoBoleto = summary.CodigoBoleto,
            CodigoPasajero = passenger.CodigoUsuario,
            CodigoHorario = summary.CodigoHorario,
            TipoBoleto = summary.TipoBoleto,
            Precio = summary.PrecioTotal
        };

        var purchaseResult = await _boletoService.ComprarAsync(boleto, cancellationToken);
        if (!purchaseResult.Ok)
        {
            throw new InvalidOperationException(purchaseResult.Error);
        }

        var pago = new Pago
        {
            CodigoPago = summary.CodigoPago,
            CodigoBoleto = boleto.CodigoBoleto,
            Metodo = summary.MetodoPago,
            Valor = summary.PrecioTotal,
            Confirmado = true
        };
        await _pagoService.RegistrarPagoAsync(pago, cancellationToken);

        if (!string.IsNullOrWhiteSpace(summary.CodigoEquipaje))
        {
            var cargoWagon = await ResolveCargoWagonCodeAsync(cancellationToken);
            await _equipajeService.UpsertAsync(new Equipaje
            {
                CodigoEquipaje = summary.CodigoEquipaje,
                CodigoBoleto = boleto.CodigoBoleto,
                CodigoVagonCarga = cargoWagon,
                PesoKg = summary.EquipajePesoKg,
                Descripcion = summary.EquipajeDescripcion
            }, cancellationToken);
        }

        _draft = new PassengerPurchaseDraft { EquipajeDeMano = passenger.EquipajeDeMano };
        return summary;
    }

    public async Task<IReadOnlyCollection<Boleto>> GetTicketsByPassengerAsync(string codigoPasajero, CancellationToken cancellationToken = default)
    {
        var boletos = await _boletoService.GetAllAsync(cancellationToken);
        return boletos
            .Where(x => x.CodigoPasajero.Equals(codigoPasajero, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(x => x.FechaCompraUtc)
            .ToArray();
    }

    private static void NormalizePassenger(Pasajero passenger, string fallbackName)
    {
        if (string.IsNullOrWhiteSpace(passenger.NombreCompleto))
        {
            passenger.NombreCompleto = fallbackName;
        }

        if (string.IsNullOrWhiteSpace(passenger.Nombres) && !string.IsNullOrWhiteSpace(passenger.NombreCompleto))
        {
            var parts = passenger.NombreCompleto.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            passenger.Nombres = parts.ElementAtOrDefault(0) ?? string.Empty;
            passenger.Apellidos = parts.ElementAtOrDefault(1) ?? string.Empty;
        }

        if (string.IsNullOrWhiteSpace(passenger.TipoIdentificacion))
        {
            passenger.TipoIdentificacion = "CC";
        }
    }

    private static bool IsSameConnection(string originA, string destinationA, string originB, string destinationB)
    {
        return (originA.Equals(originB, StringComparison.OrdinalIgnoreCase) &&
                destinationA.Equals(destinationB, StringComparison.OrdinalIgnoreCase)) ||
               (originA.Equals(destinationB, StringComparison.OrdinalIgnoreCase) &&
                destinationA.Equals(originB, StringComparison.OrdinalIgnoreCase));
    }

    private static string[] ExtractRouteCodesFromPath(IReadOnlyList<string> path, IReadOnlyCollection<Ruta> rutas)
    {
        if (path.Count < 2)
        {
            return Array.Empty<string>();
        }

        var routeCodes = new List<string>();
        for (var index = 0; index < path.Count - 1; index++)
        {
            var origin = path[index];
            var destination = path[index + 1];
            var route = rutas.FirstOrDefault(x => IsSameConnection(x.CodigoEstacionOrigen, x.CodigoEstacionDestino, origin, destination));
            if (route is not null)
            {
                routeCodes.Add(route.CodigoRuta);
            }
        }

        return routeCodes.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
    }

    private static string BuildStationLabel(IReadOnlyDictionary<string, Estacion> estaciones, string codigoEstacion)
    {
        if (!estaciones.TryGetValue(codigoEstacion, out var estacion))
        {
            return codigoEstacion;
        }

        return $"{estacion.Ciudad} ({estacion.CodigoEstacion})";
    }

    private static string BuildNextCode<T>(string prefix, IReadOnlyCollection<T> entities)
        where T : IConCodigoOperativo
    {
        var next = entities
            .Select(x =>
            {
                var parts = x.Codigo.Split('-', StringSplitOptions.RemoveEmptyEntries);
                return parts.Length > 1 && int.TryParse(parts[^1], out var numeric) ? numeric : 0;
            })
            .DefaultIfEmpty(0)
            .Max() + 1;
        return $"{prefix}-{next:000}";
    }

    private async Task<string> ResolveCargoWagonCodeAsync(CancellationToken cancellationToken)
    {
        var vagones = await _vagonService.GetAllAsync(cancellationToken);
        return vagones.FirstOrDefault(x => x.Tipo == TipoVagon.Carga)?.CodigoVagon
            ?? vagones.FirstOrDefault()?.CodigoVagon
            ?? "VG-001";
    }

    private static void ValidateDraft(PassengerPurchaseDraftDto draft)
    {
        if (string.IsNullOrWhiteSpace(draft.CodigoHorario) ||
            string.IsNullOrWhiteSpace(draft.CodigoRuta) ||
            draft.FechaViaje is null ||
            draft.HoraSalida is null ||
            draft.HoraLlegada is null ||
            draft.DistanciaKm <= 0)
        {
            throw new InvalidOperationException("Debes seleccionar una ruta y un horario antes de confirmar la compra.");
        }
    }

    private sealed class PassengerPurchaseDraft
    {
        public string CodigoOrigen { get; set; } = string.Empty;
        public string CodigoDestino { get; set; } = string.Empty;
        public string EtiquetaOrigen { get; set; } = string.Empty;
        public string EtiquetaDestino { get; set; } = string.Empty;
        public int DistanciaKm { get; set; }
        public IReadOnlyList<string> Recorrido { get; set; } = Array.Empty<string>();
        public string CodigoHorario { get; set; } = string.Empty;
        public string CodigoRuta { get; set; } = string.Empty;
        public DateOnly? FechaViaje { get; set; }
        public TimeOnly? HoraSalida { get; set; }
        public TimeOnly? HoraLlegada { get; set; }
        public string EquipajeDescripcion { get; set; } = string.Empty;
        public double EquipajePesoKg { get; set; }
        public string EquipajeDeMano { get; set; } = string.Empty;
        public TipoBoleto TipoBoleto { get; set; } = TipoBoleto.Estandar;
        public MetodoPago MetodoPago { get; set; } = MetodoPago.TarjetaDebito;
    }
}
