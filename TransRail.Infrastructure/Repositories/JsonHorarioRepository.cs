using TransRail.Application.Interfaces;
using TransRail.Domain.Entities;

namespace TransRail.Infrastructure.Repositories;

public sealed class JsonHorarioRepository : JsonRepositoryBase<Horario>, IHorarioRepository
{
    public JsonHorarioRepository(IJsonStorage storage)
        : base(storage, "horarios.json")
    {
    }

    public async Task<IReadOnlyCollection<Horario>> GetByCodigoTrenAsync(string codigoTren, CancellationToken cancellationToken = default)
    {
        var all = await GetAllAsync(cancellationToken);
        return all.Where(x => x.CodigoTren.Equals(codigoTren, StringComparison.OrdinalIgnoreCase)).ToArray();
    }
}

