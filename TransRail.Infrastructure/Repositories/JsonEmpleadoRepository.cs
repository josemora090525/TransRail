using TransRail.Application.Interfaces;
using TransRail.Domain.Entities;

namespace TransRail.Infrastructure.Repositories;

public sealed class JsonEmpleadoRepository : JsonRepositoryBase<Empleado>, IEmpleadoRepository
{
    public JsonEmpleadoRepository(IJsonStorage storage)
        : base(storage, "empleados.json")
    {
    }
}

