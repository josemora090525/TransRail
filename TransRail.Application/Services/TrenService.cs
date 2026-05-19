using TransRail.Application.Interfaces;
using TransRail.Domain.Entities;
using TransRail.Domain.Structures;

namespace TransRail.Application.Services;

public sealed class TrenService
{
    private readonly ITrenRepository _repository;
    private readonly ListaCircular<Tren> _trenesEnCirculacion = new();

    public TrenService(ITrenRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Tren>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.GetAllAsync(cancellationToken);
    }

    public async Task UpsertAsync(Tren tren, CancellationToken cancellationToken = default)
    {
        await _repository.UpsertAsync(tren, cancellationToken);
        ActualizarCirculacion(tren);
    }

    public async Task DeleteAsync(string codigoTren, CancellationToken cancellationToken = default)
    {
        await _repository.DeleteAsync(codigoTren, cancellationToken);
        _trenesEnCirculacion.Remove(x => x.CodigoTren.Equals(codigoTren, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<ListaCircular<Tren>> GetTrenesEnCirculacionAsync(CancellationToken cancellationToken = default)
    {
        var all = await _repository.GetAllAsync(cancellationToken);
        foreach (var tren in all.Where(x => x.EnCirculacion))
        {
            ActualizarCirculacion(tren);
        }

        return _trenesEnCirculacion;
    }

    private void ActualizarCirculacion(Tren tren)
    {
        _trenesEnCirculacion.Remove(x => x.CodigoTren.Equals(tren.CodigoTren, StringComparison.OrdinalIgnoreCase));
        if (tren.EnCirculacion)
        {
            _trenesEnCirculacion.Add(tren);
        }
    }
}

