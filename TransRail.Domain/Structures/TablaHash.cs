using System.Collections;

namespace TransRail.Domain.Structures;

public sealed class TablaHash<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    where TKey : notnull
{
    private enum EstadoSlot
    {
        Vacio = 0,
        Ocupado = 1,
        Eliminado = 2
    }

    private struct Slot
    {
        public TKey? Clave;
        public TValue? Valor;
        public EstadoSlot Estado;
    }

    private Slot[] _slots;
    private int _count;

    public TablaHash(int capacidadInicial = 101)
    {
        if (capacidadInicial < 5)
        {
            capacidadInicial = 5;
        }

        _slots = new Slot[capacidadInicial];
    }

    public int Count => _count;
    public int Capacity => _slots.Length;

    public bool ContainsKey(TKey clave)
    {
        return TryGetValue(clave, out _);
    }

    public bool TryGetValue(TKey clave, out TValue? valor)
    {
        var index = BuscarIndice(clave, out var encontrado);
        if (!encontrado)
        {
            valor = default;
            return false;
        }

        valor = _slots[index].Valor;
        return true;
    }

    public void AddOrUpdate(TKey clave, TValue valor)
    {
        EnsureCapacityForInsert();
        var index = BuscarIndiceParaInsercion(clave, out var existe);
        if (existe)
        {
            _slots[index].Valor = valor;
            return;
        }

        _slots[index].Clave = clave;
        _slots[index].Valor = valor;
        _slots[index].Estado = EstadoSlot.Ocupado;
        _count++;
    }

    public bool Remove(TKey clave)
    {
        var index = BuscarIndice(clave, out var encontrado);
        if (!encontrado)
        {
            return false;
        }

        _slots[index].Clave = default;
        _slots[index].Valor = default;
        _slots[index].Estado = EstadoSlot.Eliminado;
        _count--;
        return true;
    }

    public IReadOnlyCollection<TKey> Keys()
    {
        return this.Select(x => x.Key).ToArray();
    }

    public IReadOnlyCollection<TValue> Values()
    {
        return this.Select(x => x.Value).ToArray();
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        foreach (var slot in _slots)
        {
            if (slot.Estado == EstadoSlot.Ocupado && slot.Clave is not null && slot.Valor is not null)
            {
                yield return new KeyValuePair<TKey, TValue>(slot.Clave, slot.Valor);
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private void EnsureCapacityForInsert()
    {
        var loadFactor = (double)(_count + 1) / _slots.Length;
        if (loadFactor < 0.70d)
        {
            return;
        }

        Rehash(_slots.Length * 2 + 1);
    }

    private int BuscarIndiceParaInsercion(TKey clave, out bool existe)
    {
        existe = false;
        var hash = Math.Abs(clave.GetHashCode());
        var index = hash % _slots.Length;
        var firstDeleted = -1;

        for (var i = 0; i < _slots.Length; i++)
        {
            ref var slot = ref _slots[index];
            if (slot.Estado == EstadoSlot.Vacio)
            {
                return firstDeleted >= 0 ? firstDeleted : index;
            }

            if (slot.Estado == EstadoSlot.Eliminado)
            {
                if (firstDeleted < 0)
                {
                    firstDeleted = index;
                }
            }
            else if (slot.Clave is not null && slot.Clave.Equals(clave))
            {
                existe = true;
                return index;
            }

            index = (index + 1) % _slots.Length;
        }

        return firstDeleted >= 0 ? firstDeleted : 0;
    }

    private int BuscarIndice(TKey clave, out bool encontrado)
    {
        var hash = Math.Abs(clave.GetHashCode());
        var index = hash % _slots.Length;

        for (var i = 0; i < _slots.Length; i++)
        {
            ref var slot = ref _slots[index];
            if (slot.Estado == EstadoSlot.Vacio)
            {
                encontrado = false;
                return index;
            }

            if (slot.Estado == EstadoSlot.Ocupado && slot.Clave is not null && slot.Clave.Equals(clave))
            {
                encontrado = true;
                return index;
            }

            index = (index + 1) % _slots.Length;
        }

        encontrado = false;
        return index;
    }

    private void Rehash(int nuevaCapacidad)
    {
        var antiguos = this.ToArray();
        _slots = new Slot[nuevaCapacidad];
        _count = 0;

        foreach (var par in antiguos)
        {
            AddOrUpdate(par.Key, par.Value);
        }
    }
}

