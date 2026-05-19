using TransRail.Domain.Entities;

namespace TransRail.Presentation.Views;

public interface IEquipajeView
{
    string CodigoEquipaje { get; }
    string CodigoBoleto { get; }
    string CodigoVagonCarga { get; }
    double PesoKg { get; }
    string Descripcion { get; }
    string CodigoBusqueda { get; }
    string CodigoVagonFiltro { get; }

    event EventHandler? SaveRequested;
    event EventHandler? DeleteRequested;
    event EventHandler? RefreshRequested;
    event EventHandler? SearchRequested;
    event EventHandler? FilterByVagonRequested;
    event EventHandler? BuildStackRequested;

    void BindEquipajes(IReadOnlyCollection<Equipaje> equipajes);
    void FillForm(Equipaje equipaje);
    void ShowStackInfo(string stackSummary);
    void ShowMessage(string message);
}
