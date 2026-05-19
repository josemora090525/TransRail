using System.Collections;

namespace TransRail.Domain.Structures;

public sealed class ListaDoblementeEnlazada<T> : IEnumerable<T>
{
    private sealed class Nodo
    {
        public Nodo(T valor)
        {
            Valor = valor;
        }

        public T Valor { get; }
        public Nodo? Siguiente { get; set; }
        public Nodo? Anterior { get; set; }
    }

    private Nodo? _cabeza;
    private Nodo? _cola;
    private int _count;

    public int Count => _count;

    public void AddLast(T item)
    {
        var nuevo = new Nodo(item);
        if (_cabeza is null)
        {
            _cabeza = nuevo;
            _cola = nuevo;
        }
        else
        {
            _cola!.Siguiente = nuevo;
            nuevo.Anterior = _cola;
            _cola = nuevo;
        }

        _count++;
    }

    public void AddFirst(T item)
    {
        var nuevo = new Nodo(item);
        if (_cabeza is null)
        {
            _cabeza = nuevo;
            _cola = nuevo;
        }
        else
        {
            nuevo.Siguiente = _cabeza;
            _cabeza.Anterior = nuevo;
            _cabeza = nuevo;
        }

        _count++;
    }

    public T? Find(Func<T, bool> predicate)
    {
        var actual = _cabeza;
        while (actual is not null)
        {
            if (predicate(actual.Valor))
            {
                return actual.Valor;
            }

            actual = actual.Siguiente;
        }

        return default;
    }

    public bool Remove(Func<T, bool> predicate)
    {
        var actual = _cabeza;
        while (actual is not null)
        {
            if (predicate(actual.Valor))
            {
                if (actual.Anterior is not null)
                {
                    actual.Anterior.Siguiente = actual.Siguiente;
                }
                else
                {
                    _cabeza = actual.Siguiente;
                }

                if (actual.Siguiente is not null)
                {
                    actual.Siguiente.Anterior = actual.Anterior;
                }
                else
                {
                    _cola = actual.Anterior;
                }

                _count--;
                return true;
            }

            actual = actual.Siguiente;
        }

        return false;
    }

    public IEnumerable<T> EnumerarReversa()
    {
        var actual = _cola;
        while (actual is not null)
        {
            yield return actual.Valor;
            actual = actual.Anterior;
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        var actual = _cabeza;
        while (actual is not null)
        {
            yield return actual.Valor;
            actual = actual.Siguiente;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

