using TransRail.Application.Interfaces;
using TransRail.Domain.Entities;

namespace TransRail.Infrastructure.Repositories;

public sealed class JsonEquipajeRepository : JsonRepositoryBase<Equipaje>, IEquipajeRepository
{
    public JsonEquipajeRepository(IJsonStorage storage)
        : base(storage, "equipajes.json")
    {
    }

    public async Task<IReadOnlyCollection<Equipaje>> GetByCodigoBoletoAsync(string codigoBoleto, CancellationToken cancellationToken = default)
    {
        var all = await GetAllAsync(cancellationToken);
        return all.Where(x => x.CodigoBoleto.Equals(codigoBoleto, StringComparison.OrdinalIgnoreCase)).ToArray();
    }

    public async Task<IReadOnlyCollection<Equipaje>> GetByCodigoVagonCargaAsync(string codigoVagonCarga, CancellationToken cancellationToken = default)
    {
        var all = await GetAllAsync(cancellationToken);
        return all.Where(x => x.CodigoVagonCarga.Equals(codigoVagonCarga, StringComparison.OrdinalIgnoreCase)).ToArray();
    }
}
