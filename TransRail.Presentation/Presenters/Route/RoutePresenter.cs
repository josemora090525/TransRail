using TransRail.Application.UseCases.Route;
using TransRail.Domain.Entities;
using TransRail.Presentation.Views;

namespace TransRail.Presentation.Presenters;

public sealed class RoutePresenter
{
    private readonly IRouteView _view;
    private readonly ManageRouteUseCase _useCase;

    public RoutePresenter(IRouteView view, ManageRouteUseCase useCase)
    {
        _view = view;
        _useCase = useCase;
        _view.CreateRequested += OnCreateRequested;
        _view.RefreshRequested += OnRefreshRequested;
        _view.CalculateRequested += OnCalculateRequested;
    }

    private async void OnCreateRequested(object? sender, EventArgs e)
    {
        try
        {
            var ruta = new Ruta
            {
                CodigoRuta = _view.CodigoRuta,
                CodigoEstacionOrigen = _view.CodigoOrigen,
                CodigoEstacionDestino = _view.CodigoDestino,
                DistanciaKm = _view.DistanciaKm,
                Activa = true
            };

            await _useCase.UpsertAsync(ruta);
            _view.ShowMessage("Ruta guardada correctamente.");
            await RefrescarAsync();
        }
        catch (Exception ex)
        {
            _view.ShowMessage($"No se pudo guardar la ruta: {ex.Message}");
        }
    }

    private async void OnRefreshRequested(object? sender, EventArgs e)
    {
        await RefrescarAsync();
    }

    private async void OnCalculateRequested(object? sender, EventArgs e)
    {
        try
        {
            var result = await _useCase.CalculateShortestRouteAsync(_view.CalculoOrigen, _view.CalculoDestino);
            _view.ShowRouteCalculation(result.Distancia, result.Ruta);
        }
        catch (Exception ex)
        {
            _view.ShowMessage($"No se pudo calcular la ruta: {ex.Message}");
        }
    }

    public async Task RefrescarAsync()
    {
        var rutas = await _useCase.GetAllAsync();
        _view.BindRutas(rutas);
    }
}
