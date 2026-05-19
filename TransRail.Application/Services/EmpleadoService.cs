using TransRail.Application.Interfaces;
using TransRail.Domain.Entities;

namespace TransRail.Application.Services;

public sealed class EmpleadoService
{
    private readonly IEmpleadoRepository _repository;
    private readonly IUsuarioRepository? _usuarioRepository;

    public EmpleadoService(IEmpleadoRepository repository, IUsuarioRepository? usuarioRepository = null)
    {
        _repository = repository;
        _usuarioRepository = usuarioRepository;
    }

    public Task<IReadOnlyCollection<Empleado>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _repository.GetAllAsync(cancellationToken);
    }

    public async Task UpsertAsync(Empleado empleado, CancellationToken cancellationToken = default)
    {
        await _repository.UpsertAsync(empleado, cancellationToken);

        if (_usuarioRepository is null)
        {
            return;
        }

        var existingUser = await _usuarioRepository.GetByCodigoAsync(empleado.CodigoUsuario, cancellationToken)
            ?? (!string.IsNullOrWhiteSpace(empleado.Correo)
                ? await _usuarioRepository.GetByCorreoAsync(empleado.Correo, cancellationToken)
                : null);

        var userMirror = new Empleado
        {
            IdInterno = existingUser?.IdInterno ?? empleado.IdInterno,
            CodigoUsuario = existingUser?.CodigoUsuario ?? empleado.CodigoUsuario,
            NombreCompleto = empleado.NombreCompleto,
            NumeroDocumento = empleado.NumeroDocumento,
            Correo = empleado.Correo,
            Contrasena = empleado.Contrasena
        };

        await _usuarioRepository.UpsertAsync(userMirror, cancellationToken);
    }

    public Task<Empleado?> GetByCodigoAsync(string codigoEmpleado, CancellationToken cancellationToken = default)
    {
        return _repository.GetByCodigoAsync(codigoEmpleado, cancellationToken);
    }

    public async Task DeleteAsync(string codigoEmpleado, CancellationToken cancellationToken = default)
    {
        Empleado? existing = null;
        if (_usuarioRepository is not null)
        {
            existing = await _repository.GetByCodigoAsync(codigoEmpleado, cancellationToken);
        }

        await _repository.DeleteAsync(codigoEmpleado, cancellationToken);

        if (_usuarioRepository is null || existing is null)
        {
            return;
        }

        var existingUser = await _usuarioRepository.GetByCodigoAsync(existing.CodigoUsuario, cancellationToken)
            ?? (!string.IsNullOrWhiteSpace(existing.Correo)
                ? await _usuarioRepository.GetByCorreoAsync(existing.Correo, cancellationToken)
                : null);

        if (existingUser is not null)
        {
            await _usuarioRepository.DeleteAsync(existingUser.CodigoUsuario, cancellationToken);
        }
    }
}
