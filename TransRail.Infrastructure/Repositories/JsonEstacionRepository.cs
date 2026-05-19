using TransRail.Application.Interfaces;
using TransRail.Domain.Entities;

namespace TransRail.Infrastructure.Repositories;

public sealed class JsonEstacionRepository : JsonRepositoryBase<Estacion>, IEstacionRepository
{
    public JsonEstacionRepository(IJsonStorage storage)
        : base(storage, "estaciones.json")
    {
    }
}

