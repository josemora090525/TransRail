using TransRail.Domain.Entities;
using TransRail.Domain.Enums;

namespace TransRail.Presentation.Views;

public interface ITicketView
{
    string CodigoBoleto { get; }
    string CodigoPasajero { get; }
    string CodigoHorario { get; }
    TipoBoleto TipoBoleto { get; }
    decimal Precio { get; }
    int DistanciaKm { get; }
    string CodigoBusqueda { get; }

    event EventHandler? SaveRequested;
    event EventHandler? DeleteRequested;
    event EventHandler? SearchRequested;
    event EventHandler? RefreshRequested;
    event EventHandler? CalculatePriceRequested;
    event EventHandler? ShowHistoryRequested;
    event EventHandler? ShowHistoryReverseRequested;

    void BindBoletos(IReadOnlyCollection<Boleto> boletos);
    void FillForm(Boleto boleto);
    void SetPrecio(decimal precio);
    void ShowMessage(string message);
}
