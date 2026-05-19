using TransRail.Application.Interfaces;
using TransRail.Domain.Entities;

namespace TransRail.Infrastructure.Repositories;

public abstract class JsonRepositoryBase<T> : IRepositoryBase<T>
    where T : class, IConCodigoOperativo
{
    private readonly IJsonStorage _storage;
    private readonly string _fileName;
    private readonly SemaphoreSlim _mutex = new(1, 1);

    protected JsonRepositoryBase(IJsonStorage storage, string fileName)
    {
        _storage = storage;
        _fileName = fileName;
    }

    public virtual async Task<IReadOnlyCollection<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _storage.LoadAsync<T>(_fileName, cancellationToken);
    }

    public virtual async Task<T?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default)
    {
        var all = await GetAllAsync(cancellationToken);
        return all.FirstOrDefault(x => x.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase));
    }

    public virtual async Task UpsertAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _mutex.WaitAsync(cancellationToken);
        try
        {
            var all = (await _storage.LoadAsync<T>(_fileName, cancellationToken)).ToList();
            var index = all.FindIndex(x => x.Codigo.Equals(entity.Codigo, StringComparison.OrdinalIgnoreCase));
            if (index >= 0)
            {
                all[index] = entity;
            }
            else
            {
                all.Add(entity);
            }

            await _storage.SaveAsync(_fileName, all, cancellationToken);
        }
        finally
        {
            _mutex.Release();
        }
    }

    public virtual async Task DeleteAsync(string codigo, CancellationToken cancellationToken = default)
    {
        await _mutex.WaitAsync(cancellationToken);
        try
        {
            var all = (await _storage.LoadAsync<T>(_fileName, cancellationToken)).ToList();
            all.RemoveAll(x => x.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase));
            await _storage.SaveAsync(_fileName, all, cancellationToken);
        }
        finally
        {
            _mutex.Release();
        }
    }
}

