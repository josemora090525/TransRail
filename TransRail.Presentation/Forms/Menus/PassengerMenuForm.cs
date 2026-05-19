namespace TransRail.Presentation.Forms;

public sealed class PassengerMenuForm : WorkspaceMenuFormBase
{
    public PassengerMenuForm()
        : base("TransRail - Men\u00fa de pasajero")
    {
        ConfigureHeader(
            "Pasajero",
            "Planea tu viaje, actualiza tus datos y confirma tu compra sin salir de esta ventana.",
            "qlementine-icons_user-16.png");

        SetWelcomeMessage(
            "Portal de viaje",
            "Sigue el flujo: datos personales, rutas, equipaje, pago y confirmaci\u00f3n.");

        AddModule("Mis datos", "qlementine-icons_user-16.png", () => new PassengerProfileForm());
        AddModule("Rutas disponibles", "gis_route-start.png", () => new PassengerRoutesForm());
        AddModule("Equipaje", "Group 2.png", () => new PassengerLuggageForm());
        AddModule("M\u00e9todo de pago", "ticket.generated", () => new PassengerPaymentForm());
        AddModule("Confirmar compra", "ticket.generated", () => new PassengerCheckoutForm());
    }
}
