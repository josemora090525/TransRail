using TransRail.Domain.Entities;

namespace TransRail.Presentation.Views;

public interface ITrainView
{
    string CodigoTren { get; }
    string NumeroOperativo { get; }
    string NombreTren { get; }
    int CapacidadVagones { get; }
    int Kilometraje { get; }
    bool EnCirculacion { get; }

    event EventHandler? CreateRequested;
    event EventHandler? RefreshRequested;

    void BindTrenes(IReadOnlyCollection<Tren> trenes);
    void ShowMessage(string message);
}

