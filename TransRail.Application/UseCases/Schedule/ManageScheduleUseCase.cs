using TransRail.Application.Services;
using TransRail.Domain.Entities;

namespace TransRail.Application.UseCases.Schedule;

public sealed class ManageScheduleUseCase
{
    private readonly HorarioService _horarioService;

    public ManageScheduleUseCase(HorarioService horarioService)
    {
        _horarioService = horarioService;
    }

    public Task<IReadOnlyCollection<Horario>> GetAllSortedAsync(CancellationToken cancellationToken = default)
    {
        return _horarioService.GetAllOrdenadosAsync(cancellationToken);
    }

    public Task UpsertAsync(Horario horario, CancellationToken cancellationToken = default)
    {
        return _horarioService.UpsertAsync(horario, cancellationToken);
    }

    public Task<IReadOnlyCollection<Horario>> GetByTrainAsync(string codigoTren, CancellationToken cancellationToken = default)
    {
        return _horarioService.BuscarPorCodigoTrenAsync(codigoTren, cancellationToken);
    }
}
