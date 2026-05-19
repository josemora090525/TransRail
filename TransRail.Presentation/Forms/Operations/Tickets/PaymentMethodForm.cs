namespace TransRail.Presentation.Forms;

public sealed class PaymentMethodForm : ModuleLauncherForm
{
    public PaymentMethodForm()
        : base(
            "TransRail - Método de pago",
            "Método de pago",
            "La selección y el registro del pago se gestionan en el módulo de boletos y pagos.",
            () => new TicketForm())
    {
    }
}
