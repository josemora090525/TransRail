using TransRail.Domain.Enums;

namespace TransRail.Application.DTOs;

public sealed record LoginResultDto(
    bool Exitoso,
    string Mensaje,
    RolUsuario? Rol,
    string? CodigoUsuario = null,
    string? NombreCompleto = null,
    string? Correo = null);
