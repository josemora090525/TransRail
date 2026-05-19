namespace TransRail.Application.DTOs;

public sealed record PassengerStationOptionDto(string CodigoEstacion, string Nombre, string Ciudad)
{
    public string Etiqueta => $"{Ciudad} - {Nombre} ({CodigoEstacion})";
}
