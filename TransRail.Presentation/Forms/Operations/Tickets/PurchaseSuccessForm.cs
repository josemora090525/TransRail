namespace TransRail.Presentation.Forms;

public sealed class PurchaseSuccessForm : ModuleLauncherForm
{
    public PurchaseSuccessForm()
        : base(
            "TransRail - Compra confirmada",
            "Compra confirmada",
            "Puedes revisar, imprimir o gestionar la compra desde el módulo de boletos.",
            () => new TicketForm())
    {
    }
}
