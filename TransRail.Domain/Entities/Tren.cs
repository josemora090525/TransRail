namespace TransRail.Domain.Entities;

public sealed class Tren : IConCodigoOperativo
{
    public Guid IdInterno { get; set; } = Guid.NewGuid();
    public string CodigoTren { get; set; } = string.Empty;
    public string NumeroOperativo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public int CapacidadVagones { get; set; }
    public int Kilometraje { get; set; }
    public bool EnCirculacion { get; set; }

    public string Codigo => CodigoTren;
}

