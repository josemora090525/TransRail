using TransRail.Application.Interfaces;
using TransRail.Domain.Entities;
using TransRail.Domain.Structures;

namespace TransRail.Application.Services;

public sealed class HorarioService
{
    private readonly IHorarioRepository _repository;
    private readonly ArbolAvl<Horario> _indiceAvl = new();
    private readonly SemaphoreSlim _mutex = new(1, 1);
    private bool _cargado;

    public HorarioService(IHorarioRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Horario>> GetAllOrdenadosAsync(CancellationToken cancellationToken = default)
    {
        await EnsureLoadedAsync(cancellationToken);
        return _indiceAvl.ToArray();
    }

    public async Task UpsertAsync(Horario horario, CancellationToken cancellationToken = default)
    {
        await _mutex.WaitAsync(cancellationToken);
        try
        {
            await EnsureLoadedAsync(cancellationToken);
            var previo = await _repository.GetByCodigoAsync(horario.CodigoHorario, cancellationToken);
            if (previo is not null)
            {
                _indiceAvl.Remove(previo);
            }

            _indiceAvl.Insert(horario);
            await _repository.UpsertAsync(horario, cancellationToken);
        }
        finally
        {
            _mutex.Release();
        }
    }

    public async Task DeleteAsync(string codigoHorario, CancellationToken cancellationToken = default)
    {
        await _mutex.WaitAsync(cancellationToken);
        try
        {
            await EnsureLoadedAsync(cancellationToken);
            var previo = await _repository.GetByCodigoAsync(codigoHorario, cancellationToken);
            if (previo is not null)
            {
                _indiceAvl.Remove(previo);
            }

            await _repository.DeleteAsync(codigoHorario, cancellationToken);
        }
        finally
        {
            _mutex.Release();
        }
    }

    public async Task<IReadOnlyCollection<Horario>> BuscarPorCodigoTrenAsync(string codigoTren, CancellationToken cancellationToken = default)
    {
        await EnsureLoadedAsync(cancellationToken);
        return _indiceAvl.Where(x => x.CodigoTren.Equals(codigoTren, StringComparison.OrdinalIgnoreCase)).ToArray();
    }

    private async Task EnsureLoadedAsync(CancellationToken cancellationToken)
    {
        if (_cargado)
        {
            return;
        }

        var all = await _repository.GetAllAsync(cancellationToken);
        foreach (var horario in all)
        {
            _indiceAvl.Insert(horario);
        }

        _cargado = true;
    }
}

