using TransRail.Application.DTOs;
using TransRail.Application.UseCases.Auth;
using TransRail.Domain.Enums;
using TransRail.Presentation.Views;

namespace TransRail.Presentation.Presenters;

public sealed class LoginPresenter
{
    private readonly ILoginView _view;
    private readonly LoginUseCase _loginUseCase;

    public LoginPresenter(ILoginView view, LoginUseCase loginUseCase)
    {
        _view = view;
        _loginUseCase = loginUseCase;
        _view.LoginRequested += OnLoginRequested;
    }

    private async void OnLoginRequested(object? sender, EventArgs e)
    {
        var request = new LoginRequestDto(_view.Correo, _view.Contrasena);
        var result = await _loginUseCase.ExecuteAsync(request);
        if (!result.Exitoso)
        {
            _view.ShowMessage(result.Mensaje);
            return;
        }

        if (result.Rol is not null)
        {
            AppServices.UserSession.Start(
                result.CodigoUsuario ?? string.Empty,
                result.NombreCompleto ?? string.Empty,
                result.Correo ?? string.Empty,
                result.Rol.Value);
        }

        if (result.Rol == RolUsuario.Administrador)
        {
            _view.OpenAdminMenu();
            return;
        }

        if (result.Rol == RolUsuario.Empleado)
        {
            _view.OpenEmployeeMenu();
            return;
        }

        if (result.Rol == RolUsuario.Pasajero)
        {
            _view.OpenPassengerMenu();
            return;
        }

        _view.ShowMessage("Rol no soportado.");
    }
}
