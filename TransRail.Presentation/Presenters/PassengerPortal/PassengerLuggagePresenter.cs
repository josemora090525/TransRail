using TransRail.Application.UseCases.Passenger;
using TransRail.Presentation.Views;

namespace TransRail.Presentation.Presenters;

public sealed class PassengerLuggagePresenter
{
    private readonly IPassengerLuggageView _view;
    private readonly PassengerPortalUseCase _useCase;
    private readonly UserSession _session;

    public PassengerLuggagePresenter(IPassengerLuggageView view, PassengerPortalUseCase useCase, UserSession session)
    {
        _view = view;
        _useCase = useCase;
        _session = session;
        _view.SaveRequested += OnSaveRequested;
    }

    public async Task LoadAsync()
    {
        var passenger = await _useCase.GetOrCreatePassengerAsync(_session.Correo, _session.CodigoUsuario, _session.NombreCompleto);
        var draft = _useCase.GetDraft() with { EquipajeDeMano = string.IsNullOrWhiteSpace(_useCase.GetDraft().EquipajeDeMano) ? passenger.EquipajeDeMano : _useCase.GetDraft().EquipajeDeMano };
        _view.LoadDraft(draft);
    }

    private void OnSaveRequested(object? sender, EventArgs e)
    {
        try
        {
            _useCase.UpdateLuggage(_view.EquipajeDeMano, _view.EquipajeDescripcion, _view.EquipajePesoKg);
            _view.ShowMessage("La informaci\u00f3n de equipaje qued\u00f3 guardada en tu compra.");
        }
        catch (Exception ex)
        {
            _view.ShowMessage($"No se pudo guardar el equipaje: {ex.Message}");
        }
    }
}
