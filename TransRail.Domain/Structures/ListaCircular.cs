using System.Collections;

namespace TransRail.Domain.Structures;

public sealed class ListaCircular<T> : IEnumerable<T>
{
    private sealed class Nodo
    {
        public Nodo(T valor)
        {
            Valor = valor;
        }

        public T Valor { get; }
        public Nodo? Siguiente { get; set; }
    }

    private Nodo? _cabeza;
    private Nodo? _cola;
    private int _count;

    public int Count => _count;

    public void Add(T item)
    {
        var nuevo = new Nodo(item);
        if (_cabeza is null)
        {
            _cabeza = nuevo;
            _cola = nuevo;
            nuevo.Siguiente = nuevo;
        }
        else
        {
            nuevo.Siguiente = _cabeza;
            _cola!.Siguiente = nuevo;
            _cola = nuevo;
        }

        _count++;
    }

    public T? Find(Func<T, bool> predicate)
    {
        if (_cabeza is null)
        {
            return default;
        }

        var actual = _cabeza;
        do
        {
            if (predicate(actual.Valor))
            {
                return actual.Valor;
            }

            actual = actual.Siguiente!;
        } while (actual != _cabeza);

        return default;
    }

    public bool Remove(Func<T, bool> predicate)
    {
        if (_cabeza is null)
        {
            return false;
        }

        var actual = _cabeza;
        Nodo? anterior = _cola;

        do
        {
            if (predicate(actual.Valor))
            {
                if (_count == 1)
                {
                    _cabeza = null;
                    _cola = null;
                }
                else
                {
                    anterior!.Siguiente = actual.Siguiente;
                    if (actual == _cabeza)
                    {
                        _cabeza = actual.Siguiente;
                    }

                    if (actual == _cola)
                    {
                        _cola = anterior;
                    }
                }

                _count--;
                return true;
            }

            anterior = actual;
            actual = actual.Siguiente!;
        } while (actual != _cabeza);

        return false;
    }

    public IEnumerator<T> GetEnumerator()
    {
        if (_cabeza is null)
        {
            yield break;
        }

        var actual = _cabeza;
        do
        {
            yield return actual.Valor;
            actual = actual.Siguiente!;
        } while (actual != _cabeza);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

