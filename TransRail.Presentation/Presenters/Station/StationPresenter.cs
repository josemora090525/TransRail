using TransRail.Application.UseCases.Station;
using TransRail.Domain.Entities;
using TransRail.Presentation.Views;

namespace TransRail.Presentation.Presenters;

public sealed class StationPresenter
{
    private readonly IStationView _view;
    private readonly ManageStationUseCase _useCase;

    public StationPresenter(IStationView view, ManageStationUseCase useCase)
    {
        _view = view;
        _useCase = useCase;
        _view.CreateRequested += OnCreateRequested;
        _view.RefreshRequested += OnRefreshRequested;
    }

    private async void OnCreateRequested(object? sender, EventArgs e)
    {
        try
        {
            var estacion = new Estacion
            {
                CodigoEstacion = _view.CodigoEstacion,
                Nombre = _view.NombreEstacion,
                Ciudad = _view.CiudadEstacion
            };

            await _useCase.UpsertAsync(estacion);
            _view.ShowMessage("Estación guardada correctamente.");
            await RefrescarAsync();
        }
        catch (Exception ex)
        {
            _view.ShowMessage($"No se pudo guardar la estación: {ex.Message}");
        }
    }

    private async void OnRefreshRequested(object? sender, EventArgs e)
    {
        await RefrescarAsync();
    }

    public async Task RefrescarAsync()
    {
        var estaciones = await _useCase.GetAllAsync();
        _view.BindEstaciones(estaciones);
    }
}
