using TransRail.Application.Services;
using TransRail.Domain.Entities;

namespace TransRail.Application.UseCases.Passenger;

public sealed class ManagePassengerUseCase
{
    private readonly PasajeroService _pasajeroService;

    public ManagePassengerUseCase(PasajeroService pasajeroService)
    {
        _pasajeroService = pasajeroService;
    }

    public Task<IReadOnlyCollection<Pasajero>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _pasajeroService.GetAllAsync(cancellationToken);
    }

    public Task UpsertAsync(Pasajero pasajero, CancellationToken cancellationToken = default)
    {
        return _pasajeroService.UpsertAsync(pasajero, cancellationToken);
    }

    public Task DeleteAsync(string codigoPasajero, CancellationToken cancellationToken = default)
    {
        return _pasajeroService.DeleteAsync(codigoPasajero, cancellationToken);
    }

    public Task<Pasajero?> GetByCodigoAsync(string codigoPasajero, CancellationToken cancellationToken = default)
    {
        return _pasajeroService.GetByCodigoAsync(codigoPasajero, cancellationToken);
    }
}
