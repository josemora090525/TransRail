namespace TransRail.Presentation.Forms;

public sealed class PurchaseConfirmationForm : ModuleLauncherForm
{
    public PurchaseConfirmationForm()
        : base(
            "TransRail - Confirmar compra",
            "Confirmar compra",
            "La confirmación final de la compra se ejecuta desde el módulo de boletos.",
            () => new TicketForm())
    {
    }
}
