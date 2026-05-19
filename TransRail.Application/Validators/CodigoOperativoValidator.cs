namespace TransRail.Application.Validators;

public static class CodigoOperativoValidator
{
    public static bool EsValido(string? codigo)
    {
        return !string.IsNullOrWhiteSpace(codigo) && codigo.Length >= 4 && codigo.Contains('-');
    }
}

