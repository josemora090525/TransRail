using TransRail.Application.Interfaces;
using TransRail.Domain.Entities;

namespace TransRail.Infrastructure.Repositories;

public sealed class JsonPasajeroRepository : JsonRepositoryBase<Pasajero>, IPasajeroRepository
{
    public JsonPasajeroRepository(IJsonStorage storage)
        : base(storage, "pasajeros.json")
    {
    }

    public async Task<Pasajero?> GetByCorreoAsync(string correo, CancellationToken cancellationToken = default)
    {
        var all = await GetAllAsync(cancellationToken);
        return all.FirstOrDefault(x => x.Correo.Equals(correo, StringComparison.OrdinalIgnoreCase));
    }
}
