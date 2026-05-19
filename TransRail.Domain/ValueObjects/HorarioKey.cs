namespace TransRail.Domain.ValueObjects;

public readonly record struct HorarioKey(DateOnly Fecha, TimeOnly HoraSalida, string CodigoTren)
    : IComparable<HorarioKey>
{
    public int CompareTo(HorarioKey other)
    {
        var byDate = Fecha.CompareTo(other.Fecha);
        if (byDate != 0)
        {
            return byDate;
        }

        var byTime = HoraSalida.CompareTo(other.HoraSalida);
        if (byTime != 0)
        {
            return byTime;
        }

        return string.Compare(CodigoTren, other.CodigoTren, StringComparison.OrdinalIgnoreCase);
    }

    public override string ToString()
    {
        return $"{Fecha:yyyy-MM-dd} {HoraSalida:HH\\:mm} {CodigoTren}";
    }
}

