using TransRail.Application.DTOs;

namespace TransRail.Presentation.Views;

public interface IPassengerRoutesView
{
    string CodigoOrigen { get; }
    string CodigoDestino { get; }
    string CodigoHorarioSeleccionado { get; }

    event EventHandler? SearchRequested;
    event EventHandler? SelectRequested;

    void BindStations(IReadOnlyCollection<PassengerStationOptionDto> estaciones);
    void BindSchedules(IReadOnlyCollection<PassengerScheduleOptionDto> horarios);
    void ShowRouteSummary(PassengerRouteSearchResultDto result, string selectedScheduleText);
    void ShowMessage(string message);
}
