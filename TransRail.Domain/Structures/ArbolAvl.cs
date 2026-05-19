using System.Collections;

namespace TransRail.Domain.Structures;

public sealed class ArbolAvl<T> : IEnumerable<T> where T : IComparable<T>
{
    private sealed class Nodo
    {
        public Nodo(T valor)
        {
            Valor = valor;
            Altura = 1;
        }

        public T Valor { get; set; }
        public Nodo? Izquierdo { get; set; }
        public Nodo? Derecho { get; set; }
        public int Altura { get; set; }
    }

    private Nodo? _raiz;
    private int _count;

    public int Count => _count;

    public void Insert(T value)
    {
        _raiz = InsertInternal(_raiz, value);
    }

    public bool Remove(T value)
    {
        var removed = false;
        _raiz = RemoveInternal(_raiz, value, ref removed);
        if (removed)
        {
            _count--;
        }

        return removed;
    }

    public bool Contains(T value)
    {
        return TryFind(value, out _);
    }

    public bool TryFind(T value, out T found)
    {
        var actual = _raiz;
        while (actual is not null)
        {
            var cmp = value.CompareTo(actual.Valor);
            if (cmp == 0)
            {
                found = actual.Valor;
                return true;
            }

            actual = cmp < 0 ? actual.Izquierdo : actual.Derecho;
        }

        found = default!;
        return false;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return InOrder(_raiz).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private IEnumerable<T> InOrder(Nodo? nodo)
    {
        if (nodo is null)
        {
            yield break;
        }

        foreach (var value in InOrder(nodo.Izquierdo))
        {
            yield return value;
        }

        yield return nodo.Valor;

        foreach (var value in InOrder(nodo.Derecho))
        {
            yield return value;
        }
    }

    private Nodo InsertInternal(Nodo? node, T value)
    {
        if (node is null)
        {
            _count++;
            return new Nodo(value);
        }

        var cmp = value.CompareTo(node.Valor);
        if (cmp < 0)
        {
            node.Izquierdo = InsertInternal(node.Izquierdo, value);
        }
        else if (cmp > 0)
        {
            node.Derecho = InsertInternal(node.Derecho, value);
        }
        else
        {
            node.Valor = value;
            return node;
        }

        return Balance(node);
    }

    private Nodo? RemoveInternal(Nodo? node, T value, ref bool removed)
    {
        if (node is null)
        {
            return null;
        }

        var cmp = value.CompareTo(node.Valor);
        if (cmp < 0)
        {
            node.Izquierdo = RemoveInternal(node.Izquierdo, value, ref removed);
        }
        else if (cmp > 0)
        {
            node.Derecho = RemoveInternal(node.Derecho, value, ref removed);
        }
        else
        {
            removed = true;
            if (node.Izquierdo is null || node.Derecho is null)
            {
                return node.Izquierdo ?? node.Derecho;
            }

            var successor = GetMin(node.Derecho);
            node.Valor = successor.Valor;
            var tmpRemoved = false;
            node.Derecho = RemoveInternal(node.Derecho, successor.Valor, ref tmpRemoved);
        }

        return Balance(node);
    }

    private static Nodo GetMin(Nodo node)
    {
        var actual = node;
        while (actual.Izquierdo is not null)
        {
            actual = actual.Izquierdo;
        }

        return actual;
    }

    private static int Altura(Nodo? node)
    {
        return node?.Altura ?? 0;
    }

    private static void ActualizarAltura(Nodo node)
    {
        node.Altura = 1 + Math.Max(Altura(node.Izquierdo), Altura(node.Derecho));
    }

    private static int FactorBalance(Nodo node)
    {
        return Altura(node.Izquierdo) - Altura(node.Derecho);
    }

    private static Nodo Balance(Nodo node)
    {
        ActualizarAltura(node);
        var balance = FactorBalance(node);

        if (balance > 1)
        {
            if (FactorBalance(node.Izquierdo!) < 0)
            {
                node.Izquierdo = RotacionIzquierda(node.Izquierdo!);
            }

            return RotacionDerecha(node);
        }

        if (balance < -1)
        {
            if (FactorBalance(node.Derecho!) > 0)
            {
                node.Derecho = RotacionDerecha(node.Derecho!);
            }

            return RotacionIzquierda(node);
        }

        return node;
    }

    private static Nodo RotacionDerecha(Nodo y)
    {
        var x = y.Izquierdo!;
        var t2 = x.Derecho;

        x.Derecho = y;
        y.Izquierdo = t2;

        ActualizarAltura(y);
        ActualizarAltura(x);
        return x;
    }

    private static Nodo RotacionIzquierda(Nodo x)
    {
        var y = x.Derecho!;
        var t2 = y.Izquierdo;

        y.Izquierdo = x;
        x.Derecho = t2;

        ActualizarAltura(x);
        ActualizarAltura(y);
        return y;
    }
}

