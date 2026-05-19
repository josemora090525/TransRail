using TransRail.Application.UseCases.Schedule;
using TransRail.Domain.Entities;
using TransRail.Presentation.Views;

namespace TransRail.Presentation.Presenters;

public sealed class SchedulePresenter
{
    private readonly IScheduleView _view;
    private readonly ManageScheduleUseCase _useCase;

    public SchedulePresenter(IScheduleView view, ManageScheduleUseCase useCase)
    {
        _view = view;
        _useCase = useCase;
        _view.CreateRequested += OnCreateRequested;
        _view.RefreshRequested += OnRefreshRequested;
        _view.FilterByTrainRequested += OnFilterByTrainRequested;
    }

    private async void OnCreateRequested(object? sender, EventArgs e)
    {
        try
        {
            var horario = new Horario
            {
                CodigoHorario = _view.CodigoHorario,
                CodigoTren = _view.CodigoTren,
                CodigoRuta = _view.CodigoRuta,
                Fecha = _view.Fecha,
                HoraSalida = _view.HoraSalida,
                HoraLlegada = _view.HoraLlegada
            };

            await _useCase.UpsertAsync(horario);
            _view.ShowMessage("Horario guardado correctamente.");
            await RefrescarAsync();
        }
        catch (Exception ex)
        {
            _view.ShowMessage($"No se pudo guardar el horario: {ex.Message}");
        }
    }

    private async void OnRefreshRequested(object? sender, EventArgs e)
    {
        await RefrescarAsync();
    }

    private async void OnFilterByTrainRequested(object? sender, EventArgs e)
    {
        var horarios = await _useCase.GetByTrainAsync(_view.CodigoTrenFiltro);
        _view.BindHorarios(horarios);
    }

    public async Task RefrescarAsync()
    {
        var horarios = await _useCase.GetAllSortedAsync();
        _view.BindHorarios(horarios);
    }
}
