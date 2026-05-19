using TransRail.Application.Services;
using TransRail.Domain.Entities;
using TransRail.Domain.Structures;

namespace TransRail.Application.UseCases.Luggage;

public sealed class LuggageOperationsUseCase
{
    private readonly EquipajeService _equipajeService;

    public LuggageOperationsUseCase(EquipajeService equipajeService)
    {
        _equipajeService = equipajeService;
    }

    public Task UpsertAsync(Equipaje equipaje, CancellationToken cancellationToken = default)
    {
        return _equipajeService.UpsertAsync(equipaje, cancellationToken);
    }

    public Task DeleteAsync(string codigoEquipaje, CancellationToken cancellationToken = default)
    {
        return _equipajeService.DeleteAsync(codigoEquipaje, cancellationToken);
    }

    public Task<IReadOnlyCollection<Equipaje>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _equipajeService.GetAllAsync(cancellationToken);
    }

    public Task<IReadOnlyCollection<Equipaje>> GetByCodigoVagonAsync(string codigoVagonCarga, CancellationToken cancellationToken = default)
    {
        return _equipajeService.GetByCodigoVagonCargaAsync(codigoVagonCarga, cancellationToken);
    }

    public Task<Pila<Equipaje>> BuildStackByVagonAsync(string codigoVagonCarga, CancellationToken cancellationToken = default)
    {
        return _equipajeService.ConstruirPilaPorVagonAsync(codigoVagonCarga, cancellationToken);
    }
}
