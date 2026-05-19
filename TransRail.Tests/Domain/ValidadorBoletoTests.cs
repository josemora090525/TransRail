using TransRail.Domain.Entities;
using TransRail.Domain.Rules;

namespace TransRail.Tests.Domain;

public sealed class ValidadorBoletoTests
{
    [Fact]
    public void Validar_ReturnsError_WhenCodigoIsMissing()
    {
        var boleto = new Boleto
        {
            CodigoPasajero = "PAS-001",
            CodigoHorario = "HOR-001",
            Precio = 100
        };

        var result = ValidadorBoleto.Validar(boleto);

        Assert.False(result.EsValido);
        Assert.Equal("El codigo de boleto es obligatorio.", result.Error);
    }

    [Fact]
    public void Validar_ReturnsError_WhenPrecioIsZero()
    {
        var boleto = new Boleto
        {
            CodigoBoleto = "BOL-001",
            CodigoPasajero = "PAS-001",
            CodigoHorario = "HOR-001",
            Precio = 0
        };

        var result = ValidadorBoleto.Validar(boleto);

        Assert.False(result.EsValido);
        Assert.Equal("El precio del boleto debe ser mayor que cero.", result.Error);
    }

    [Fact]
    public void Validar_ReturnsValid_ForCompleteTicket()
    {
        var boleto = new Boleto
        {
            CodigoBoleto = "BOL-001",
            CodigoPasajero = "PAS-001",
            CodigoHorario = "HOR-001",
            Precio = 35.5m
        };

        var result = ValidadorBoleto.Validar(boleto);

        Assert.True(result.EsValido);
        Assert.Equal(string.Empty, result.Error);
    }
}
