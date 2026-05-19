using TransRail.Domain.Entities;

namespace TransRail.Presentation.Views;

public interface IPassengerProfileView
{
    string Nombres { get; }
    string Apellidos { get; }
    string Correo { get; }
    string Direccion { get; }
    string TipoIdentificacion { get; }
    string NumeroIdentificacion { get; }
    string Telefono { get; }
    string NombreContacto { get; }
    string ApellidoContacto { get; }
    string TelefonoContacto { get; }
    string EquipajeDeMano { get; }
    bool EsAdultoMayor { get; }
    bool TieneDiscapacidad { get; }

    event EventHandler? SaveRequested;

    void LoadPassenger(Pasajero pasajero);
    void ShowMessage(string message);
}
