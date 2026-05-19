using TransRail.Application.Interfaces;
using TransRail.Domain.Entities;

namespace TransRail.Application.Services;

public sealed class PagoService
{
    private readonly IPagoRepository _repository;

    public PagoService(IPagoRepository repository)
    {
        _repository = repository;
    }

    public Task RegistrarPagoAsync(Pago pago, CancellationToken cancellationToken = default)
    {
        return _repository.UpsertAsync(pago, cancellationToken);
    }

    public Task<IReadOnlyCollection<Pago>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _repository.GetAllAsync(cancellationToken);
    }

    public Task<IReadOnlyCollection<Pago>> GetByCodigoBoletoAsync(string codigoBoleto, CancellationToken cancellationToken = default)
    {
        return _repository.GetByCodigoBoletoAsync(codigoBoleto, cancellationToken);
    }
}
