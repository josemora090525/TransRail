using TransRail.Application.Interfaces;
using TransRail.Domain.Entities;

namespace TransRail.Application.Services;

public sealed class UsuarioService
{
    private readonly IUsuarioRepository _repository;

    public UsuarioService(IUsuarioRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyCollection<Usuario>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _repository.GetAllAsync(cancellationToken);
    }

    public Task<Usuario?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default)
    {
        return _repository.GetByCodigoAsync(codigo, cancellationToken);
    }

    public Task UpsertAsync(Usuario usuario, CancellationToken cancellationToken = default)
    {
        return _repository.UpsertAsync(usuario, cancellationToken);
    }

    public Task DeleteAsync(string codigo, CancellationToken cancellationToken = default)
    {
        return _repository.DeleteAsync(codigo, cancellationToken);
    }
}

