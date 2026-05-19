using TransRail.Application.Interfaces;
using TransRail.Domain.Entities;
using TransRail.Domain.Rules;
using TransRail.Domain.Structures;

namespace TransRail.Application.Services;

public sealed class RutaService
{
    private readonly IRutaRepository _rutaRepository;
    private readonly IEstacionRepository _estacionRepository;

    public RutaService(IRutaRepository rutaRepository, IEstacionRepository estacionRepository)
    {
        _rutaRepository = rutaRepository;
        _estacionRepository = estacionRepository;
    }

    public Task<IReadOnlyCollection<Ruta>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _rutaRepository.GetAllAsync(cancellationToken);
    }

    public Task UpsertAsync(Ruta ruta, CancellationToken cancellationToken = default)
    {
        return _rutaRepository.UpsertAsync(ruta, cancellationToken);
    }

    public async Task<(int Distancia, IReadOnlyList<string> Ruta)> CalcularRutaMasCortaAsync(
        string codigoOrigen,
        string codigoDestino,
        CancellationToken cancellationToken = default)
    {
        var grafo = await ConstruirGrafoAsync(cancellationToken);
        var distancia = CalculadoraRutaDijkstra.CalcularDistancia(grafo, codigoOrigen, codigoDestino);
        var ruta = CalculadoraRutaDijkstra.CalcularRuta(grafo, codigoOrigen, codigoDestino);
        return (distancia, ruta);
    }

    public async Task<Grafo<string>> ConstruirGrafoAsync(CancellationToken cancellationToken = default)
    {
        var estaciones = await _estacionRepository.GetAllAsync(cancellationToken);
        var rutas = await _rutaRepository.GetAllAsync(cancellationToken);

        var grafo = new Grafo<string>();
        foreach (var estacion in estaciones)
        {
            grafo.AddNodo(estacion.CodigoEstacion);
        }

        foreach (var ruta in rutas.Where(x => x.Activa))
        {
            grafo.AddAristaNoDirigida(ruta.CodigoEstacionOrigen, ruta.CodigoEstacionDestino, ruta.DistanciaKm);
        }

        return grafo;
    }
}

