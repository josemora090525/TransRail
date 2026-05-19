using System.Drawing;
using System.Windows.Forms;
using TransRail.Application.DTOs;
using TransRail.Domain.Enums;
using TransRail.Presentation.Controls;
using TransRail.Presentation.Presenters;
using TransRail.Presentation.Theme;
using TransRail.Presentation.Views;

namespace TransRail.Presentation.Forms;

public sealed class PassengerPaymentForm : TransRailFormBase, IPassengerPaymentView
{
    private readonly ComboBox _cmbTipoBoleto = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly ComboBox _cmbMetodoPago = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly Label _lblResumen = new() { AutoSize = true, ForeColor = TransRailTheme.InkDark, Font = TransRailTheme.NormalFont };
    private readonly Label _lblPrecio = new() { AutoSize = true, ForeColor = TransRailTheme.AccentGreen, Font = new Font("Segoe UI", 18, FontStyle.Bold) };
    private readonly PassengerPaymentPresenter _presenter;

    public PassengerPaymentForm()
    {
        Text = "M\u00e9todo de pago";
        Size = new Size(1160, 700);
        MinimumSize = new Size(980, 640);
        BackColor = TransRailTheme.PrimaryDark;
        ForeColor = TransRailTheme.WhiteSoft;
        Font = TransRailTheme.NormalFont;

        ConfigureCombos();

        var split = TransRailFormLayout.CreateModuleSplit(320, 300, 620);
        split.Panel1.Controls.Add(BuildFormPanel());
        split.Panel2.Controls.Add(BuildSummaryPanel());
        Controls.Add(split);

        _presenter = new PassengerPaymentPresenter(this, AppServices.PassengerPortalUseCase);
        Shown += (_, _) => _presenter.Load();
    }

    public TipoBoleto TipoBoleto => _cmbTipoBoleto.SelectedValue is TipoBoleto value ? value : TipoBoleto.Estandar;
    public MetodoPago MetodoPago => _cmbMetodoPago.SelectedValue is MetodoPago value ? value : MetodoPago.TarjetaDebito;

    public event EventHandler? SaveRequested;

    public void LoadDraft(PassengerPurchaseDraftDto draft)
    {
        _cmbTipoBoleto.SelectedValue = draft.TipoBoleto;
        _cmbMetodoPago.SelectedValue = draft.MetodoPago;
        _lblPrecio.Text = $"Total estimado: {draft.PrecioCalculado:C}";
        _lblResumen.Text =
            $"Ruta\n{(string.IsNullOrWhiteSpace(draft.EtiquetaOrigen) ? "A\u00fan no seleccionada" : $"{draft.EtiquetaOrigen} -> {draft.EtiquetaDestino}")}\n\n" +
            $"Horario seleccionado\n{(string.IsNullOrWhiteSpace(draft.CodigoHorario) ? "A\u00fan no seleccionado" : $"{draft.CodigoHorario} | {draft.FechaViaje:yyyy-MM-dd} | {draft.HoraSalida:HH\\:mm} - {draft.HoraLlegada:HH\\:mm}")}\n\n" +
            $"Tipo de boleto\n{FormatTipoBoleto(draft.TipoBoleto)}\n\n" +
            $"M\u00e9todo de pago\n{FormatMetodoPago(draft.MetodoPago)}";
    }

    public void ShowMessage(string message)
    {
        MessageBox.Show(this, message, "TransRail", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private Control BuildFormPanel()
    {
        var form = new TableLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 1,
            Width = 280,
            Margin = Padding.Empty
        };

        form.Controls.Add(MakeLabel("Tipo de boleto"));
        form.Controls.Add(BuildInput(_cmbTipoBoleto));
        form.Controls.Add(MakeLabel("M\u00e9todo de pago"));
        form.Controls.Add(BuildInput(_cmbMetodoPago));

        var btnGuardar = new TransRailButton
        {
            Text = "Guardar pago",
            Width = 280,
            Height = 46,
            Margin = new Padding(0, 12, 0, 0)
        };
        btnGuardar.Click += (_, _) => SaveRequested?.Invoke(this, EventArgs.Empty);
        form.Controls.Add(btnGuardar);

        var note = new Label
        {
            Text = "El valor se recalcula autom\u00e1ticamente seg\u00fan la distancia de la ruta y el tipo de boleto seleccionado.",
            ForeColor = TransRailTheme.WhiteSoft,
            AutoSize = true,
            Margin = new Padding(0, 14, 0, 0),
            MaximumSize = new Size(280, 0)
        };
        form.Controls.Add(note);

        return TransRailFormLayout.CreateCenteredScrollHost(form, TransRailTheme.Surface, 280);
    }

    private Control BuildSummaryPanel()
    {
        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = TransRailTheme.SurfaceAlt,
            Padding = new Padding(18)
        };

        var card = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Padding = new Padding(22)
        };
        var stack = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true
        };
        _lblPrecio.Margin = new Padding(0, 0, 0, 18);
        _lblResumen.Margin = new Padding(0);
        stack.Controls.Add(_lblPrecio);
        stack.Controls.Add(_lblResumen);
        card.Controls.Add(stack);
        panel.Controls.Add(card);
        TransRailFormLayout.BindWrapWidth(_lblPrecio, stack, 10);
        TransRailFormLayout.BindWrapWidth(_lblResumen, stack, 10);

        return panel;
    }

    private void ConfigureCombos()
    {
        _cmbTipoBoleto.DisplayMember = nameof(SelectionOption<TipoBoleto>.Label);
        _cmbTipoBoleto.ValueMember = nameof(SelectionOption<TipoBoleto>.Value);
        _cmbTipoBoleto.DataSource = Enum
            .GetValues<TipoBoleto>()
            .Select(value => new SelectionOption<TipoBoleto>(value, FormatTipoBoleto(value)))
            .ToList();

        _cmbMetodoPago.DisplayMember = nameof(SelectionOption<MetodoPago>.Label);
        _cmbMetodoPago.ValueMember = nameof(SelectionOption<MetodoPago>.Value);
        _cmbMetodoPago.DataSource = Enum
            .GetValues<MetodoPago>()
            .Select(value => new SelectionOption<MetodoPago>(value, FormatMetodoPago(value)))
            .ToList();
    }

    private static Control BuildInput(Control control)
    {
        control.Width = 280;
        control.Margin = new Padding(0, 4, 0, 10);
        return control;
    }

    private static Label MakeLabel(string text)
    {
        return new Label
        {
            Text = text,
            ForeColor = TransRailTheme.WhiteSoft,
            AutoSize = true,
            Margin = new Padding(0)
        };
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

    private sealed record SelectionOption<T>(T Value, string Label);
}
