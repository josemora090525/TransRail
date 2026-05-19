namespace TransRail.Application.DTOs;

public sealed record PassengerScheduleOptionDto(
    string CodigoHorario,
    string CodigoRuta,
    string Origen,
    string Destino,
    DateOnly Fecha,
    TimeOnly HoraSalida,
    TimeOnly HoraLlegada,
    int DistanciaKm,
    bool EsRutaDirecta);
