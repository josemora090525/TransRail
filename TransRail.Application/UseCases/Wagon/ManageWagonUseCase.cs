using TransRail.Application.Services;
using TransRail.Domain.Entities;

namespace TransRail.Application.UseCases.Wagon;

public sealed class ManageWagonUseCase
{
    private readonly VagonService _vagonService;

    public ManageWagonUseCase(VagonService vagonService)
    {
        _vagonService = vagonService;
    }

    public Task<IReadOnlyCollection<Vagon>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _vagonService.GetAllAsync(cancellationToken);
    }

    public Task<IReadOnlyCollection<Vagon>> GetByCodigoTrenAsync(string codigoTren, CancellationToken cancellationToken = default)
    {
        return _vagonService.GetByCodigoTrenAsync(codigoTren, cancellationToken);
    }

    public Task<Vagon?> GetByCodigoAsync(string codigoVagon, CancellationToken cancellationToken = default)
    {
        return _vagonService.GetByCodigoAsync(codigoVagon, cancellationToken);
    }

    public Task UpsertAsync(Vagon vagon, CancellationToken cancellationToken = default)
    {
        return _vagonService.UpsertAsync(vagon, cancellationToken);
    }

    public Task DeleteAsync(string codigoVagon, CancellationToken cancellationToken = default)
    {
        return _vagonService.DeleteAsync(codigoVagon, cancellationToken);
    }
}
