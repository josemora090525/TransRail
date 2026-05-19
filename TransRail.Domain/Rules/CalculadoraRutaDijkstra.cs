using TransRail.Domain.Structures;

namespace TransRail.Domain.Rules;

public static class CalculadoraRutaDijkstra
{
    public static int CalcularDistancia(Grafo<string> grafo, string origen, string destino)
    {
        var distancias = grafo.CalcularDistanciasMinimas(origen);
        return distancias.TryGetValue(destino, out var distancia) ? distancia : int.MaxValue;
    }

    public static IReadOnlyList<string> CalcularRuta(Grafo<string> grafo, string origen, string destino)
    {
        return grafo.CalcularRutaMasCorta(origen, destino);
    }
}

