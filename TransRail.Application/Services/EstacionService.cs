using TransRail.Application.Interfaces;
using TransRail.Domain.Entities;

namespace TransRail.Application.Services;

public sealed class EstacionService
{
    private readonly IEstacionRepository _repository;

    public EstacionService(IEstacionRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyCollection<Estacion>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _repository.GetAllAsync(cancellationToken);
    }

    public Task UpsertAsync(Estacion estacion, CancellationToken cancellationToken = default)
    {
        return _repository.UpsertAsync(estacion, cancellationToken);
    }
}

