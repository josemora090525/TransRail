using TransRail.Application.Interfaces;
using TransRail.Domain.Entities;

namespace TransRail.Infrastructure.Repositories;

public sealed class JsonTrenRepository : JsonRepositoryBase<Tren>, ITrenRepository
{
    public JsonTrenRepository(IJsonStorage storage)
        : base(storage, "trenes.json")
    {
    }
}

