using TransRail.Application.Services;
using TransRail.Domain.Entities;

namespace TransRail.Application.UseCases.Station;

public sealed class ManageStationUseCase
{
    private readonly EstacionService _estacionService;

    public ManageStationUseCase(EstacionService estacionService)
    {
        _estacionService = estacionService;
    }

    public Task<IReadOnlyCollection<Estacion>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _estacionService.GetAllAsync(cancellationToken);
    }

    public Task UpsertAsync(Estacion estacion, CancellationToken cancellationToken = default)
    {
        return _estacionService.UpsertAsync(estacion, cancellationToken);
    }
}
