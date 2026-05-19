using TransRail.Application.DTOs;
using TransRail.Domain.Entities;

namespace TransRail.Presentation.Views;

public interface IPassengerCheckoutView
{
    event EventHandler? RefreshRequested;
    event EventHandler? ConfirmRequested;

    void LoadSummary(PassengerPurchaseSummaryDto? summary, string statusText);
    void BindTickets(IReadOnlyCollection<Boleto> boletos);
    void ShowMessage(string message);
    void ShowPurchasePopup(string details);
}
