namespace TransRail.Domain.Structures;

public sealed class Pila<T>
{
    private readonly List<T> _items = new();

    public int Count => _items.Count;

    public void Push(T item)
    {
        _items.Add(item);
    }

    public T Pop()
    {
        if (_items.Count == 0)
        {
            throw new InvalidOperationException("La pila esta vacia.");
        }

        var index = _items.Count - 1;
        var value = _items[index];
        _items.RemoveAt(index);
        return value;
    }

    public T Peek()
    {
        if (_items.Count == 0)
        {
            throw new InvalidOperationException("La pila esta vacia.");
        }

        return _items[^1];
    }

    public IReadOnlyCollection<T> ToReadOnly()
    {
        return _items.ToArray();
    }
}

