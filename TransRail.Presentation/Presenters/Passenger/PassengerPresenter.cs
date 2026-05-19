using TransRail.Application.UseCases.Passenger;
using TransRail.Domain.Entities;
using TransRail.Presentation.Views;

namespace TransRail.Presentation.Presenters;

public sealed class PassengerPresenter
{
    private readonly IPassengerView _view;
    private readonly ManagePassengerUseCase _useCase;

    public PassengerPresenter(IPassengerView view, ManagePassengerUseCase useCase)
    {
        _view = view;
        _useCase = useCase;
        _view.SaveRequested += OnSaveRequested;
        _view.DeleteRequested += OnDeleteRequested;
        _view.SearchRequested += OnSearchRequested;
        _view.RefreshRequested += OnRefreshRequested;
    }

    private async void OnSaveRequested(object? sender, EventArgs e)
    {
        try
        {
            var codigo = _view.CodigoPasajero;
            if (string.IsNullOrWhiteSpace(codigo))
            {
                _view.ShowMessage("Debes ingresar el código del pasajero.");
                return;
            }

            var pasajero = new Pasajero
            {
                CodigoUsuario = codigo,
                NombreCompleto = _view.NombreCompleto,
                NumeroDocumento = _view.NumeroDocumento,
                Correo = _view.Correo,
                Contrasena = _view.Contrasena,
                Categoria = _view.Categoria,
                EsAdultoMayor = _view.EsAdultoMayor,
                TieneDiscapacidad = _view.TieneDiscapacidad
            };

            await _useCase.UpsertAsync(pasajero);
            _view.ShowMessage("Pasajero guardado correctamente.");
            await RefrescarAsync();
        }
        catch (Exception ex)
        {
            _view.ShowMessage($"No se pudo guardar el pasajero: {ex.Message}");
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
        _view.ShowMessage("Pasajero eliminado.");
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

        var pasajero = await _useCase.GetByCodigoAsync(codigo);
        if (pasajero is null)
        {
            _view.ShowMessage("Pasajero no encontrado.");
            return;
        }

        _view.FillForm(pasajero);
    }

    private async void OnRefreshRequested(object? sender, EventArgs e)
    {
        await RefrescarAsync();
    }

    public async Task RefrescarAsync()
    {
        var pasajeros = await _useCase.GetAllAsync();
        _view.BindPasajeros(pasajeros.OrderBy(x => x.CodigoUsuario).ToArray());
    }

    private string ResolveSearchCode()
    {
        if (!string.IsNullOrWhiteSpace(_view.CodigoBusqueda))
        {
            return _view.CodigoBusqueda;
        }

        return _view.CodigoPasajero;
    }
}
