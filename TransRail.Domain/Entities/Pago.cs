using TransRail.Domain.Enums;

namespace TransRail.Domain.Entities;

public sealed class Pago : IConCodigoOperativo
{
    public Guid IdInterno { get; set; } = Guid.NewGuid();
    public string CodigoPago { get; set; } = string.Empty;
    public string CodigoBoleto { get; set; } = string.Empty;
    public MetodoPago Metodo { get; set; } = MetodoPago.TarjetaDebito;
    public decimal Valor { get; set; }
    public DateTime FechaPagoUtc { get; set; } = DateTime.UtcNow;
    public bool Confirmado { get; set; }

    public string Codigo => CodigoPago;
}

