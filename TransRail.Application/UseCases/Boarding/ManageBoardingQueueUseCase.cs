using TransRail.Application.Services;
using TransRail.Domain.Entities;
using TransRail.Domain.Enums;

namespace TransRail.Application.UseCases.Boarding;

public sealed class ManageBoardingQueueUseCase
{
    private readonly AbordajeService _abordajeService;

    public ManageBoardingQueueUseCase(AbordajeService abordajeService)
    {
        _abordajeService = abordajeService;
    }

    public void EnqueuePassenger(Pasajero pasajero)
    {
        _abordajeService.Encolar(pasajero);
    }

    public Pasajero CallNextPassenger()
    {
        return _abordajeService.LlamarSiguiente();
    }

    public IReadOnlyCollection<(Pasajero Pasajero, PrioridadAbordaje Prioridad)> GetQueue()
    {
        return _abordajeService.GetColaOrdenada();
    }

    public void ClearQueue()
    {
        _abordajeService.LimpiarCola();
    }
}
