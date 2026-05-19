using TransRail.Domain.Entities;

namespace TransRail.Application.Interfaces;

public interface IPagoRepository : IRepositoryBase<Pago>
{
    Task<IReadOnlyCollection<Pago>> GetByCodigoBoletoAsync(string codigoBoleto, CancellationToken cancellationToken = default);
}
