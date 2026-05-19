using TransRail.Application.Interfaces;
using TransRail.Domain.Entities;

namespace TransRail.Infrastructure.Repositories;

public sealed class JsonPagoRepository : JsonRepositoryBase<Pago>, IPagoRepository
{
    public JsonPagoRepository(IJsonStorage storage)
        : base(storage, "pagos.json")
    {
    }

    public async Task<IReadOnlyCollection<Pago>> GetByCodigoBoletoAsync(string codigoBoleto, CancellationToken cancellationToken = default)
    {
        var all = await GetAllAsync(cancellationToken);
        return all.Where(x => x.CodigoBoleto.Equals(codigoBoleto, StringComparison.OrdinalIgnoreCase)).ToArray();
    }
}
