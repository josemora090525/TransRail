namespace TransRail.Domain.Entities;

public sealed class Equipaje : IConCodigoOperativo
{
    public Guid IdInterno { get; set; } = Guid.NewGuid();
    public string CodigoEquipaje { get; set; } = string.Empty;
    public string CodigoBoleto { get; set; } = string.Empty;
    public string CodigoVagonCarga { get; set; } = string.Empty;
    public double PesoKg { get; set; }
    public string Descripcion { get; set; } = string.Empty;

    public string Codigo => CodigoEquipaje;
}
