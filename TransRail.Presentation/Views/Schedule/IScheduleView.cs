using TransRail.Domain.Entities;

namespace TransRail.Presentation.Views;

public interface IScheduleView
{
    string CodigoHorario { get; }
    string CodigoTren { get; }
    string CodigoRuta { get; }
    DateOnly Fecha { get; }
    TimeOnly HoraSalida { get; }
    TimeOnly HoraLlegada { get; }

    string CodigoTrenFiltro { get; }

    event EventHandler? CreateRequested;
    event EventHandler? RefreshRequested;
    event EventHandler? FilterByTrainRequested;

    void BindHorarios(IReadOnlyCollection<Horario> horarios);
    void ShowMessage(string message);
}

