using TransRail.Application.Services;
using TransRail.Domain.Entities;

namespace TransRail.Application.UseCases.Train;

public sealed class ManageTrainUseCase
{
    private readonly TrenService _trenService;

    public ManageTrainUseCase(TrenService trenService)
    {
        _trenService = trenService;
    }

    public Task<IReadOnlyCollection<Tren>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _trenService.GetAllAsync(cancellationToken);
    }

    public Task UpsertAsync(Tren tren, CancellationToken cancellationToken = default)
    {
        return _trenService.UpsertAsync(tren, cancellationToken);
    }
}
