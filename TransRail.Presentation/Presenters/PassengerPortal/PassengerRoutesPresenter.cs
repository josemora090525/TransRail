using TransRail.Application.DTOs;
using TransRail.Application.UseCases.Passenger;
using TransRail.Presentation.Views;

namespace TransRail.Presentation.Presenters;

public sealed class PassengerRoutesPresenter
{
    private readonly IPassengerRoutesView _view;
    private readonly PassengerPortalUseCase _useCase;
    private PassengerRouteSearchResultDto? _lastResult;

    public PassengerRoutesPresenter(IPassengerRoutesView view, PassengerPortalUseCase useCase)
    {
        _view = view;
        _useCase = useCase;
        _view.SearchRequested += OnSearchRequested;
        _view.SelectRequested += OnSelectRequested;
    }

    public async Task InitializeAsync()
    {
        var stations = await _useCase.GetStationOptionsAsync();
        _view.BindStations(stations);
        _view.ShowRouteSummary(
            new PassengerRouteSearchResultDto(
                string.Empty,
                string.Empty,
                "A\u00fan no has buscado una ruta",
                string.Empty,
                0,
                Array.Empty<string>(),
                Array.Empty<PassengerScheduleOptionDto>()),
            "Selecciona origen, destino y luego un horario para continuar.");
    }

    private async void OnSearchRequested(object? sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_view.CodigoOrigen) || string.IsNullOrWhiteSpace(_view.CodigoDestino))
            {
                _view.ShowMessage("Debes seleccionar origen y destino.");
                return;
            }

            _lastResult = await _useCase.SearchRoutesAsync(_view.CodigoOrigen, _view.CodigoDestino);
            _view.BindSchedules(_lastResult.HorariosDisponibles);
            _view.ShowRouteSummary(
                _lastResult,
                _lastResult.HorariosDisponibles.Count == 0
                    ? "No hay horarios asociados a esta ruta por ahora."
                    : "Selecciona un horario disponible y guarda la ruta.");
        }
        catch (Exception ex)
        {
            _view.ShowMessage($"No se pudo calcular la ruta: {ex.Message}");
        }
    }

    private async void OnSelectRequested(object? sender, EventArgs e)
    {
        try
        {
            if (_lastResult is null)
            {
                _view.ShowMessage("Primero debes buscar una ruta.");
                return;
            }

            if (string.IsNullOrWhiteSpace(_view.CodigoHorarioSeleccionado))
            {
                _view.ShowMessage("Debes seleccionar un horario de la lista.");
                return;
            }

            await _useCase.SelectScheduleAsync(_lastResult.CodigoOrigen, _lastResult.CodigoDestino, _view.CodigoHorarioSeleccionado);
            var selected = _lastResult.HorariosDisponibles.First(x => x.CodigoHorario.Equals(_view.CodigoHorarioSeleccionado, StringComparison.OrdinalIgnoreCase));
            _view.ShowRouteSummary(
                _lastResult,
                $"Horario seleccionado: {selected.CodigoHorario} | Salida {selected.HoraSalida:HH\\:mm} | Llegada {selected.HoraLlegada:HH\\:mm}");
            _view.ShowMessage("La ruta y el horario quedaron listos para continuar con la compra.");
        }
        catch (Exception ex)
        {
            _view.ShowMessage($"No se pudo seleccionar el horario: {ex.Message}");
        }
    }
}
