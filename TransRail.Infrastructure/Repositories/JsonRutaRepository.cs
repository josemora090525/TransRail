using TransRail.Application.Interfaces;
using TransRail.Domain.Entities;

namespace TransRail.Infrastructure.Repositories;

public sealed class JsonRutaRepository : JsonRepositoryBase<Ruta>, IRutaRepository
{
    public JsonRutaRepository(IJsonStorage storage)
        : base(storage, "rutas.json")
    {
    }
}

