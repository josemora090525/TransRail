using TransRail.Domain.Entities;
using TransRail.Domain.Enums;
using TransRail.Domain.Rules;

namespace TransRail.Tests.Domain;

public sealed class ValidadorAbordajeTests
{
    [Fact]
    public void ObtenerPrioridad_PrioritizesAdultoMayor_OverCategoria()
    {
        var pasajero = new Pasajero
        {
            Categoria = CategoriaPasajero.Premium,
            EsAdultoMayor = true
        };

        var prioridad = ValidadorAbordaje.ObtenerPrioridad(pasajero);

        Assert.Equal(PrioridadAbordaje.AdultoMayor, prioridad);
    }

    [Fact]
    public void ObtenerPrioridad_ReturnsDiscapacidad_WhenFlagIsPresent()
    {
        var pasajero = new Pasajero
        {
            Categoria = CategoriaPasajero.Ejecutivo,
            TieneDiscapacidad = true
        };

        var prioridad = ValidadorAbordaje.ObtenerPrioridad(pasajero);

        Assert.Equal(PrioridadAbordaje.Discapacidad, prioridad);
    }

    [Theory]
    [InlineData(CategoriaPasajero.Premium, PrioridadAbordaje.Premium)]
    [InlineData(CategoriaPasajero.Ejecutivo, PrioridadAbordaje.Ejecutivo)]
    [InlineData(CategoriaPasajero.Estandar, PrioridadAbordaje.Estandar)]
    public void ObtenerPrioridad_UsesPassengerCategory_WhenThereAreNoFlags(
        CategoriaPasajero categoria,
        PrioridadAbordaje expected)
    {
        var pasajero = new Pasajero
        {
            Categoria = categoria
        };

        var prioridad = ValidadorAbordaje.ObtenerPrioridad(pasajero);

        Assert.Equal(expected, prioridad);
    }
}
