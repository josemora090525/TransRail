using TransRail.Domain.Entities;

namespace TransRail.Application.Interfaces;

public interface IHorarioRepository : IRepositoryBase<Horario>
{
    Task<IReadOnlyCollection<Horario>> GetByCodigoTrenAsync(string codigoTren, CancellationToken cancellationToken = default);
}


