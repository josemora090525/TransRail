using System.Drawing;
using System.Windows.Forms;
using TransRail.Domain.Enums;
using TransRail.Presentation.Controls;
using TransRail.Presentation.Presenters;
using TransRail.Presentation.Theme;
using TransRail.Presentation.Views;

namespace TransRail.Presentation.Forms;

public sealed class BoardingManagementForm : TransRailFormBase, IBoardingView
{
    private readonly TextBox _txtCodigoPasajero = new();
    private readonly Label _lblSiguiente = new() { AutoSize = true, ForeColor = TransRailTheme.WhiteSoft };
    private readonly DataGridView _grid = new();
    private readonly BoardingPresenter _presenter;

    public BoardingManagementForm()
    {
        Text = "Gesti\u00f3n de abordaje";
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(1080, 640);
        MinimumSize = new Size(900, 560);
        BackColor = TransRailTheme.PrimaryDark;
        ForeColor = TransRailTheme.WhiteSoft;
        Font = TransRailTheme.NormalFont;

        var split = TransRailFormLayout.CreateModuleSplit(320, 280, 560);
        split.Panel1.Controls.Add(BuildLeftPanel());
        split.Panel2.Controls.Add(BuildGrid());
        Controls.Add(split);

        _presenter = new BoardingPresenter(this, AppServices.ManageBoardingQueueUseCase, AppServices.ManagePassengerUseCase);
        Shown += (_, _) => _presenter.RefreshQueue();
    }

    public string CodigoPasajero => _txtCodigoPasajero.Text.Trim();
    public event EventHandler? EnqueueRequested;
    public event EventHandler? CallNextRequested;
    public event EventHandler? RefreshQueueRequested;
    public event EventHandler? ClearQueueRequested;

    public void BindQueue(IReadOnlyCollection<BoardingQueueItemVm> queueItems)
    {
        _grid.DataSource = queueItems.Select(x => new
        {
            x.CodigoPasajero,
            x.Nombre,
            x.Categoria,
            x.Prioridad
        }).ToList();
    }

    public void ShowNextPassenger(string pasajero, PrioridadAbordaje prioridad)
    {
        _lblSiguiente.Text = $"Siguiente abordaje: {pasajero} ({prioridad})";
    }

    public void ShowMessage(string message)
    {
        MessageBox.Show(this, message, "TransRail", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private Control BuildLeftPanel()
    {
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            Padding = new Padding(14),
            ColumnCount = 1,
            BackColor = TransRailTheme.Surface
        };

        panel.Controls.Add(MakeLabel("C\u00f3digo del pasajero"));
        panel.Controls.Add(_txtCodigoPasajero);

        var actions = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = true };
        var btnEncolar = new TransRailButton { Text = "Encolar pasajero", Width = 150 };
        btnEncolar.Click += (_, _) => EnqueueRequested?.Invoke(this, EventArgs.Empty);
        actions.Controls.Add(btnEncolar);

        var btnSiguiente = new TransRailButton { Text = "Llamar siguiente", Width = 150 };
        btnSiguiente.Click += (_, _) => CallNextRequested?.Invoke(this, EventArgs.Empty);
        actions.Controls.Add(btnSiguiente);

        var btnRefresh = new TransRailButton { Text = "Actualizar cola", Width = 150 };
        btnRefresh.Click += (_, _) => RefreshQueueRequested?.Invoke(this, EventArgs.Empty);
        actions.Controls.Add(btnRefresh);

        var btnLimpiar = new TransRailButton { Text = "Limpiar cola", Width = 150 };
        btnLimpiar.Click += (_, _) => ClearQueueRequested?.Invoke(this, EventArgs.Empty);
        actions.Controls.Add(btnLimpiar);

        panel.Controls.Add(actions);
        panel.Controls.Add(_lblSiguiente);
        panel.Controls.Add(new Label
        {
            Text = "Prioridad: adulto mayor > discapacidad > premium > ejecutivo > est\u00e1ndar.",
            AutoSize = true,
            ForeColor = TransRailTheme.WhiteSoft,
            Padding = new Padding(0, 8, 0, 0)
        });
        return panel;
    }

    private Control BuildGrid()
    {
        _grid.Dock = DockStyle.Fill;
        _grid.ReadOnly = true;
        _grid.AllowUserToAddRows = false;
        _grid.AllowUserToDeleteRows = false;
        _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _grid.MultiSelect = false;
        TransRailGridStyler.ApplyStandardStyle(_grid);
        return _grid;
    }

    private static Label MakeLabel(string text)
    {
        return new Label
        {
            Text = text,
            ForeColor = TransRailTheme.WhiteSoft,
            AutoSize = true,
            TextAlign = ContentAlignment.BottomLeft
        };
    }
}
