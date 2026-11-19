namespace QproLite.Api.Domain;

public static class CalculadoraPedido
{
    /// <summary>Total del pedido: suma de precio unitario por cantidad de cada ítem.</summary>
    public static decimal CalcularTotal(IEnumerable<PedidoItem> items)
    {
        decimal total = 0;
        foreach (var item in items)
        {
            total = total + item.PrecioUnitario * item.Cantidad;
        }

        return total;
    }
}
