namespace TransRail.Domain.Entities;

public sealed class Estacion : IConCodigoOperativo
{
    public Guid IdInterno { get; set; } = Guid.NewGuid();
    public string CodigoEstacion { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;

    public string Codigo => CodigoEstacion;
}

