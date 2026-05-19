using TransRail.Domain.ValueObjects;

namespace TransRail.Domain.Entities;

public sealed class Horario : IConCodigoOperativo, IComparable<Horario>
{
    public Guid IdInterno { get; set; } = Guid.NewGuid();
    public string CodigoHorario { get; set; } = string.Empty;
    public string CodigoTren { get; set; } = string.Empty;
    public string CodigoRuta { get; set; } = string.Empty;
    public DateOnly Fecha { get; set; }
    public TimeOnly HoraSalida { get; set; }
    public TimeOnly HoraLlegada { get; set; }

    public HorarioKey Key => new(Fecha, HoraSalida, CodigoTren);
    public string Codigo => CodigoHorario;

    public int CompareTo(Horario? other)
    {
        if (other is null)
        {
            return 1;
        }

        return Key.CompareTo(other.Key);
    }
}

