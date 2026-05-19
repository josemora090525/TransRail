using TransRail.Domain.Entities;
using TransRail.Domain.Enums;

namespace TransRail.Domain.Rules;

public static class ValidadorAbordaje
{
    public static PrioridadAbordaje ObtenerPrioridad(Pasajero pasajero)
    {
        if (pasajero.EsAdultoMayor)
        {
            return PrioridadAbordaje.AdultoMayor;
        }

        if (pasajero.TieneDiscapacidad)
        {
            return PrioridadAbordaje.Discapacidad;
        }

        return pasajero.Categoria switch
        {
            CategoriaPasajero.Premium => PrioridadAbordaje.Premium,
            CategoriaPasajero.Ejecutivo => PrioridadAbordaje.Ejecutivo,
            _ => PrioridadAbordaje.Estandar
        };
    }
}

