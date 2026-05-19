using System.Text.Json.Serialization;
using TransRail.Domain.Enums;

namespace TransRail.Domain.Entities;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$tipo")]
[JsonDerivedType(typeof(Administrador), "administrador")]
[JsonDerivedType(typeof(Empleado), "empleado")]
[JsonDerivedType(typeof(Pasajero), "pasajero")]
public abstract class Usuario : IConCodigoOperativo
{
    public Guid IdInterno { get; set; } = Guid.NewGuid();
    public string CodigoUsuario { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string NumeroDocumento { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Contrasena { get; set; } = string.Empty;
    public RolUsuario Rol { get; protected set; }

    public string Codigo => CodigoUsuario;
}
