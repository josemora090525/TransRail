using TransRail.Domain.Enums;

namespace TransRail.Presentation;

public sealed class UserSession
{
    public string CodigoUsuario { get; private set; } = string.Empty;
    public string NombreCompleto { get; private set; } = string.Empty;
    public string Correo { get; private set; } = string.Empty;
    public RolUsuario? Rol { get; private set; }

    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(Correo) && Rol is not null;

    public void Start(string codigoUsuario, string nombreCompleto, string correo, RolUsuario rol)
    {
        CodigoUsuario = codigoUsuario;
        NombreCompleto = nombreCompleto;
        Correo = correo;
        Rol = rol;
    }

    public void Clear()
    {
        CodigoUsuario = string.Empty;
        NombreCompleto = string.Empty;
        Correo = string.Empty;
        Rol = null;
    }
}
