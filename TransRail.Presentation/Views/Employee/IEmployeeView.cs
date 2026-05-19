using TransRail.Domain.Entities;

namespace TransRail.Presentation.Views;

public interface IEmployeeView
{
    string CodigoEmpleado { get; }
    string NombreCompleto { get; }
    string NumeroDocumento { get; }
    string Correo { get; }
    string Contrasena { get; }
    string CodigoBusqueda { get; }

    event EventHandler? SaveRequested;
    event EventHandler? DeleteRequested;
    event EventHandler? SearchRequested;
    event EventHandler? RefreshRequested;

    void BindEmpleados(IReadOnlyCollection<Empleado> empleados);
    void FillForm(Empleado empleado);
    void ShowMessage(string message);
}
