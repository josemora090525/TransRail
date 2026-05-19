namespace TransRail.Domain.Structures;

public sealed class ColaPrioridad<T>
{
    private readonly List<(int Prioridad, long Secuencia, T Valor)> _heap = new();
    private long _secuencia;

    public int Count => _heap.Count;

    public void Enqueue(T valor, int prioridad)
    {
        _heap.Add((prioridad, _secuencia++, valor));
        HeapifyUp(_heap.Count - 1);
    }

    public T Dequeue()
    {
        if (_heap.Count == 0)
        {
            throw new InvalidOperationException("La cola de prioridad esta vacia.");
        }

        var root = _heap[0].Valor;
        var last = _heap[^1];
        _heap.RemoveAt(_heap.Count - 1);
        if (_heap.Count > 0)
        {
            _heap[0] = last;
            HeapifyDown(0);
        }

        return root;
    }

    public T Peek()
    {
        if (_heap.Count == 0)
        {
            throw new InvalidOperationException("La cola de prioridad esta vacia.");
        }

        return _heap[0].Valor;
    }

    private static bool EsMenor((int Prioridad, long Secuencia, T Valor) a, (int Prioridad, long Secuencia, T Valor) b)
    {
        if (a.Prioridad != b.Prioridad)
        {
            return a.Prioridad < b.Prioridad;
        }

        return a.Secuencia < b.Secuencia;
    }

    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            var parent = (index - 1) / 2;
            if (!EsMenor(_heap[index], _heap[parent]))
            {
                break;
            }

            (_heap[index], _heap[parent]) = (_heap[parent], _heap[index]);
            index = parent;
        }
    }

    private void HeapifyDown(int index)
    {
        while (true)
        {
            var left = 2 * index + 1;
            var right = 2 * index + 2;
            var smallest = index;

            if (left < _heap.Count && EsMenor(_heap[left], _heap[smallest]))
            {
                smallest = left;
            }

            if (right < _heap.Count && EsMenor(_heap[right], _heap[smallest]))
            {
                smallest = right;
            }

            if (smallest == index)
            {
                break;
            }

            (_heap[index], _heap[smallest]) = (_heap[smallest], _heap[index]);
            index = smallest;
        }
    }
}

