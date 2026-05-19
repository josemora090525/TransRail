using TransRail.Domain.Entities;
using TransRail.Domain.Enums;

namespace TransRail.Presentation.Views;

public interface IPassengerView
{
    string CodigoPasajero { get; }
    string NombreCompleto { get; }
    string NumeroDocumento { get; }
    string Correo { get; }
    string Contrasena { get; }
    CategoriaPasajero Categoria { get; }
    bool EsAdultoMayor { get; }
    bool TieneDiscapacidad { get; }
    string CodigoBusqueda { get; }

    event EventHandler? SaveRequested;
    event EventHandler? DeleteRequested;
    event EventHandler? SearchRequested;
    event EventHandler? RefreshRequested;

    void BindPasajeros(IReadOnlyCollection<Pasajero> pasajeros);
    void FillForm(Pasajero pasajero);
    void ShowMessage(string message);
}
