namespace QproLite.Api.Domain;

public static class Inventario
{
    /// <summary>Con estas existencias o menos, el producto aparece con alerta en el inventario.</summary>
    public const int UmbralDeStockBajo = 5;

    public static bool HayExistencias(Producto producto, int cantidad) => producto.Stock >= cantidad;

    public static bool TieneStockBajo(Producto producto) => producto.Stock <= UmbralDeStockBajo;

    /// <summary>Descuenta las unidades vendidas; el producto queda agotado cuando llega a cero.</summary>
    public static void Descontar(Producto producto, int cantidad)
    {
        if (!HayExistencias(producto, cantidad))
        {
            throw new InvalidOperationException(MensajeSinExistencias(producto));
        }

        Reponer(producto, producto.Stock - cantidad);
    }

    /// <summary>Fija las existencias del producto y actualiza su disponibilidad en la carta.</summary>
    public static void Reponer(Producto producto, int existencias)
    {
        producto.Stock = existencias;
        producto.Disponible = existencias > 0;
    }

    public static string MensajeSinExistencias(Producto producto) =>
        $"Solo quedan {producto.Stock} unidades de {producto.Nombre}";
}
