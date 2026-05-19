using TransRail.Domain.Enums;

namespace TransRail.Domain.Entities;

public sealed class Empleado : Usuario
{
    public Empleado()
    {
        Rol = RolUsuario.Empleado;
    }
}

