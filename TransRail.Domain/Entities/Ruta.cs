namespace TransRail.Domain.Entities;

public sealed class Ruta : IConCodigoOperativo
{
    public Guid IdInterno { get; set; } = Guid.NewGuid();
    public string CodigoRuta { get; set; } = string.Empty;
    public string CodigoEstacionOrigen { get; set; } = string.Empty;
    public string CodigoEstacionDestino { get; set; } = string.Empty;
    public int DistanciaKm { get; set; }
    public bool Activa { get; set; } = true;

    public string Codigo => CodigoRuta;
}

