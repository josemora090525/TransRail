using TransRail.Application.UseCases.Passenger;
using TransRail.Presentation.Views;

namespace TransRail.Presentation.Presenters;

public sealed class PassengerPaymentPresenter
{
    private readonly IPassengerPaymentView _view;
    private readonly PassengerPortalUseCase _useCase;

    public PassengerPaymentPresenter(IPassengerPaymentView view, PassengerPortalUseCase useCase)
    {
        _view = view;
        _useCase = useCase;
        _view.PaymentChanged += OnPaymentChanged;
        _view.SaveRequested += OnSaveRequested;
    }

    public void Load()
    {
        _view.LoadDraft(_useCase.GetDraft());
    }

    private void OnSaveRequested(object? sender, EventArgs e)
    {
        try
        {
            _useCase.UpdatePayment(_view.TipoBoleto, _view.MetodoPago);
            _view.LoadDraft(_useCase.GetDraft());
            _view.ShowMessage("El tipo de boleto y el m\u00e9todo de pago quedaron listos.");
        }
        catch (Exception ex)
        {
            _view.ShowMessage($"No se pudo actualizar el pago: {ex.Message}");
        }
    }

    private void OnPaymentChanged(object? sender, EventArgs e)
    {
        try
        {
            _useCase.UpdatePayment(_view.TipoBoleto, _view.MetodoPago);
            _view.LoadDraft(_useCase.GetDraft());
        }
        catch (Exception ex)
        {
            _view.ShowMessage($"No se pudo recalcular el pago: {ex.Message}");
        }
    }
}
