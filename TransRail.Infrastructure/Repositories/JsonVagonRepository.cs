using TransRail.Application.Interfaces;
using TransRail.Domain.Entities;

namespace TransRail.Infrastructure.Repositories;

public sealed class JsonVagonRepository : JsonRepositoryBase<Vagon>, IVagonRepository
{
    public JsonVagonRepository(IJsonStorage storage)
        : base(storage, "vagones.json")
    {
    }

    public async Task<IReadOnlyCollection<Vagon>> GetByCodigoTrenAsync(string codigoTren, CancellationToken cancellationToken = default)
    {
        var all = await GetAllAsync(cancellationToken);
        return all.Where(x => x.CodigoTren.Equals(codigoTren, StringComparison.OrdinalIgnoreCase)).ToArray();
    }
}

