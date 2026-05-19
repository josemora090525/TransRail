using TransRail.Application.UseCases.Passenger;
using TransRail.Domain.Entities;
using TransRail.Presentation.Views;

namespace TransRail.Presentation.Presenters;

public sealed class PassengerProfilePresenter
{
    private readonly IPassengerProfileView _view;
    private readonly PassengerPortalUseCase _useCase;
    private readonly UserSession _session;

    public PassengerProfilePresenter(IPassengerProfileView view, PassengerPortalUseCase useCase, UserSession session)
    {
        _view = view;
        _useCase = useCase;
        _session = session;
        _view.SaveRequested += OnSaveRequested;
    }

    public async Task LoadAsync()
    {
        var passenger = await _useCase.GetOrCreatePassengerAsync(_session.Correo, _session.CodigoUsuario, _session.NombreCompleto);
        _view.LoadPassenger(passenger);
    }

    private async void OnSaveRequested(object? sender, EventArgs e)
    {
        try
        {
            var existing = await _useCase.GetOrCreatePassengerAsync(_session.Correo, _session.CodigoUsuario, _session.NombreCompleto);
            var passenger = new Pasajero
            {
                IdInterno = existing.IdInterno,
                CodigoUsuario = existing.CodigoUsuario,
                Contrasena = existing.Contrasena,
                Categoria = existing.Categoria,
                Nombres = _view.Nombres,
                Apellidos = _view.Apellidos,
                Correo = _view.Correo,
                Direccion = _view.Direccion,
                TipoIdentificacion = _view.TipoIdentificacion,
                NumeroDocumento = _view.NumeroIdentificacion,
                Telefono = _view.Telefono,
                NombreContacto = _view.NombreContacto,
                ApellidoContacto = _view.ApellidoContacto,
                TelefonoContacto = _view.TelefonoContacto,
                EquipajeDeMano = _view.EquipajeDeMano,
                EsAdultoMayor = _view.EsAdultoMayor,
                TieneDiscapacidad = _view.TieneDiscapacidad
            };

            await _useCase.SavePassengerAsync(passenger);
            _session.Start(existing.CodigoUsuario, passenger.NombreCompleto, passenger.Correo, existing.Rol);
            _view.ShowMessage("Tus datos personales se actualizaron correctamente.");
            await LoadAsync();
        }
        catch (Exception ex)
        {
            _view.ShowMessage($"No se pudieron guardar tus datos: {ex.Message}");
        }
    }
}
