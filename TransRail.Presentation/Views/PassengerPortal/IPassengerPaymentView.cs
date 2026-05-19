using TransRail.Application.DTOs;
using TransRail.Domain.Enums;

namespace TransRail.Presentation.Views;

public interface IPassengerPaymentView
{
    TipoBoleto TipoBoleto { get; }
    MetodoPago MetodoPago { get; }

    event EventHandler? PaymentChanged;
    event EventHandler? SaveRequested;

    void LoadDraft(PassengerPurchaseDraftDto draft);
    void ShowMessage(string message);
}
