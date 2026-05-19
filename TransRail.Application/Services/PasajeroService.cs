using TransRail.Application.Interfaces;
using TransRail.Domain.Entities;

namespace TransRail.Application.Services;

public sealed class PasajeroService
{
    private readonly IPasajeroRepository _repository;
    private readonly IUsuarioRepository? _usuarioRepository;

    public PasajeroService(IPasajeroRepository repository, IUsuarioRepository? usuarioRepository = null)
    {
        _repository = repository;
        _usuarioRepository = usuarioRepository;
    }

    public Task<IReadOnlyCollection<Pasajero>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _repository.GetAllAsync(cancellationToken);
    }

    public async Task UpsertAsync(Pasajero pasajero, CancellationToken cancellationToken = default)
    {
        NormalizePassenger(pasajero);
        var existingUser = await ResolveExistingUserAsync(pasajero, cancellationToken);
        if (string.IsNullOrWhiteSpace(pasajero.Contrasena) && existingUser is not null)
        {
            pasajero.Contrasena = existingUser.Contrasena;
        }

        await _repository.UpsertAsync(pasajero, cancellationToken);

        if (_usuarioRepository is null)
        {
            return;
        }

        var userMirror = ClonePassenger(pasajero);
        if (existingUser is not null)
        {
            userMirror.CodigoUsuario = existingUser.CodigoUsuario;
            userMirror.IdInterno = existingUser.IdInterno;
        }

        await _usuarioRepository.UpsertAsync(userMirror, cancellationToken);
    }

    public async Task DeleteAsync(string codigoPasajero, CancellationToken cancellationToken = default)
    {
        Pasajero? existing = null;
        if (_usuarioRepository is not null)
        {
            existing = await _repository.GetByCodigoAsync(codigoPasajero, cancellationToken);
        }

        await _repository.DeleteAsync(codigoPasajero, cancellationToken);
        if (_usuarioRepository is null)
        {
            return;
        }

        if (existing is not null)
        {
            var existingUser = await ResolveExistingUserAsync(existing, cancellationToken);
            if (existingUser is not null)
            {
                await _usuarioRepository.DeleteAsync(existingUser.CodigoUsuario, cancellationToken);
            }
        }
    }

    public Task<Pasajero?> GetByCodigoAsync(string codigoPasajero, CancellationToken cancellationToken = default)
    {
        return _repository.GetByCodigoAsync(codigoPasajero, cancellationToken);
    }

    public Task<Pasajero?> GetByCorreoAsync(string correo, CancellationToken cancellationToken = default)
    {
        return _repository.GetByCorreoAsync(correo, cancellationToken);
    }

    private static void NormalizePassenger(Pasajero pasajero)
    {
        pasajero.Nombres = pasajero.Nombres.Trim();
        pasajero.Apellidos = pasajero.Apellidos.Trim();
        pasajero.TipoIdentificacion = pasajero.TipoIdentificacion.Trim();
        pasajero.Direccion = pasajero.Direccion.Trim();
        pasajero.Telefono = pasajero.Telefono.Trim();
        pasajero.NombreContacto = pasajero.NombreContacto.Trim();
        pasajero.ApellidoContacto = pasajero.ApellidoContacto.Trim();
        pasajero.TelefonoContacto = pasajero.TelefonoContacto.Trim();
        pasajero.EquipajeDeMano = pasajero.EquipajeDeMano.Trim();

        if (string.IsNullOrWhiteSpace(pasajero.Nombres) && !string.IsNullOrWhiteSpace(pasajero.NombreCompleto))
        {
            var parts = pasajero.NombreCompleto.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            pasajero.Nombres = parts.ElementAtOrDefault(0) ?? string.Empty;
            pasajero.Apellidos = parts.ElementAtOrDefault(1) ?? string.Empty;
        }

        var fullName = $"{pasajero.Nombres} {pasajero.Apellidos}".Trim();
        if (!string.IsNullOrWhiteSpace(fullName))
        {
            pasajero.NombreCompleto = fullName;
        }
    }

    private async Task<Usuario?> ResolveExistingUserAsync(Pasajero pasajero, CancellationToken cancellationToken)
    {
        if (_usuarioRepository is null)
        {
            return null;
        }

        var byCode = await _usuarioRepository.GetByCodigoAsync(pasajero.CodigoUsuario, cancellationToken);
        if (byCode is not null)
        {
            return byCode;
        }

        if (!string.IsNullOrWhiteSpace(pasajero.Correo))
        {
            return await _usuarioRepository.GetByCorreoAsync(pasajero.Correo, cancellationToken);
        }

        return null;
    }

    private static Pasajero ClonePassenger(Pasajero source)
    {
        return new Pasajero
        {
            IdInterno = source.IdInterno,
            CodigoUsuario = source.CodigoUsuario,
            NombreCompleto = source.NombreCompleto,
            NumeroDocumento = source.NumeroDocumento,
            Correo = source.Correo,
            Contrasena = source.Contrasena,
            Nombres = source.Nombres,
            Apellidos = source.Apellidos,
            TipoIdentificacion = source.TipoIdentificacion,
            Direccion = source.Direccion,
            Telefono = source.Telefono,
            NombreContacto = source.NombreContacto,
            ApellidoContacto = source.ApellidoContacto,
            TelefonoContacto = source.TelefonoContacto,
            EquipajeDeMano = source.EquipajeDeMano,
            Categoria = source.Categoria,
            EsAdultoMayor = source.EsAdultoMayor,
            TieneDiscapacidad = source.TieneDiscapacidad
        };
    }
}
