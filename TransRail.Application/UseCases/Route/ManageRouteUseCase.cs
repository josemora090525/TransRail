using TransRail.Application.Services;
using TransRail.Domain.Entities;

namespace TransRail.Application.UseCases.Route;

public sealed class ManageRouteUseCase
{
    private readonly RutaService _rutaService;

    public ManageRouteUseCase(RutaService rutaService)
    {
        _rutaService = rutaService;
    }

    public Task<IReadOnlyCollection<Ruta>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _rutaService.GetAllAsync(cancellationToken);
    }

    public Task UpsertAsync(Ruta ruta, CancellationToken cancellationToken = default)
    {
        return _rutaService.UpsertAsync(ruta, cancellationToken);
    }

    public Task<(int Distancia, IReadOnlyList<string> Ruta)> CalculateShortestRouteAsync(
        string codigoOrigen,
        string codigoDestino,
        CancellationToken cancellationToken = default)
    {
        return _rutaService.CalcularRutaMasCortaAsync(codigoOrigen, codigoDestino, cancellationToken);
    }
}
