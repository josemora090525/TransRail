using TransRail.Application.Interfaces;
using TransRail.Domain.Entities;

namespace TransRail.Infrastructure.Repositories;

public sealed class JsonUsuarioRepository : JsonRepositoryBase<Usuario>, IUsuarioRepository
{
    public JsonUsuarioRepository(IJsonStorage storage)
        : base(storage, "usuarios.json")
    {
    }

    public async Task<Usuario?> GetByCorreoAsync(string correo, CancellationToken cancellationToken = default)
    {
        var all = await GetAllAsync(cancellationToken);
        return all.FirstOrDefault(x => x.Correo.Equals(correo, StringComparison.OrdinalIgnoreCase));
    }
}

