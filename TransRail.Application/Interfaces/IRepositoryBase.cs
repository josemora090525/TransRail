using TransRail.Domain.Entities;

namespace TransRail.Application.Interfaces;

public interface IRepositoryBase<T> where T : class, IConCodigoOperativo
{
    Task<IReadOnlyCollection<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default);
    Task UpsertAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(string codigo, CancellationToken cancellationToken = default);
}


