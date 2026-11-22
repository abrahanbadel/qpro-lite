namespace QproLite.Api.Domain;

public static class Inventario
{
    public static bool HayExistencias(Producto producto, int cantidad)
    {
        if (producto.Stock >= cantidad)
        {
            return true;
        }

        return false;
    }

    public static void Descontar(Producto producto, int cantidad)
    {
        if (producto.Stock < cantidad)
        {
            throw new InvalidOperationException("Solo quedan " + producto.Stock + " unidades de " + producto.Nombre);
        }

        producto.Stock = producto.Stock - cantidad;
        if (producto.Stock == 0)
        {
            producto.Disponible = false;
        }
    }

    public static bool TieneStockBajo(Producto producto)
    {
        if (producto.Stock <= 5)
        {
            return true;
        }

        return false;
    }

    public static void Reponer(Producto producto, int existencias)
    {
        producto.Stock = existencias;
        if (existencias > 0)
        {
            producto.Disponible = true;
        }
        else
        {
            producto.Disponible = false;
        }
    }
}
