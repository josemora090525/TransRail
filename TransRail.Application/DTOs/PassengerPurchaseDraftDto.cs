using TransRail.Domain.Enums;

namespace TransRail.Application.DTOs;

public sealed record PassengerPurchaseDraftDto(
    string CodigoOrigen,
    string CodigoDestino,
    string EtiquetaOrigen,
    string EtiquetaDestino,
    int DistanciaKm,
    string RecorridoTexto,
    string CodigoHorario,
    string CodigoRuta,
    DateOnly? FechaViaje,
    TimeOnly? HoraSalida,
    TimeOnly? HoraLlegada,
    string EquipajeDescripcion,
    double EquipajePesoKg,
    string EquipajeDeMano,
    TipoBoleto TipoBoleto,
    MetodoPago MetodoPago,
    decimal PrecioCalculado);
