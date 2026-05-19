namespace TransRail.Domain.Structures;

public sealed class Grafo<TNodo> where TNodo : notnull
{
    private readonly Dictionary<TNodo, List<(TNodo Destino, int Peso)>> _adyacencia = new();

    public IReadOnlyCollection<TNodo> Nodos => _adyacencia.Keys.ToArray();

    public void AddNodo(TNodo nodo)
    {
        if (!_adyacencia.ContainsKey(nodo))
        {
            _adyacencia[nodo] = new List<(TNodo Destino, int Peso)>();
        }
    }

    public bool ContainsNodo(TNodo nodo)
    {
        return _adyacencia.ContainsKey(nodo);
    }

    public void AddAristaDirigida(TNodo origen, TNodo destino, int peso)
    {
        if (peso < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(peso), "El peso no puede ser negativo.");
        }

        AddNodo(origen);
        AddNodo(destino);

        var lista = _adyacencia[origen];
        var index = lista.FindIndex(x => EqualityComparer<TNodo>.Default.Equals(x.Destino, destino));
        if (index >= 0)
        {
            lista[index] = (destino, peso);
        }
        else
        {
            lista.Add((destino, peso));
        }
    }

    public void AddAristaNoDirigida(TNodo nodoA, TNodo nodoB, int peso)
    {
        AddAristaDirigida(nodoA, nodoB, peso);
        AddAristaDirigida(nodoB, nodoA, peso);
    }

    public IReadOnlyCollection<(TNodo Destino, int Peso)> GetVecinos(TNodo nodo)
    {
        if (!_adyacencia.TryGetValue(nodo, out var vecinos))
        {
            return Array.Empty<(TNodo Destino, int Peso)>();
        }

        return vecinos.ToArray();
    }

    public IReadOnlyDictionary<TNodo, int> CalcularDistanciasMinimas(TNodo origen)
    {
        if (!_adyacencia.ContainsKey(origen))
        {
            throw new InvalidOperationException("El nodo origen no existe en el grafo.");
        }

        var distancias = _adyacencia.Keys.ToDictionary(x => x, _ => int.MaxValue);
        var pq = new PriorityQueue<TNodo, int>();

        distancias[origen] = 0;
        pq.Enqueue(origen, 0);

        while (pq.Count > 0)
        {
            var actual = pq.Dequeue();
            var distanciaActual = distancias[actual];

            foreach (var (destino, peso) in _adyacencia[actual])
            {
                if (distanciaActual == int.MaxValue)
                {
                    continue;
                }

                var alternativa = distanciaActual + peso;
                if (alternativa < distancias[destino])
                {
                    distancias[destino] = alternativa;
                    pq.Enqueue(destino, alternativa);
                }
            }
        }

        return distancias;
    }

    public IReadOnlyList<TNodo> CalcularRutaMasCorta(TNodo origen, TNodo destino)
    {
        if (!_adyacencia.ContainsKey(origen) || !_adyacencia.ContainsKey(destino))
        {
            return Array.Empty<TNodo>();
        }

        var distancias = _adyacencia.Keys.ToDictionary(x => x, _ => int.MaxValue);
        var anteriores = new Dictionary<TNodo, TNodo>();
        var pq = new PriorityQueue<TNodo, int>();

        distancias[origen] = 0;
        pq.Enqueue(origen, 0);

        while (pq.Count > 0)
        {
            var actual = pq.Dequeue();
            if (EqualityComparer<TNodo>.Default.Equals(actual, destino))
            {
                break;
            }

            foreach (var (vecino, peso) in _adyacencia[actual])
            {
                if (distancias[actual] == int.MaxValue)
                {
                    continue;
                }

                var alternativa = distancias[actual] + peso;
                if (alternativa < distancias[vecino])
                {
                    distancias[vecino] = alternativa;
                    anteriores[vecino] = actual;
                    pq.Enqueue(vecino, alternativa);
                }
            }
        }

        if (distancias[destino] == int.MaxValue)
        {
            return Array.Empty<TNodo>();
        }

        var ruta = new List<TNodo> { destino };
        var actualNodo = destino;
        while (!EqualityComparer<TNodo>.Default.Equals(actualNodo, origen))
        {
            if (!anteriores.TryGetValue(actualNodo, out var anterior))
            {
                return Array.Empty<TNodo>();
            }

            ruta.Add(anterior);
            actualNodo = anterior;
        }

        ruta.Reverse();
        return ruta;
    }
}

