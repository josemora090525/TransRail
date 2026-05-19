using TransRail.Application.Services;
using TransRail.Domain.Entities;
using TransRail.Domain.Enums;

namespace TransRail.Tests.Application;

public sealed class AbordajeServiceTests
{
    [Fact]
    public void LlamarSiguiente_RespetaPrioridad()
    {
        var service = new AbordajeService();
        service.Encolar(new Pasajero
        {
            CodigoUsuario = "PAS-STD",
            NombreCompleto = "Estandar",
            Categoria = CategoriaPasajero.Estandar
        });
        service.Encolar(new Pasajero
        {
            CodigoUsuario = "PAS-PREM",
            NombreCompleto = "Premium",
            Categoria = CategoriaPasajero.Premium
        });
        service.Encolar(new Pasajero
        {
            CodigoUsuario = "PAS-ADU",
            NombreCompleto = "Adulto",
            EsAdultoMayor = true
        });

        var next = service.LlamarSiguiente();

        Assert.Equal("PAS-ADU", next.CodigoUsuario);
    }
}
