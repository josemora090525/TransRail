using TransRail.Application.UseCases.Train;
using TransRail.Domain.Entities;
using TransRail.Presentation.Views;

namespace TransRail.Presentation.Presenters;

public sealed class TrainPresenter
{
    private readonly ITrainView _view;
    private readonly ManageTrainUseCase _useCase;

    public TrainPresenter(ITrainView view, ManageTrainUseCase useCase)
    {
        _view = view;
        _useCase = useCase;
        _view.CreateRequested += OnCreateRequested;
        _view.RefreshRequested += OnRefreshRequested;
    }

    private async void OnCreateRequested(object? sender, EventArgs e)
    {
        try
        {
            var tren = new Tren
            {
                CodigoTren = _view.CodigoTren,
                NumeroOperativo = _view.NumeroOperativo,
                Nombre = _view.NombreTren,
                CapacidadVagones = _view.CapacidadVagones,
                Kilometraje = _view.Kilometraje,
                EnCirculacion = _view.EnCirculacion
            };

            await _useCase.UpsertAsync(tren);
            _view.ShowMessage("Tren guardado correctamente.");
            await RefrescarAsync();
        }
        catch (Exception ex)
        {
            _view.ShowMessage($"No se pudo guardar el tren: {ex.Message}");
        }
    }

    private async void OnRefreshRequested(object? sender, EventArgs e)
    {
        await RefrescarAsync();
    }

    public async Task RefrescarAsync()
    {
        var trenes = await _useCase.GetAllAsync();
        _view.BindTrenes(trenes);
    }
}
