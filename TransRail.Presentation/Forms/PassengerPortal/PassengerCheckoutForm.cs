using System.Drawing;
using System.Windows.Forms;
using TransRail.Application.DTOs;
using TransRail.Domain.Entities;
using TransRail.Domain.Enums;
using TransRail.Presentation.Controls;
using TransRail.Presentation.Presenters;
using TransRail.Presentation.Theme;
using TransRail.Presentation.Views;

namespace TransRail.Presentation.Forms;

public sealed class PassengerCheckoutForm : TransRailFormBase, IPassengerCheckoutView
{
    private readonly Label _lblStatus = new() { AutoSize = true, ForeColor = TransRailTheme.WhiteSoft, Font = TransRailTheme.SubtitleFont };
    private readonly Label _lblSummary = new() { AutoSize = true, ForeColor = TransRailTheme.InkDark, Font = TransRailTheme.NormalFont };
    private readonly DataGridView _grid = new();
    private readonly PassengerCheckoutPresenter _presenter;

    public PassengerCheckoutForm()
    {
        Text = "Confirmar compra";
        Size = new Size(1240, 720);
        MinimumSize = new Size(1040, 640);
        BackColor = TransRailTheme.PrimaryDark;
        ForeColor = TransRailTheme.WhiteSoft;
        Font = TransRailTheme.NormalFont;

        var split = TransRailFormLayout.CreateModuleSplit(320, 290, 760);
        split.Panel1.Controls.Add(BuildActionsPanel());
        split.Panel2.Controls.Add(BuildSummaryPanel());
        Controls.Add(split);

        _presenter = new PassengerCheckoutPresenter(this, AppServices.PassengerPortalUseCase, AppServices.UserSession);
        Shown += async (_, _) => await _presenter.RefreshAsync();
    }

    public event EventHandler? RefreshRequested;
    public event EventHandler? ConfirmRequested;

    public void LoadSummary(PassengerPurchaseSummaryDto? summary, string statusText)
    {
        _lblStatus.Text = statusText;
        _lblSummary.Text = summary is null
            ? "Completa primero tu ruta, tu equipaje y tu pago para poder ver el resumen final."
            : $"Pasajero\n{summary.Pasajero.NombreCompleto}\n\n" +
              $"Ruta\n{summary.EtiquetaOrigen} -> {summary.EtiquetaDestino}\n\n" +
              $"Recorrido\n{summary.RecorridoTexto}\n\n" +
              $"Horario\n{summary.CodigoHorario} | {summary.FechaViaje:yyyy-MM-dd}\n{summary.HoraSalida:HH\\:mm} - {summary.HoraLlegada:HH\\:mm}\n\n" +
              $"Tipo de boleto\n{FormatTipoBoleto(summary.TipoBoleto)}\n\n" +
              $"Pago\n{FormatMetodoPago(summary.MetodoPago)}\n\n" +
              $"Total\n{summary.PrecioTotal:C}\n\n" +
              $"Equipaje de mano\n{(string.IsNullOrWhiteSpace(summary.EquipajeDeMano) ? "Sin registrar" : summary.EquipajeDeMano)}\n\n" +
              $"Equipaje documentado\n{(string.IsNullOrWhiteSpace(summary.CodigoEquipaje) ? "No registrar\u00e1 equipaje documentado" : $"{summary.EquipajeDescripcion} ({summary.EquipajePesoKg:0.##} kg)")}";
    }

    public void BindTickets(IReadOnlyCollection<Boleto> boletos)
    {
        _grid.DataSource = boletos.Select(x => new
        {
            x.CodigoBoleto,
            x.CodigoHorario,
            Tipo = FormatTipoBoleto(x.TipoBoleto),
            Precio = x.Precio.ToString("C"),
            FechaCompra = x.FechaCompraUtc.ToLocalTime().ToString("yyyy-MM-dd HH:mm")
        }).ToList();
    }

    public void ShowMessage(string message)
    {
        MessageBox.Show(this, message, "TransRail", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    public void ShowPurchasePopup(string details)
    {
        MessageBox.Show(this, details, "Compra confirmada", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private Control BuildActionsPanel()
    {
        var form = new TableLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 1,
            Width = 280,
            Margin = Padding.Empty
        };

        var btnRefresh = new TransRailButton
        {
            Text = "Actualizar resumen",
            Width = 280,
            Height = 46,
            Margin = new Padding(0, 0, 0, 8)
        };
        btnRefresh.Click += (_, _) => RefreshRequested?.Invoke(this, EventArgs.Empty);
        form.Controls.Add(btnRefresh);

        var btnConfirm = new TransRailButton
        {
            Text = "Confirmar compra",
            Width = 280,
            Height = 46,
            Margin = new Padding(0, 0, 0, 14)
        };
        btnConfirm.Click += (_, _) => ConfirmRequested?.Invoke(this, EventArgs.Empty);
        form.Controls.Add(btnConfirm);

        var note = new Label
        {
            Text = "Antes de confirmar, verifica que la ruta, el horario, el equipaje y el pago est\u00e9n correctos.",
            ForeColor = TransRailTheme.WhiteSoft,
            AutoSize = true,
            MaximumSize = new Size(280, 0),
            Margin = new Padding(0, 0, 0, 16)
        };
        form.Controls.Add(note);
        form.Controls.Add(_lblStatus);

        return TransRailFormLayout.CreateCenteredScrollHost(form, TransRailTheme.Surface, 280);
    }

    private Control BuildSummaryPanel()
    {
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = TransRailTheme.SurfaceAlt,
            Padding = new Padding(18),
            ColumnCount = 1,
            RowCount = 2
        };
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 58));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 42));

        var summaryCard = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Padding = new Padding(20)
        };
        var summaryStack = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true
        };
        summaryStack.Controls.Add(_lblSummary);
        summaryCard.Controls.Add(summaryStack);
        panel.Controls.Add(summaryCard, 0, 0);
        TransRailFormLayout.BindWrapWidth(_lblSummary, summaryStack, 10);

        var ticketsCard = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Padding = new Padding(16),
            ColumnCount = 1,
            RowCount = 2
        };
        ticketsCard.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        ticketsCard.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        ticketsCard.Controls.Add(new Label
        {
            Text = "Tus boletos recientes",
            Dock = DockStyle.Fill,
            Font = TransRailTheme.SectionFont,
            ForeColor = TransRailTheme.InkDark
        }, 0, 0);

        _grid.Dock = DockStyle.Fill;
        _grid.ReadOnly = true;
        _grid.AllowUserToAddRows = false;
        _grid.AllowUserToDeleteRows = false;
        _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _grid.MultiSelect = false;
        TransRailGridStyler.ApplyStandardStyle(_grid);
        ticketsCard.Controls.Add(_grid, 0, 1);
        panel.Controls.Add(ticketsCard, 0, 1);

        return panel;
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
