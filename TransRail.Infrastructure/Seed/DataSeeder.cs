using TransRail.Application.Interfaces;
using TransRail.Domain.Entities;
using TransRail.Domain.Enums;

namespace TransRail.Infrastructure.Seed;

public sealed class DataSeeder
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasajeroRepository _pasajeroRepository;
    private readonly IEmpleadoRepository _empleadoRepository;
    private readonly IEstacionRepository _estacionRepository;
    private readonly IRutaRepository _rutaRepository;
    private readonly ITrenRepository _trenRepository;
    private readonly IVagonRepository _vagonRepository;
    private readonly IHorarioRepository _horarioRepository;
    private readonly IBoletoRepository _boletoRepository;
    private readonly IEquipajeRepository _equipajeRepository;
    private readonly IPagoRepository _pagoRepository;

    public DataSeeder(
        IUsuarioRepository usuarioRepository,
        IPasajeroRepository pasajeroRepository,
        IEmpleadoRepository empleadoRepository,
        IEstacionRepository estacionRepository,
        IRutaRepository rutaRepository,
        ITrenRepository trenRepository,
        IVagonRepository vagonRepository,
        IHorarioRepository horarioRepository,
        IBoletoRepository boletoRepository,
        IEquipajeRepository equipajeRepository,
        IPagoRepository pagoRepository)
    {
        _usuarioRepository = usuarioRepository;
        _pasajeroRepository = pasajeroRepository;
        _empleadoRepository = empleadoRepository;
        _estacionRepository = estacionRepository;
        _rutaRepository = rutaRepository;
        _trenRepository = trenRepository;
        _vagonRepository = vagonRepository;
        _horarioRepository = horarioRepository;
        _boletoRepository = boletoRepository;
        _equipajeRepository = equipajeRepository;
        _pagoRepository = pagoRepository;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await SeedUsuariosAsync(cancellationToken);
        await SeedPasajerosYEmpleadosAsync(cancellationToken);
        await SeedEstacionesYRutasAsync(cancellationToken);
        await SeedTrenesYHorariosAsync(cancellationToken);
        await SeedVagonesYBoletosAsync(cancellationToken);
        await SeedEquipajesYPagosAsync(cancellationToken);
    }

    private async Task SeedUsuariosAsync(CancellationToken cancellationToken)
    {
        var usuarios = await _usuarioRepository.GetAllAsync(cancellationToken);
        if (usuarios.Count > 0)
        {
            return;
        }

        await _usuarioRepository.UpsertAsync(new Administrador
        {
            CodigoUsuario = "USR-ADM-001",
            NombreCompleto = "Admin TransRail",
            NumeroDocumento = "1000001",
            Correo = "admin@transrail.local",
            Contrasena = "admin123"
        }, cancellationToken);

        await _usuarioRepository.UpsertAsync(new Empleado
        {
            CodigoUsuario = "USR-EMP-001",
            NombreCompleto = "Empleado TransRail",
            NumeroDocumento = "2000001",
            Correo = "empleado@transrail.local",
            Contrasena = "empleado123"
        }, cancellationToken);

        await _usuarioRepository.UpsertAsync(new Pasajero
        {
            CodigoUsuario = "PAS-001",
            NombreCompleto = "María Pérez",
            NumeroDocumento = "3000001",
            Correo = "pasajero@transrail.local",
            Contrasena = "pasajero123",
            Nombres = "María",
            Apellidos = "Pérez",
            TipoIdentificacion = "CC",
            Direccion = "Calle 45 # 12 - 18, Medellín",
            Telefono = "3001234567",
            NombreContacto = "Ana",
            ApellidoContacto = "Pérez",
            TelefonoContacto = "3015557788",
            EquipajeDeMano = "Morral pequeño con documentos",
            Categoria = CategoriaPasajero.Ejecutivo
        }, cancellationToken);
    }

    private async Task SeedEstacionesYRutasAsync(CancellationToken cancellationToken)
    {
        var estacionesBase = new Dictionary<string, (string Nombre, string Ciudad)>
        {
            ["A"] = ("Estaci\u00f3n Medell\u00edn Central", "Medell\u00edn"),
            ["B"] = ("Estaci\u00f3n Bogot\u00e1 Terminal", "Bogot\u00e1"),
            ["C"] = ("Estaci\u00f3n Cali Valle", "Cali"),
            ["D"] = ("Estaci\u00f3n Barranquilla Norte", "Barranquilla"),
            ["E"] = ("Estaci\u00f3n Cartagena Bah\u00eda", "Cartagena"),
            ["F"] = ("Estaci\u00f3n Bucaramanga Andina", "Bucaramanga"),
            ["G"] = ("Estaci\u00f3n Pereira Cafetera", "Pereira"),
            ["H"] = ("Estaci\u00f3n Manizales Cable", "Manizales"),
            ["I"] = ("Estaci\u00f3n C\u00facuta Frontera", "C\u00facuta"),
            ["J"] = ("Estaci\u00f3n Santa Marta Mar", "Santa Marta"),
            ["K"] = ("Estaci\u00f3n Villavicencio Llanos", "Villavicencio")
        };

        var estacionesActuales = await _estacionRepository.GetAllAsync(cancellationToken);
        foreach (var (codigo, estacionBase) in estacionesBase)
        {
            var actual = estacionesActuales.FirstOrDefault(x => x.CodigoEstacion.Equals(codigo, StringComparison.OrdinalIgnoreCase));
            if (actual is null)
            {
                await _estacionRepository.UpsertAsync(new Estacion
                {
                    CodigoEstacion = codigo,
                    Nombre = estacionBase.Nombre,
                    Ciudad = estacionBase.Ciudad
                }, cancellationToken);

                continue;
            }

            var usaPlaceholder =
                string.Equals(actual.Ciudad, "TransRail", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(actual.Nombre, $"Estacion {codigo}", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(actual.Nombre, $"Estaci\u00f3n {codigo}", StringComparison.OrdinalIgnoreCase);

            if (!usaPlaceholder)
            {
                continue;
            }

            actual.Nombre = estacionBase.Nombre;
            actual.Ciudad = estacionBase.Ciudad;
            await _estacionRepository.UpsertAsync(actual, cancellationToken);
        }

        // Matriz base A-K para el c\u00e1lculo de distancias (aristas no dirigidas).
        var rutasMatriz = new (string Origen, string Destino, int Distancia)[]
        {
            ("A", "B", 30),
            ("A", "C", 40),
            ("A", "D", 50),
            ("A", "F", 50),
            ("D", "E", 20),
            ("E", "F", 65),
            ("F", "G", 80),
            ("G", "H", 30),
            ("G", "I", 145),
            ("C", "I", 80),
            ("C", "J", 120),
            ("C", "K", 110)
        };

        var rutasActuales = await _rutaRepository.GetAllAsync(cancellationToken);
        foreach (var (origen, destino, distancia) in rutasMatriz)
        {
            var codigoRuta = $"RUT-{origen}-{destino}";
            if (rutasActuales.Any(x => x.CodigoRuta.Equals(codigoRuta, StringComparison.OrdinalIgnoreCase)))
            {
                continue;
            }

            await _rutaRepository.UpsertAsync(new Ruta
            {
                CodigoRuta = codigoRuta,
                CodigoEstacionOrigen = origen,
                CodigoEstacionDestino = destino,
                DistanciaKm = distancia,
                Activa = true
            }, cancellationToken);
        }
    }

    private async Task SeedTrenesYHorariosAsync(CancellationToken cancellationToken)
    {
        var trenes = await _trenRepository.GetAllAsync(cancellationToken);
        if (trenes.Count == 0)
        {
            await _trenRepository.UpsertAsync(new Tren
            {
                CodigoTren = "TR-001",
                NumeroOperativo = "001",
                Nombre = "Tren Andino",
                CapacidadVagones = 10,
                Kilometraje = 12000,
                EnCirculacion = true
            }, cancellationToken);
        }

        var horarios = await _horarioRepository.GetAllAsync(cancellationToken);
        var horariosBase = new[]
        {
            new Horario
            {
                CodigoHorario = "HOR-2026-001",
                CodigoTren = "TR-001",
                CodigoRuta = "RUT-A-B",
                Fecha = DateOnly.FromDateTime(DateTime.Today),
                HoraSalida = new TimeOnly(8, 30),
                HoraLlegada = new TimeOnly(9, 15)
            },
            new Horario
            {
                CodigoHorario = "HOR-2026-002",
                CodigoTren = "TR-001",
                CodigoRuta = "RUT-A-C",
                Fecha = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                HoraSalida = new TimeOnly(7, 10),
                HoraLlegada = new TimeOnly(8, 5)
            },
            new Horario
            {
                CodigoHorario = "HOR-2026-003",
                CodigoTren = "TR-001",
                CodigoRuta = "RUT-A-D",
                Fecha = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                HoraSalida = new TimeOnly(10, 20),
                HoraLlegada = new TimeOnly(11, 20)
            },
            new Horario
            {
                CodigoHorario = "HOR-2026-004",
                CodigoTren = "TR-001",
                CodigoRuta = "RUT-D-E",
                Fecha = DateOnly.FromDateTime(DateTime.Today.AddDays(2)),
                HoraSalida = new TimeOnly(9, 15),
                HoraLlegada = new TimeOnly(9, 45)
            },
            new Horario
            {
                CodigoHorario = "HOR-2026-005",
                CodigoTren = "TR-001",
                CodigoRuta = "RUT-C-I",
                Fecha = DateOnly.FromDateTime(DateTime.Today.AddDays(2)),
                HoraSalida = new TimeOnly(14, 0),
                HoraLlegada = new TimeOnly(15, 35)
            }
        };

        foreach (var horarioBase in horariosBase)
        {
            if (horarios.Any(x => x.CodigoHorario.Equals(horarioBase.CodigoHorario, StringComparison.OrdinalIgnoreCase)))
            {
                continue;
            }

            await _horarioRepository.UpsertAsync(horarioBase, cancellationToken);
        }
    }

    private async Task SeedPasajerosYEmpleadosAsync(CancellationToken cancellationToken)
    {
        var pasajeros = await _pasajeroRepository.GetAllAsync(cancellationToken);
        if (pasajeros.Count == 0)
        {
            await _pasajeroRepository.UpsertAsync(new Pasajero
            {
                CodigoUsuario = "PAS-001",
                NombreCompleto = "María Pérez",
                NumeroDocumento = "3000001",
                Correo = "pasajero@transrail.local",
                Contrasena = "pasajero123",
                Nombres = "María",
                Apellidos = "Pérez",
                TipoIdentificacion = "CC",
                Direccion = "Calle 45 # 12 - 18, Medellín",
                Telefono = "3001234567",
                NombreContacto = "Ana",
                ApellidoContacto = "Pérez",
                TelefonoContacto = "3015557788",
                EquipajeDeMano = "Morral pequeño con documentos",
                Categoria = CategoriaPasajero.Ejecutivo
            }, cancellationToken);
        }

        var empleados = await _empleadoRepository.GetAllAsync(cancellationToken);
        if (empleados.Count == 0)
        {
            await _empleadoRepository.UpsertAsync(new Empleado
            {
                CodigoUsuario = "EMP-001",
                NombreCompleto = "Carlos Ruiz",
                NumeroDocumento = "2000001",
                Correo = "empleado@transrail.local",
                Contrasena = "empleado123"
            }, cancellationToken);
        }
    }

    private async Task SeedVagonesYBoletosAsync(CancellationToken cancellationToken)
    {
        var vagones = await _vagonRepository.GetAllAsync(cancellationToken);
        if (vagones.Count == 0)
        {
            await _vagonRepository.UpsertAsync(new Vagon
            {
                CodigoVagon = "VG-001",
                CodigoTren = "TR-001",
                Tipo = TipoVagon.Pasajeros,
                Capacidad = 80,
                PesoMaximoKg = 0
            }, cancellationToken);

            await _vagonRepository.UpsertAsync(new Vagon
            {
                CodigoVagon = "VG-002",
                CodigoTren = "TR-001",
                Tipo = TipoVagon.Carga,
                Capacidad = 40,
                PesoMaximoKg = 2500
            }, cancellationToken);
        }
        else if (!vagones.Any(x => x.Tipo == TipoVagon.Carga))
        {
            await _vagonRepository.UpsertAsync(new Vagon
            {
                CodigoVagon = "VG-002",
                CodigoTren = "TR-001",
                Tipo = TipoVagon.Carga,
                Capacidad = 40,
                PesoMaximoKg = 2500
            }, cancellationToken);
        }

        var boletos = await _boletoRepository.GetAllAsync(cancellationToken);
        if (boletos.Count == 0)
        {
            await _boletoRepository.UpsertAsync(new Boleto
            {
                CodigoBoleto = "BOL-001",
                CodigoPasajero = "PAS-001",
                CodigoHorario = "HOR-2026-001",
                TipoBoleto = TipoBoleto.Estandar,
                Precio = 15.00m
            }, cancellationToken);
        }
    }

    private async Task SeedEquipajesYPagosAsync(CancellationToken cancellationToken)
    {
        var equipajes = await _equipajeRepository.GetAllAsync(cancellationToken);
        if (equipajes.Count == 0)
        {
            await _equipajeRepository.UpsertAsync(new Equipaje
            {
                CodigoEquipaje = "EQ-001",
                CodigoBoleto = "BOL-001",
                CodigoVagonCarga = "VG-001",
                PesoKg = 18.5,
                Descripcion = "Maleta de cabina"
            }, cancellationToken);
        }

        var pagos = await _pagoRepository.GetAllAsync(cancellationToken);
        if (pagos.Count == 0)
        {
            await _pagoRepository.UpsertAsync(new Pago
            {
                CodigoPago = "PAG-001",
                CodigoBoleto = "BOL-001",
                Metodo = MetodoPago.TarjetaDebito,
                Valor = 15.00m,
                Confirmado = true
            }, cancellationToken);
        }
    }
}
