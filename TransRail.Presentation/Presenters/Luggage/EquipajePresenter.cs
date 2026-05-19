using TransRail.Application.UseCases.Luggage;
using TransRail.Domain.Entities;
using TransRail.Presentation.Views;

namespace TransRail.Presentation.Presenters;

public sealed class EquipajePresenter
{
    private readonly IEquipajeView _view;
    private readonly LuggageOperationsUseCase _useCase;

    public EquipajePresenter(IEquipajeView view, LuggageOperationsUseCase useCase)
    {
        _view = view;
        _useCase = useCase;

        _view.SaveRequested += OnSaveRequested;
        _view.DeleteRequested += OnDeleteRequested;
        _view.RefreshRequested += OnRefreshRequested;
        _view.SearchRequested += OnSearchRequested;
        _view.FilterByVagonRequested += OnFilterByVagonRequested;
        _view.BuildStackRequested += OnBuildStackRequested;
    }

    private async void OnSaveRequested(object? sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_view.CodigoEquipaje) ||
                string.IsNullOrWhiteSpace(_view.CodigoBoleto) ||
                string.IsNullOrWhiteSpace(_view.CodigoVagonCarga))
            {
                _view.ShowMessage("Los códigos de equipaje, boleto y vagón son obligatorios.");
                return;
            }

            var equipaje = new Equipaje
            {
                CodigoEquipaje = _view.CodigoEquipaje,
                CodigoBoleto = _view.CodigoBoleto,
                CodigoVagonCarga = _view.CodigoVagonCarga,
                PesoKg = _view.PesoKg,
                Descripcion = _view.Descripcion
            };

            await _useCase.UpsertAsync(equipaje);
            _view.ShowMessage("Equipaje guardado.");
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            _view.ShowMessage($"No se pudo guardar el equipaje: {ex.Message}");
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
        _view.ShowMessage("Equipaje eliminado.");
        await RefreshAsync();
    }

    private async void OnRefreshRequested(object? sender, EventArgs e)
    {
        await RefreshAsync();
    }

    private async void OnSearchRequested(object? sender, EventArgs e)
    {
        var codigo = ResolveSearchCode();
        if (string.IsNullOrWhiteSpace(codigo))
        {
            _view.ShowMessage("Debes indicar un código para buscar.");
            return;
        }

        var all = await _useCase.GetAllAsync();
        var equipaje = all.FirstOrDefault(x => x.CodigoEquipaje.Equals(codigo, StringComparison.OrdinalIgnoreCase));
        if (equipaje is null)
        {
            _view.ShowMessage("Equipaje no encontrado.");
            return;
        }

        _view.FillForm(equipaje);
    }

    private async void OnFilterByVagonRequested(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_view.CodigoVagonFiltro))
        {
            await RefreshAsync();
            return;
        }

        var equipajes = await _useCase.GetByCodigoVagonAsync(_view.CodigoVagonFiltro);
        _view.BindEquipajes(equipajes.OrderBy(x => x.CodigoEquipaje).ToArray());
    }

    private async void OnBuildStackRequested(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_view.CodigoVagonFiltro))
        {
            _view.ShowMessage("Debes indicar el código del vagón para construir la pila.");
            return;
        }

        var pila = await _useCase.BuildStackByVagonAsync(_view.CodigoVagonFiltro);
        var items = pila.ToReadOnly().ToArray();
        var topCode = items.Length > 0 ? items[^1].CodigoEquipaje : "N/A";
        _view.ShowStackInfo($"Vagón: {_view.CodigoVagonFiltro} | Elementos: {items.Length} | Tope LIFO: {topCode}");
    }

    public async Task RefreshAsync()
    {
        var equipajes = await _useCase.GetAllAsync();
        _view.BindEquipajes(equipajes.OrderBy(x => x.CodigoEquipaje).ToArray());
    }

    private string ResolveSearchCode()
    {
        if (!string.IsNullOrWhiteSpace(_view.CodigoBusqueda))
        {
            return _view.CodigoBusqueda;
        }

        return _view.CodigoEquipaje;
    }
}
