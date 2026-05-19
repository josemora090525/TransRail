using TransRail.Application.Services;
using TransRail.Domain.Entities;

namespace TransRail.Application.UseCases.Employee;

public sealed class ManageEmployeeUseCase
{
    private readonly EmpleadoService _empleadoService;

    public ManageEmployeeUseCase(EmpleadoService empleadoService)
    {
        _empleadoService = empleadoService;
    }

    public Task<IReadOnlyCollection<Empleado>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _empleadoService.GetAllAsync(cancellationToken);
    }

    public Task UpsertAsync(Empleado empleado, CancellationToken cancellationToken = default)
    {
        return _empleadoService.UpsertAsync(empleado, cancellationToken);
    }

    public Task DeleteAsync(string codigoEmpleado, CancellationToken cancellationToken = default)
    {
        return _empleadoService.DeleteAsync(codigoEmpleado, cancellationToken);
    }

    public Task<Empleado?> GetByCodigoAsync(string codigoEmpleado, CancellationToken cancellationToken = default)
    {
        return _empleadoService.GetByCodigoAsync(codigoEmpleado, cancellationToken);
    }
}
