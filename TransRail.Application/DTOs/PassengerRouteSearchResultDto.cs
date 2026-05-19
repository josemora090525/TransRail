namespace TransRail.Application.DTOs;

public sealed record PassengerRouteSearchResultDto(
    string CodigoOrigen,
    string CodigoDestino,
    string EtiquetaOrigen,
    string EtiquetaDestino,
    int DistanciaKm,
    IReadOnlyList<string> Recorrido,
    IReadOnlyCollection<PassengerScheduleOptionDto> HorariosDisponibles);
