using TransRail.Domain.Entities;
using TransRail.Domain.Enums;

namespace TransRail.Presentation.Views;

public interface IWagonView
{
    string CodigoVagon { get; }
    string CodigoTren { get; }
    TipoVagon TipoVagon { get; }
    int Capacidad { get; }
    double PesoMaximoKg { get; }
    string CodigoBusqueda { get; }
    string CodigoTrenFiltro { get; }

    event EventHandler? SaveRequested;
    event EventHandler? DeleteRequested;
    event EventHandler? SearchRequested;
    event EventHandler? FilterByTrainRequested;
    event EventHandler? RefreshRequested;

    void BindVagones(IReadOnlyCollection<Vagon> vagones);
    void FillForm(Vagon vagon);
    void ShowMessage(string message);
}
