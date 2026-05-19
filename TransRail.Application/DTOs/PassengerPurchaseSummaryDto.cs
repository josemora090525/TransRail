using TransRail.Domain.Entities;
using TransRail.Domain.Enums;

namespace TransRail.Application.DTOs;

public sealed record PassengerPurchaseSummaryDto(
    Pasajero Pasajero,
    string CodigoBoleto,
    string CodigoPago,
    string? CodigoEquipaje,
    string EtiquetaOrigen,
    string EtiquetaDestino,
    int DistanciaKm,
    string RecorridoTexto,
    string CodigoRuta,
    string CodigoHorario,
    DateOnly FechaViaje,
    TimeOnly HoraSalida,
    TimeOnly HoraLlegada,
    string EquipajeDescripcion,
    double EquipajePesoKg,
    string EquipajeDeMano,
    TipoBoleto TipoBoleto,
    MetodoPago MetodoPago,
    decimal PrecioTotal);
