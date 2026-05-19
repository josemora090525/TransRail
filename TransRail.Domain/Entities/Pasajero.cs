using TransRail.Domain.Enums;

namespace TransRail.Domain.Entities;

public sealed class Pasajero : Usuario
{
    public Pasajero()
    {
        Rol = RolUsuario.Pasajero;
    }

    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string TipoIdentificacion { get; set; } = "CC";
    public string Direccion { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string NombreContacto { get; set; } = string.Empty;
    public string ApellidoContacto { get; set; } = string.Empty;
    public string TelefonoContacto { get; set; } = string.Empty;
    public string EquipajeDeMano { get; set; } = string.Empty;
    public CategoriaPasajero Categoria { get; set; } = CategoriaPasajero.Estandar;
    public bool EsAdultoMayor { get; set; }
    public bool TieneDiscapacidad { get; set; }
}
