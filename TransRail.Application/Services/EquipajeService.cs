using TransRail.Application.Interfaces;
using TransRail.Domain.Entities;
using TransRail.Domain.Structures;

namespace TransRail.Application.Services;

public sealed class EquipajeService
{
    private readonly IEquipajeRepository _repository;

    public EquipajeService(IEquipajeRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyCollection<Equipaje>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _repository.GetAllAsync(cancellationToken);
    }

    public Task<Equipaje?> GetByCodigoAsync(string codigoEquipaje, CancellationToken cancellationToken = default)
    {
        return _repository.GetByCodigoAsync(codigoEquipaje, cancellationToken);
    }

    public Task<IReadOnlyCollection<Equipaje>> GetByCodigoBoletoAsync(string codigoBoleto, CancellationToken cancellationToken = default)
    {
        return _repository.GetByCodigoBoletoAsync(codigoBoleto, cancellationToken);
    }

    public Task<IReadOnlyCollection<Equipaje>> GetByCodigoVagonCargaAsync(string codigoVagonCarga, CancellationToken cancellationToken = default)
    {
        return _repository.GetByCodigoVagonCargaAsync(codigoVagonCarga, cancellationToken);
    }

    public Task UpsertAsync(Equipaje equipaje, CancellationToken cancellationToken = default)
    {
        return _repository.UpsertAsync(equipaje, cancellationToken);
    }

    public Task DeleteAsync(string codigoEquipaje, CancellationToken cancellationToken = default)
    {
        return _repository.DeleteAsync(codigoEquipaje, cancellationToken);
    }

    public async Task<Pila<Equipaje>> ConstruirPilaPorVagonAsync(string codigoVagonCarga, CancellationToken cancellationToken = default)
    {
        var equipajes = await _repository.GetByCodigoVagonCargaAsync(codigoVagonCarga, cancellationToken);
        var pila = new Pila<Equipaje>();
        foreach (var equipaje in equipajes.OrderBy(x => x.CodigoEquipaje))
        {
            pila.Push(equipaje);
        }

        return pila;
    }
}
