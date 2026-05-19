using TransRail.Application.UseCases.Passenger;
using TransRail.Domain.Enums;
using TransRail.Presentation.Views;

namespace TransRail.Presentation.Presenters;

public sealed class PassengerCheckoutPresenter
{
    private readonly IPassengerCheckoutView _view;
    private readonly PassengerPortalUseCase _useCase;
    private readonly UserSession _session;

    public PassengerCheckoutPresenter(IPassengerCheckoutView view, PassengerPortalUseCase useCase, UserSession session)
    {
        _view = view;
        _useCase = useCase;
        _session = session;
        _view.RefreshRequested += OnRefreshRequested;
        _view.ConfirmRequested += OnConfirmRequested;
    }

    public async Task RefreshAsync()
    {
        var passenger = await _useCase.GetOrCreatePassengerAsync(_session.Correo, _session.CodigoUsuario, _session.NombreCompleto);
        var tickets = await _useCase.GetTicketsByPassengerAsync(passenger.CodigoUsuario);
        _view.BindTickets(tickets);

        try
        {
            var summary = await _useCase.BuildCheckoutSummaryAsync(_session.Correo, _session.CodigoUsuario, _session.NombreCompleto);
            _view.LoadSummary(summary, "Revisa cada dato antes de confirmar la compra.");
        }
        catch (Exception ex)
        {
            _view.LoadSummary(null, ex.Message);
        }
    }

    private async void OnRefreshRequested(object? sender, EventArgs e)
    {
        await RefreshAsync();
    }

    private async void OnConfirmRequested(object? sender, EventArgs e)
    {
        try
        {
            var summary = await _useCase.ConfirmPurchaseAsync(_session.Correo, _session.CodigoUsuario, _session.NombreCompleto);
            _view.ShowPurchasePopup(BuildPopup(summary));
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            _view.ShowMessage($"No se pudo confirmar la compra: {ex.Message}");
        }
    }

    private static string BuildPopup(Application.DTOs.PassengerPurchaseSummaryDto summary)
    {
        return
            $"Compra confirmada\n\n" +
            $"Pasajero: {summary.Pasajero.NombreCompleto}\n" +
            $"Boleto: {summary.CodigoBoleto}\n" +
            $"Ruta: {summary.EtiquetaOrigen} -> {summary.EtiquetaDestino}\n" +
            $"Recorrido: {summary.RecorridoTexto}\n" +
            $"Horario: {summary.CodigoHorario} | {summary.FechaViaje:yyyy-MM-dd} | {summary.HoraSalida:HH\\:mm} - {summary.HoraLlegada:HH\\:mm}\n" +
            $"Tipo de boleto: {FormatTipoBoleto(summary.TipoBoleto)}\n" +
            $"Pago: {FormatMetodoPago(summary.MetodoPago)}\n" +
            $"Valor total: {summary.PrecioTotal:C}\n" +
            $"Equipaje de mano: {summary.EquipajeDeMano}\n" +
            $"Equipaje registrado: {(string.IsNullOrWhiteSpace(summary.CodigoEquipaje) ? "No" : $"{summary.CodigoEquipaje} ({summary.EquipajeDescripcion}, {summary.EquipajePesoKg} kg)")}";
    }

    private static string FormatTipoBoleto(TipoBoleto value)
    {
        return value switch
        {
            TipoBoleto.Estandar => "Est\u00e1ndar",
            TipoBoleto.Ejecutivo => "Ejecutivo",
            TipoBoleto.Premium => "Premium",
            _ => value.ToString()
        };
    }

    private static string FormatMetodoPago(MetodoPago value)
    {
        return value switch
        {
            MetodoPago.TarjetaCredito => "Tarjeta de cr\u00e9dito",
            MetodoPago.TarjetaDebito => "Tarjeta d\u00e9bito",
            MetodoPago.Transferencia => "Transferencia",
            MetodoPago.Efectivo => "Efectivo",
            _ => value.ToString()
        };
    }
}
