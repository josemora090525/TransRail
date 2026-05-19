using TransRail.Application.Services;
using TransRail.Domain.Entities;

namespace TransRail.Application.UseCases.Payment;

public sealed class RegisterPaymentUseCase
{
    private readonly PagoService _pagoService;

    public RegisterPaymentUseCase(PagoService pagoService)
    {
        _pagoService = pagoService;
    }

    public Task ExecuteAsync(Pago pago, CancellationToken cancellationToken = default)
    {
        return _pagoService.RegistrarPagoAsync(pago, cancellationToken);
    }

    public Task<IReadOnlyCollection<Pago>> GetByCodigoBoletoAsync(string codigoBoleto, CancellationToken cancellationToken = default)
    {
        return _pagoService.GetByCodigoBoletoAsync(codigoBoleto, cancellationToken);
    }
}
