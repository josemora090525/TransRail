using System.Drawing;
using System.Windows.Forms;
using TransRail.Application.DTOs;
using TransRail.Presentation.Controls;
using TransRail.Presentation.Presenters;
using TransRail.Presentation.Theme;
using TransRail.Presentation.Views;

namespace TransRail.Presentation.Forms;

public sealed class PassengerLuggageForm : TransRailFormBase, IPassengerLuggageView
{
    private readonly TextBox _txtEquipajeDeMano = new() { Multiline = true, Height = 84, ScrollBars = ScrollBars.Vertical };
    private readonly TextBox _txtDescripcion = new() { Multiline = true, Height = 96, ScrollBars = ScrollBars.Vertical };
    private readonly NumericUpDown _numPeso = new() { Minimum = 0, Maximum = 100, DecimalPlaces = 2, Increment = 0.5m };
    private readonly Label _lblResumen = new() { AutoSize = true, ForeColor = TransRailTheme.InkDark, Font = TransRailTheme.NormalFont };
    private readonly PassengerLuggagePresenter _presenter;

    public PassengerLuggageForm()
    {
        Text = "Equipaje";
        Size = new Size(1160, 700);
        MinimumSize = new Size(980, 640);
        BackColor = TransRailTheme.PrimaryDark;
        ForeColor = TransRailTheme.WhiteSoft;
        Font = TransRailTheme.NormalFont;

        var split = TransRailFormLayout.CreateModuleSplit(320, 300, 620);
        split.Panel1.Controls.Add(BuildFormPanel());
        split.Panel2.Controls.Add(BuildSummaryPanel());
        Controls.Add(split);

        _presenter = new PassengerLuggagePresenter(this, AppServices.PassengerPortalUseCase, AppServices.UserSession);
        Shown += async (_, _) => await _presenter.LoadAsync();
    }

    public string EquipajeDeMano => _txtEquipajeDeMano.Text.Trim();
    public string EquipajeDescripcion => _txtDescripcion.Text.Trim();
    public double EquipajePesoKg => (double)_numPeso.Value;

    public event EventHandler? SaveRequested;

    public void LoadDraft(PassengerPurchaseDraftDto draft)
    {
        _txtEquipajeDeMano.Text = draft.EquipajeDeMano;
        _txtDescripcion.Text = draft.EquipajeDescripcion;
        _numPeso.Value = Math.Clamp((decimal)draft.EquipajePesoKg, _numPeso.Minimum, _numPeso.Maximum);
        _lblResumen.Text =
            $"Ruta seleccionada\n{(string.IsNullOrWhiteSpace(draft.EtiquetaOrigen) ? "A\u00fan no seleccionada" : $"{draft.EtiquetaOrigen} -> {draft.EtiquetaDestino}")}\n\n" +
            $"Horario seleccionado\n{(string.IsNullOrWhiteSpace(draft.CodigoHorario) ? "A\u00fan no seleccionado" : $"{draft.CodigoHorario} | {draft.FechaViaje:yyyy-MM-dd} | {draft.HoraSalida:HH\\:mm} - {draft.HoraLlegada:HH\\:mm}")}\n\n" +
            $"Equipaje documentado\n{(string.IsNullOrWhiteSpace(draft.EquipajeDescripcion) ? "Sin registrar" : $"{draft.EquipajeDescripcion} ({draft.EquipajePesoKg:0.##} kg)")}\n\n" +
            $"Equipaje de mano\n{(string.IsNullOrWhiteSpace(draft.EquipajeDeMano) ? "Sin registrar" : draft.EquipajeDeMano)}";
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

        form.Controls.Add(MakeLabel("Equipaje de mano"));
        form.Controls.Add(BuildInput(_txtEquipajeDeMano));
        form.Controls.Add(MakeLabel("Informaci\u00f3n del equipaje documentado"));
        form.Controls.Add(BuildInput(_txtDescripcion));
        form.Controls.Add(MakeLabel("Peso del equipaje (kg)"));
        form.Controls.Add(BuildInput(_numPeso));

        var btnGuardar = new TransRailButton
        {
            Text = "Guardar equipaje",
            Width = 280,
            Height = 46,
            Margin = new Padding(0, 12, 0, 0)
        };
        btnGuardar.Click += (_, _) => SaveRequested?.Invoke(this, EventArgs.Empty);
        form.Controls.Add(btnGuardar);

        var note = new Label
        {
            Text = "Registra lo que llevar\u00e1s contigo y el equipaje documentado antes de continuar al pago.",
            AutoSize = true,
            ForeColor = TransRailTheme.WhiteSoft,
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
        stack.Controls.Add(_lblResumen);
        card.Controls.Add(stack);
        panel.Controls.Add(card);
        TransRailFormLayout.BindWrapWidth(_lblResumen, stack, 10);
        return panel;
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
}
