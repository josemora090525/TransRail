using TransRail.Application.DTOs;
using TransRail.Application.Interfaces;

namespace TransRail.Application.Services;

public sealed class AuthService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public AuthService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<LoginResultDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Correo) || string.IsNullOrWhiteSpace(request.Contrasena))
        {
            return new LoginResultDto(false, "Debe ingresar correo y contrasena.", null);
        }

        var usuario = await _usuarioRepository.GetByCorreoAsync(request.Correo, cancellationToken);
        if (usuario is null)
        {
            return new LoginResultDto(false, "Usuario no encontrado.", null);
        }

        if (!string.Equals(usuario.Contrasena, request.Contrasena, StringComparison.Ordinal))
        {
            return new LoginResultDto(false, "Contrasena invalida.", null);
        }

        return new LoginResultDto(
            true,
            $"Bienvenido {usuario.NombreCompleto}",
            usuario.Rol,
            usuario.CodigoUsuario,
            usuario.NombreCompleto,
            usuario.Correo);
    }
}
