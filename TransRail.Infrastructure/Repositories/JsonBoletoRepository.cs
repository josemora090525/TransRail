using TransRail.Application.Interfaces;
using TransRail.Domain.Entities;

namespace TransRail.Infrastructure.Repositories;

public sealed class JsonBoletoRepository : JsonRepositoryBase<Boleto>, IBoletoRepository
{
    public JsonBoletoRepository(IJsonStorage storage)
        : base(storage, "boletos.json")
    {
    }
}

