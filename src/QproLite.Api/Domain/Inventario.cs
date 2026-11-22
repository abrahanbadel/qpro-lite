namespace QproLite.Api.Domain;

public static class Inventario
{
    // Pendiente: aún no hay implementación (etapa ROJA del ciclo TDD).
    public static bool HayExistencias(Producto producto, int cantidad) => true;

    public static void Descontar(Producto producto, int cantidad)
    {
    }

    public static bool TieneStockBajo(Producto producto) => false;

    public static void Reponer(Producto producto, int existencias)
    {
    }
}
