using TransRail.Application.Interfaces;
using TransRail.Domain.Entities;
using TransRail.Domain.Structures;

namespace TransRail.Application.Services;

public sealed class VagonService
{
    private readonly IVagonRepository _repository;

    public VagonService(IVagonRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyCollection<Vagon>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _repository.GetAllAsync(cancellationToken);
    }

    public Task<IReadOnlyCollection<Vagon>> GetByCodigoTrenAsync(string codigoTren, CancellationToken cancellationToken = default)
    {
        return _repository.GetByCodigoTrenAsync(codigoTren, cancellationToken);
    }

    public async Task<Pila<Vagon>> ConstruirPilaPorTrenAsync(string codigoTren, CancellationToken cancellationToken = default)
    {
        var vagones = await _repository.GetByCodigoTrenAsync(codigoTren, cancellationToken);
        var pila = new Pila<Vagon>();
        foreach (var vagon in vagones)
        {
            pila.Push(vagon);
        }

        return pila;
    }

    public Task UpsertAsync(Vagon vagon, CancellationToken cancellationToken = default)
    {
        return _repository.UpsertAsync(vagon, cancellationToken);
    }

    public Task<Vagon?> GetByCodigoAsync(string codigoVagon, CancellationToken cancellationToken = default)
    {
        return _repository.GetByCodigoAsync(codigoVagon, cancellationToken);
    }

    public Task DeleteAsync(string codigoVagon, CancellationToken cancellationToken = default)
    {
        return _repository.DeleteAsync(codigoVagon, cancellationToken);
    }
}
