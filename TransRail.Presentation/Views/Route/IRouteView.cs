using TransRail.Domain.Entities;

namespace TransRail.Presentation.Views;

public interface IRouteView
{
    string CodigoRuta { get; }
    string CodigoOrigen { get; }
    string CodigoDestino { get; }
    int DistanciaKm { get; }

    event EventHandler? CreateRequested;
    event EventHandler? RefreshRequested;
    event EventHandler? CalculateRequested;

    string CalculoOrigen { get; }
    string CalculoDestino { get; }

    void BindRutas(IReadOnlyCollection<Ruta> rutas);
    void ShowRouteCalculation(int distancia, IReadOnlyList<string> ruta);
    void ShowMessage(string message);
}

