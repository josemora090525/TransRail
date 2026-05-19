using TransRail.Domain.Structures;

namespace TransRail.Tests.Structures;

public sealed class GrafoTests
{
    [Fact]
    public void Dijkstra_Returns_MinDistance()
    {
        var grafo = new Grafo<string>();
        grafo.AddAristaNoDirigida("A", "B", 5);
        grafo.AddAristaNoDirigida("B", "C", 4);
        grafo.AddAristaNoDirigida("A", "C", 15);

        var distancias = grafo.CalcularDistanciasMinimas("A");

        Assert.Equal(0, distancias["A"]);
        Assert.Equal(5, distancias["B"]);
        Assert.Equal(9, distancias["C"]);
    }
}

