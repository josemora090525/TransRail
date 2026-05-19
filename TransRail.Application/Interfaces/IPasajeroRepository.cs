using TransRail.Domain.Entities;

namespace TransRail.Application.Interfaces;

public interface IPasajeroRepository : IRepositoryBase<Pasajero>
{
    Task<Pasajero?> GetByCorreoAsync(string correo, CancellationToken cancellationToken = default);
}


