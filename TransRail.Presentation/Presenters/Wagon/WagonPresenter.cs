using TransRail.Application.UseCases.Wagon;
using TransRail.Domain.Entities;
using TransRail.Presentation.Views;

namespace TransRail.Presentation.Presenters;

public sealed class WagonPresenter
{
    private readonly IWagonView _view;
    private readonly ManageWagonUseCase _useCase;

    public WagonPresenter(IWagonView view, ManageWagonUseCase useCase)
    {
        _view = view;
        _useCase = useCase;
        _view.SaveRequested += OnSaveRequested;
        _view.DeleteRequested += OnDeleteRequested;
        _view.SearchRequested += OnSearchRequested;
        _view.FilterByTrainRequested += OnFilterByTrainRequested;
        _view.RefreshRequested += OnRefreshRequested;
    }

    private async void OnSaveRequested(object? sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_view.CodigoVagon) || string.IsNullOrWhiteSpace(_view.CodigoTren))
            {
                _view.ShowMessage("Debes completar el código del vagón y el código del tren.");
                return;
            }

            var vagon = new Vagon
            {
                CodigoVagon = _view.CodigoVagon,
                CodigoTren = _view.CodigoTren,
                Tipo = _view.TipoVagon,
                Capacidad = _view.Capacidad,
                PesoMaximoKg = _view.PesoMaximoKg
            };

            await _useCase.UpsertAsync(vagon);
            _view.ShowMessage("Vagón guardado correctamente.");
            await RefrescarAsync();
        }
        catch (Exception ex)
        {
            _view.ShowMessage($"No se pudo guardar el vagón: {ex.Message}");
        }
    }

    private async void OnDeleteRequested(object? sender, EventArgs e)
    {
        var codigo = ResolveSearchCode();
        if (string.IsNullOrWhiteSpace(codigo))
        {
            _view.ShowMessage("Debes indicar un código para eliminar.");
            return;
        }

        await _useCase.DeleteAsync(codigo);
        _view.ShowMessage("Vagón eliminado.");
        await RefrescarAsync();
    }

    private async void OnSearchRequested(object? sender, EventArgs e)
    {
        var codigo = ResolveSearchCode();
        if (string.IsNullOrWhiteSpace(codigo))
        {
            _view.ShowMessage("Debes indicar un código para buscar.");
            return;
        }

        var vagon = await _useCase.GetByCodigoAsync(codigo);
        if (vagon is null)
        {
            _view.ShowMessage("Vagón no encontrado.");
            return;
        }

        _view.FillForm(vagon);
    }

    private async void OnFilterByTrainRequested(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_view.CodigoTrenFiltro))
        {
            await RefrescarAsync();
            return;
        }

        var vagones = await _useCase.GetByCodigoTrenAsync(_view.CodigoTrenFiltro);
        _view.BindVagones(vagones.OrderBy(x => x.CodigoVagon).ToArray());
    }

    private async void OnRefreshRequested(object? sender, EventArgs e)
    {
        await RefrescarAsync();
    }

    public async Task RefrescarAsync()
    {
        var vagones = await _useCase.GetAllAsync();
        _view.BindVagones(vagones.OrderBy(x => x.CodigoVagon).ToArray());
    }

    private string ResolveSearchCode()
    {
        if (!string.IsNullOrWhiteSpace(_view.CodigoBusqueda))
        {
            return _view.CodigoBusqueda;
        }

        return _view.CodigoVagon;
    }
}
