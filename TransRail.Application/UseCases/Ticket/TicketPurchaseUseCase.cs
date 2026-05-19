using TransRail.Application.Services;
using TransRail.Domain.Entities;
using TransRail.Domain.Enums;

namespace TransRail.Application.UseCases.Ticket;

public sealed class TicketPurchaseUseCase
{
    private readonly BoletoService _boletoService;

    public TicketPurchaseUseCase(BoletoService boletoService)
    {
        _boletoService = boletoService;
    }

    public Task<(bool Ok, string Error)> ExecutePurchaseAsync(Boleto boleto, CancellationToken cancellationToken = default)
    {
        return _boletoService.ComprarAsync(boleto, cancellationToken);
    }

    public decimal CalculatePrice(int distanciaKm, TipoBoleto tipoBoleto)
    {
        return _boletoService.CalcularPrecio(distanciaKm, tipoBoleto);
    }

    public Task<IReadOnlyCollection<Boleto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _boletoService.GetAllAsync(cancellationToken);
    }

    public Task<Boleto?> GetByCodigoAsync(string codigoBoleto, CancellationToken cancellationToken = default)
    {
        return _boletoService.GetByCodigoAsync(codigoBoleto, cancellationToken);
    }

    public Task DeleteAsync(string codigoBoleto, CancellationToken cancellationToken = default)
    {
        return _boletoService.DeleteAsync(codigoBoleto, cancellationToken);
    }

    public IReadOnlyCollection<Boleto> GetHistoryForward()
    {
        return _boletoService.HistorialEnOrden();
    }

    public IReadOnlyCollection<Boleto> GetHistoryReverse()
    {
        return _boletoService.HistorialReverso();
    }
}
