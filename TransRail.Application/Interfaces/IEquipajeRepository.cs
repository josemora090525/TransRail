using TransRail.Domain.Entities;

namespace TransRail.Application.Interfaces;

public interface IEquipajeRepository : IRepositoryBase<Equipaje>
{
    Task<IReadOnlyCollection<Equipaje>> GetByCodigoBoletoAsync(string codigoBoleto, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Equipaje>> GetByCodigoVagonCargaAsync(string codigoVagonCarga, CancellationToken cancellationToken = default);
}
