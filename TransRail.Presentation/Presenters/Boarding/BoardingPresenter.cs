using TransRail.Application.UseCases.Boarding;
using TransRail.Application.UseCases.Passenger;
using TransRail.Domain.Rules;
using TransRail.Presentation.Views;

namespace TransRail.Presentation.Presenters;

public sealed class BoardingPresenter
{
    private readonly IBoardingView _view;
    private readonly ManageBoardingQueueUseCase _boardingUseCase;
    private readonly ManagePassengerUseCase _passengerUseCase;

    public BoardingPresenter(
        IBoardingView view,
        ManageBoardingQueueUseCase boardingUseCase,
        ManagePassengerUseCase passengerUseCase)
    {
        _view = view;
        _boardingUseCase = boardingUseCase;
        _passengerUseCase = passengerUseCase;

        _view.EnqueueRequested += OnEnqueueRequested;
        _view.CallNextRequested += OnCallNextRequested;
        _view.RefreshQueueRequested += OnRefreshQueueRequested;
        _view.ClearQueueRequested += OnClearQueueRequested;
    }

    private async void OnEnqueueRequested(object? sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_view.CodigoPasajero))
            {
                _view.ShowMessage("Debes indicar el código del pasajero.");
                return;
            }

            var pasajero = await _passengerUseCase.GetByCodigoAsync(_view.CodigoPasajero);
            if (pasajero is null)
            {
                _view.ShowMessage("Pasajero no encontrado.");
                return;
            }

            _boardingUseCase.EnqueuePassenger(pasajero);
            _view.ShowMessage("Pasajero agregado a la cola de abordaje.");
            RefreshQueue();
        }
        catch (Exception ex)
        {
            _view.ShowMessage($"No se pudo encolar: {ex.Message}");
        }
    }

    private void OnCallNextRequested(object? sender, EventArgs e)
    {
        try
        {
            var next = _boardingUseCase.CallNextPassenger();
            var prioridad = ValidadorAbordaje.ObtenerPrioridad(next);
            _view.ShowNextPassenger(next.NombreCompleto, prioridad);
            RefreshQueue();
        }
        catch (InvalidOperationException)
        {
            _view.ShowMessage("No hay pasajeros en la cola.");
        }
        catch (Exception ex)
        {
            _view.ShowMessage($"No se pudo llamar al siguiente pasajero: {ex.Message}");
        }
    }

    private void OnRefreshQueueRequested(object? sender, EventArgs e)
    {
        RefreshQueue();
    }

    private void OnClearQueueRequested(object? sender, EventArgs e)
    {
        _boardingUseCase.ClearQueue();
        RefreshQueue();
        _view.ShowMessage("Cola de abordaje limpiada.");
    }

    public void RefreshQueue()
    {
        var queue = _boardingUseCase.GetQueue()
            .Select(x => new BoardingQueueItemVm(
                x.Pasajero.CodigoUsuario,
                x.Pasajero.NombreCompleto,
                x.Pasajero.Categoria.ToString(),
                x.Prioridad.ToString()))
            .ToArray();

        _view.BindQueue(queue);
    }
}
