using TransRail.Application.DTOs;

namespace TransRail.Presentation.Views;

public interface IPassengerLuggageView
{
    string EquipajeDeMano { get; }
    string EquipajeDescripcion { get; }
    double EquipajePesoKg { get; }

    event EventHandler? SaveRequested;

    void LoadDraft(PassengerPurchaseDraftDto draft);
    void ShowMessage(string message);
}
