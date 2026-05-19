using TransRail.Application.DTOs;
using TransRail.Application.Services;

namespace TransRail.Application.UseCases.Auth;

public sealed class LoginUseCase
{
    private readonly AuthService _authService;

    public LoginUseCase(AuthService authService)
    {
        _authService = authService;
    }

    public Task<LoginResultDto> ExecuteAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        return _authService.LoginAsync(request, cancellationToken);
    }
}
