using TransRail.Domain.Enums;

namespace TransRail.Presentation.Views;

public interface IBoardingView
{
    string CodigoPasajero { get; }
    event EventHandler? EnqueueRequested;
    event EventHandler? CallNextRequested;
    event EventHandler? RefreshQueueRequested;
    event EventHandler? ClearQueueRequested;

    void BindQueue(IReadOnlyCollection<BoardingQueueItemVm> queueItems);
    void ShowNextPassenger(string pasajero, PrioridadAbordaje prioridad);
    void ShowMessage(string message);
}

public sealed record BoardingQueueItemVm(string CodigoPasajero, string Nombre, string Categoria, string Prioridad);
