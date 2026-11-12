namespace QproLite.Api.Domain;

public static class Restaurante
{
    public const int NumeroDeMesas = 10;

    public static bool MesaExiste(int mesa) => mesa is >= 1 and <= NumeroDeMesas;
}
