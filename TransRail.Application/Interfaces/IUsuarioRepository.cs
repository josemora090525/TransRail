using TransRail.Domain.Entities;

namespace TransRail.Application.Interfaces;

public interface IUsuarioRepository : IRepositoryBase<Usuario>
{
    Task<Usuario?> GetByCorreoAsync(string correo, CancellationToken cancellationToken = default);
}


