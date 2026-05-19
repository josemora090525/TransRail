using TransRail.Domain.Enums;

namespace TransRail.Domain.Entities;

public sealed class Administrador : Usuario
{
    public Administrador()
    {
        Rol = RolUsuario.Administrador;
    }
}

