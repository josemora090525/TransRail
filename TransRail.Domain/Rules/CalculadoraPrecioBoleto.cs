using TransRail.Domain.Enums;

namespace TransRail.Domain.Rules;

public static class CalculadoraPrecioBoleto
{
    public static decimal Calcular(int distanciaKm, TipoBoleto tipoBoleto)
    {
        if (distanciaKm < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(distanciaKm));
        }

        const decimal tarifaBasePorKm = 0.50m;
        var precioBase = distanciaKm * tarifaBasePorKm;

        return tipoBoleto switch
        {
            TipoBoleto.Premium => decimal.Round(precioBase * 1.50m, 2),
            TipoBoleto.Ejecutivo => decimal.Round(precioBase * 1.20m, 2),
            _ => decimal.Round(precioBase, 2)
        };
    }
}

