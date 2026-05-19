namespace TransRail.Application.Interfaces;

public interface IJsonStorage
{
    Task<IReadOnlyCollection<T>> LoadAsync<T>(string moduleFileName, CancellationToken cancellationToken = default);
    Task SaveAsync<T>(string moduleFileName, IReadOnlyCollection<T> items, CancellationToken cancellationToken = default);
}


