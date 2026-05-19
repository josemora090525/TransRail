using TransRail.Domain.Entities;
using TransRail.Domain.Enums;
using TransRail.Domain.Rules;
using TransRail.Domain.Structures;

namespace TransRail.Application.Services;

public sealed class AbordajeService
{
    private readonly ColaPrioridad<Pasajero> _cola = new();
    private readonly List<(Pasajero Pasajero, PrioridadAbordaje Prioridad)> _enCola = new();

    public int TotalEnCola => _cola.Count;

    public void Encolar(Pasajero pasajero)
    {
        var prioridadEnum = ValidadorAbordaje.ObtenerPrioridad(pasajero);
        var prioridad = (int)prioridadEnum;
        _cola.Enqueue(pasajero, prioridad);
        _enCola.Add((pasajero, prioridadEnum));
    }

    public Pasajero LlamarSiguiente()
    {
        var siguiente = _cola.Dequeue();
        var index = _enCola.FindIndex(x => x.Pasajero.CodigoUsuario.Equals(siguiente.CodigoUsuario, StringComparison.OrdinalIgnoreCase));
        if (index >= 0)
        {
            _enCola.RemoveAt(index);
        }

        return siguiente;
    }

    public IReadOnlyCollection<(Pasajero Pasajero, PrioridadAbordaje Prioridad)> GetColaOrdenada()
    {
        return _enCola
            .OrderBy(x => (int)x.Prioridad)
            .ThenBy(x => x.Pasajero.NombreCompleto)
            .ToArray();
    }

    public void LimpiarCola()
    {
        _enCola.Clear();
        while (_cola.Count > 0)
        {
            _cola.Dequeue();
        }
    }
}
