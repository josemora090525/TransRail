using TransRail.Application.UseCases.Employee;
using TransRail.Domain.Entities;
using TransRail.Presentation.Views;

namespace TransRail.Presentation.Presenters;

public sealed class EmployeePresenter
{
    private readonly IEmployeeView _view;
    private readonly ManageEmployeeUseCase _useCase;

    public EmployeePresenter(IEmployeeView view, ManageEmployeeUseCase useCase)
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
            var codigo = _view.CodigoEmpleado;
            if (string.IsNullOrWhiteSpace(codigo))
            {
                _view.ShowMessage("Debes ingresar el código del empleado.");
                return;
            }

            var empleado = new Empleado
            {
                CodigoUsuario = codigo,
                NombreCompleto = _view.NombreCompleto,
                NumeroDocumento = _view.NumeroDocumento,
                Correo = _view.Correo,
                Contrasena = _view.Contrasena
            };

            await _useCase.UpsertAsync(empleado);
            _view.ShowMessage("Empleado guardado correctamente.");
            await RefrescarAsync();
        }
        catch (Exception ex)
        {
            _view.ShowMessage($"No se pudo guardar el empleado: {ex.Message}");
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
        _view.ShowMessage("Empleado eliminado.");
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

        var empleado = await _useCase.GetByCodigoAsync(codigo);
        if (empleado is null)
        {
            _view.ShowMessage("Empleado no encontrado.");
            return;
        }

        _view.FillForm(empleado);
    }

    private async void OnRefreshRequested(object? sender, EventArgs e)
    {
        await RefrescarAsync();
    }

    public async Task RefrescarAsync()
    {
        var empleados = await _useCase.GetAllAsync();
        _view.BindEmpleados(empleados.OrderBy(x => x.CodigoUsuario).ToArray());
    }

    private string ResolveSearchCode()
    {
        if (!string.IsNullOrWhiteSpace(_view.CodigoBusqueda))
        {
            return _view.CodigoBusqueda;
        }

        return _view.CodigoEmpleado;
    }
}
