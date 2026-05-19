using TransRail.Domain.Entities;

namespace TransRail.Presentation.Views;

public interface IStationView
{
    string CodigoEstacion { get; }
    string NombreEstacion { get; }
    string CiudadEstacion { get; }

    event EventHandler? CreateRequested;
    event EventHandler? RefreshRequested;

    void BindEstaciones(IReadOnlyCollection<Estacion> estaciones);
    void ShowMessage(string message);
}

