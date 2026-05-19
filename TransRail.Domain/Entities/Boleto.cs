using TransRail.Domain.Enums;

namespace TransRail.Domain.Entities;

public sealed class Boleto : IConCodigoOperativo
{
    public Guid IdInterno { get; set; } = Guid.NewGuid();
    public string CodigoBoleto { get; set; } = string.Empty;
    public string CodigoPasajero { get; set; } = string.Empty;
    public string CodigoHorario { get; set; } = string.Empty;
    public TipoBoleto TipoBoleto { get; set; } = TipoBoleto.Estandar;
    public decimal Precio { get; set; }
    public DateTime FechaCompraUtc { get; set; } = DateTime.UtcNow;

    public string Codigo => CodigoBoleto;
}

