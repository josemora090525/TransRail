using TransRail.Domain.Entities;
using TransRail.Domain.ValueObjects;

namespace TransRail.Tests.Domain;

public sealed class HorarioKeyTests
{
    [Fact]
    public void CompareTo_OrdersByFechaHoraYTren()
    {
        var baseKey = new HorarioKey(new DateOnly(2026, 5, 18), new TimeOnly(8, 30), "TR-002");
        var laterDate = new HorarioKey(new DateOnly(2026, 5, 19), new TimeOnly(6, 0), "TR-001");
        var laterTime = new HorarioKey(new DateOnly(2026, 5, 18), new TimeOnly(9, 0), "TR-001");
        var laterTrain = new HorarioKey(new DateOnly(2026, 5, 18), new TimeOnly(8, 30), "TR-003");

        Assert.True(baseKey.CompareTo(laterDate) < 0);
        Assert.True(baseKey.CompareTo(laterTime) < 0);
        Assert.True(baseKey.CompareTo(laterTrain) < 0);
    }

    [Fact]
    public void ToString_ReturnsCompositeKeyFormat()
    {
        var key = new HorarioKey(new DateOnly(2026, 5, 18), new TimeOnly(8, 30), "TR-001");

        Assert.Equal("2026-05-18 08:30 TR-001", key.ToString());
    }

    [Fact]
    public void HorarioCompareTo_UsesUnderlyingKey()
    {
        var early = new Horario
        {
            CodigoHorario = "HOR-001",
            CodigoTren = "TR-001",
            Fecha = new DateOnly(2026, 5, 18),
            HoraSalida = new TimeOnly(8, 30)
        };

        var late = new Horario
        {
            CodigoHorario = "HOR-002",
            CodigoTren = "TR-001",
            Fecha = new DateOnly(2026, 5, 18),
            HoraSalida = new TimeOnly(9, 15)
        };

        Assert.True(early.CompareTo(late) < 0);
    }
}
