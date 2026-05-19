using TransRail.Domain.Entities;

namespace TransRail.Domain.Rules;

public static class ValidadorBoleto
{
    public static (bool EsValido, string Error) Validar(Boleto boleto)
    {
        if (string.IsNullOrWhiteSpace(boleto.CodigoBoleto))
        {
            return (false, "El codigo de boleto es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(boleto.CodigoPasajero))
        {
            return (false, "El codigo del pasajero es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(boleto.CodigoHorario))
        {
            return (false, "El codigo del horario es obligatorio.");
        }

        if (boleto.Precio <= 0)
        {
            return (false, "El precio del boleto debe ser mayor que cero.");
        }

        return (true, string.Empty);
    }
}

