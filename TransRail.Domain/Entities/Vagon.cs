using TransRail.Domain.Enums;

namespace TransRail.Domain.Entities;

public sealed class Vagon : IConCodigoOperativo
{
    public Guid IdInterno { get; set; } = Guid.NewGuid();
    public string CodigoVagon { get; set; } = string.Empty;
    public string CodigoTren { get; set; } = string.Empty;
    public TipoVagon Tipo { get; set; } = TipoVagon.Pasajeros;
    public int Capacidad { get; set; }
    public double PesoMaximoKg { get; set; }

    public string Codigo => CodigoVagon;
}

