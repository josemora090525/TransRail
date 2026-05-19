using TransRail.Domain.Entities;

namespace TransRail.Application.Interfaces;

public interface IVagonRepository : IRepositoryBase<Vagon>
{
    Task<IReadOnlyCollection<Vagon>> GetByCodigoTrenAsync(string codigoTren, CancellationToken cancellationToken = default);
}


