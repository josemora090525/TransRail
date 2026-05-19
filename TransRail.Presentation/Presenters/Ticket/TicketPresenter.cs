using TransRail.Application.UseCases.Ticket;
using TransRail.Domain.Entities;
using TransRail.Presentation.Views;

namespace TransRail.Presentation.Presenters;

public sealed class TicketPresenter
{
    private readonly ITicketView _view;
    private readonly TicketPurchaseUseCase _useCase;

    public TicketPresenter(ITicketView view, TicketPurchaseUseCase useCase)
    {
        _view = view;
        _useCase = useCase;
        _view.SaveRequested += OnSaveRequested;
        _view.DeleteRequested += OnDeleteRequested;
        _view.SearchRequested += OnSearchRequested;
        _view.RefreshRequested += OnRefreshRequested;
        _view.CalculatePriceRequested += OnCalculatePriceRequested;
        _view.ShowHistoryRequested += OnShowHistoryRequested;
        _view.ShowHistoryReverseRequested += OnShowHistoryReverseRequested;
    }

    private async void OnSaveRequested(object? sender, EventArgs e)
    {
        try
        {
            var boleto = new Boleto
            {
                CodigoBoleto = _view.CodigoBoleto,
                CodigoPasajero = _view.CodigoPasajero,
                CodigoHorario = _view.CodigoHorario,
                TipoBoleto = _view.TipoBoleto,
                Precio = _view.Precio
            };

            var result = await _useCase.ExecutePurchaseAsync(boleto);
            if (!result.Ok)
            {
                _view.ShowMessage(result.Error);
                return;
            }

            _view.ShowMessage("Boleto guardado correctamente.");
            await RefrescarAsync();
        }
        catch (Exception ex)
        {
            _view.ShowMessage($"No se pudo guardar el boleto: {ex.Message}");
        }
    }

    private async void OnDeleteRequested(object? sender, EventArgs e)
    {
        var codigo = ResolveSearchCode();
        if (string.IsNullOrWhiteSpace(codigo))
        {
            _view.ShowMessage("Debes indicar un código para eliminar.");
            return;
        }

        await _useCase.DeleteAsync(codigo);
        _view.ShowMessage("Boleto eliminado.");
        await RefrescarAsync();
    }

    private async void OnSearchRequested(object? sender, EventArgs e)
    {
        var codigo = ResolveSearchCode();
        if (string.IsNullOrWhiteSpace(codigo))
        {
            _view.ShowMessage("Debes indicar un código para buscar.");
            return;
        }

        var boleto = await _useCase.GetByCodigoAsync(codigo);
        if (boleto is null)
        {
            _view.ShowMessage("Boleto no encontrado.");
            return;
        }

        _view.FillForm(boleto);
    }

    private async void OnRefreshRequested(object? sender, EventArgs e)
    {
        await RefrescarAsync();
    }

    private void OnCalculatePriceRequested(object? sender, EventArgs e)
    {
        if (_view.DistanciaKm <= 0)
        {
            _view.ShowMessage("La distancia debe ser mayor que cero para calcular el precio.");
            return;
        }

        var precio = _useCase.CalculatePrice(_view.DistanciaKm, _view.TipoBoleto);
        _view.SetPrecio(precio);
    }

    private void OnShowHistoryRequested(object? sender, EventArgs e)
    {
        _view.BindBoletos(_useCase.GetHistoryForward());
    }

    private void OnShowHistoryReverseRequested(object? sender, EventArgs e)
    {
        _view.BindBoletos(_useCase.GetHistoryReverse());
    }

    public async Task RefrescarAsync()
    {
        var boletos = await _useCase.GetAllAsync();
        _view.BindBoletos(boletos.OrderBy(x => x.FechaCompraUtc).ToArray());
    }

    private string ResolveSearchCode()
    {
        if (!string.IsNullOrWhiteSpace(_view.CodigoBusqueda))
        {
            return _view.CodigoBusqueda;
        }

        return _view.CodigoBoleto;
    }
}
