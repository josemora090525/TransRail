using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using TransRail.Presentation.Theme;

namespace TransRail.Presentation.Controls;

public static class TransRailGridStyler
{
    private static readonly Regex CamelCaseRegex = new("(?<=[a-z])(?=[A-Z])", RegexOptions.Compiled);
    private static readonly IReadOnlyDictionary<string, string> Replacements = new Dictionary<string, string>
    {
        ["Codigo"] = "C\u00f3digo",
        ["Credito"] = "cr\u00e9dito",
        ["Debito"] = "d\u00e9bito",
        ["Estandar"] = "Est\u00e1ndar",
        ["Numero"] = "N\u00famero",
        ["Categoria"] = "Categor\u00eda",
        ["Contrasena"] = "Contrase\u00f1a",
        ["Descripcion"] = "Descripci\u00f3n",
        ["Estacion"] = "Estaci\u00f3n",
        ["Metodo"] = "M\u00e9todo",
        ["Circulacion"] = "Circulaci\u00f3n",
        ["Maximo"] = "M\u00e1ximo",
        ["Minimo"] = "M\u00ednimo",
        ["Vagon"] = "Vag\u00f3n"
    };

    public static void ApplyStandardStyle(DataGridView grid)
    {
        grid.EnableHeadersVisualStyles = false;
        grid.BorderStyle = BorderStyle.None;
        grid.RowHeadersVisible = false;
        grid.GridColor = Color.FromArgb(214, 223, 229);
        grid.BackgroundColor = Color.White;
        grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        grid.DefaultCellStyle.BackColor = Color.White;
        grid.DefaultCellStyle.ForeColor = TransRailTheme.InkDark;
        grid.DefaultCellStyle.Font = new Font(TransRailTheme.NormalFont, FontStyle.Bold);
        grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(205, 224, 235);
        grid.DefaultCellStyle.SelectionForeColor = TransRailTheme.InkDark;
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(247, 250, 252);
        grid.AlternatingRowsDefaultCellStyle.ForeColor = TransRailTheme.InkDark;
        grid.RowsDefaultCellStyle.Font = new Font(TransRailTheme.NormalFont, FontStyle.Bold);
        grid.RowsDefaultCellStyle.ForeColor = TransRailTheme.InkDark;
        grid.ColumnHeadersDefaultCellStyle.BackColor = TransRailTheme.SurfaceAlt;
        grid.ColumnHeadersDefaultCellStyle.ForeColor = TransRailTheme.InkDark;
        grid.ColumnHeadersDefaultCellStyle.Font = new Font(TransRailTheme.NormalFont, FontStyle.Bold);
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        grid.DataBindingComplete -= OnDataBindingComplete;
        grid.DataBindingComplete += OnDataBindingComplete;
    }

    private static void OnDataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
    {
        if (sender is not DataGridView grid)
        {
            return;
        }

        foreach (DataGridViewColumn column in grid.Columns)
        {
            column.HeaderText = HumanizeText(column.Name);
            column.SortMode = DataGridViewColumnSortMode.Automatic;
            column.MinimumWidth = GetMinimumWidth(column.Name);
            column.FillWeight = GetFillWeight(column.Name);
        }
    }

    public static string HumanizeText(string source)
    {
        var value = CamelCaseRegex.Replace(source, " ");
        foreach (var replacement in Replacements)
        {
            value = value.Replace(replacement.Key, replacement.Value, StringComparison.Ordinal);
        }

        return value;
    }

    private static int GetMinimumWidth(string columnName)
    {
        if (columnName.Contains("Descripcion", StringComparison.OrdinalIgnoreCase) ||
            columnName.Contains("Nombre", StringComparison.OrdinalIgnoreCase) ||
            columnName.Contains("Correo", StringComparison.OrdinalIgnoreCase))
        {
            return 170;
        }

        if (columnName.Contains("Fecha", StringComparison.OrdinalIgnoreCase) ||
            columnName.Contains("Salida", StringComparison.OrdinalIgnoreCase) ||
            columnName.Contains("Llegada", StringComparison.OrdinalIgnoreCase))
        {
            return 120;
        }

        if (columnName.Contains("Codigo", StringComparison.OrdinalIgnoreCase) ||
            columnName.Contains("Numero", StringComparison.OrdinalIgnoreCase))
        {
            return 130;
        }

        if (columnName.Contains("Ciudad", StringComparison.OrdinalIgnoreCase) ||
            columnName.Contains("Categoria", StringComparison.OrdinalIgnoreCase))
        {
            return 140;
        }

        return 110;
    }

    private static float GetFillWeight(string columnName)
    {
        if (columnName.Contains("Descripcion", StringComparison.OrdinalIgnoreCase))
        {
            return 200f;
        }

        if (columnName.Contains("Nombre", StringComparison.OrdinalIgnoreCase) ||
            columnName.Contains("Correo", StringComparison.OrdinalIgnoreCase))
        {
            return 170f;
        }

        if (columnName.Contains("Ciudad", StringComparison.OrdinalIgnoreCase) ||
            columnName.Contains("Categoria", StringComparison.OrdinalIgnoreCase))
        {
            return 145f;
        }

        if (columnName.Contains("Fecha", StringComparison.OrdinalIgnoreCase) ||
            columnName.Contains("Salida", StringComparison.OrdinalIgnoreCase) ||
            columnName.Contains("Llegada", StringComparison.OrdinalIgnoreCase))
        {
            return 125f;
        }

        if (columnName.Contains("Codigo", StringComparison.OrdinalIgnoreCase) ||
            columnName.Contains("Numero", StringComparison.OrdinalIgnoreCase))
        {
            return 120f;
        }

        return 105f;
    }
}
