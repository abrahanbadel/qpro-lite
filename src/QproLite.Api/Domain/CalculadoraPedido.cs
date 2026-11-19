namespace QproLite.Api.Domain;

public static class CalculadoraPedido
{
    /// <summary>Total del pedido: suma de los subtotales de sus ítems.</summary>
    public static decimal CalcularTotal(IEnumerable<PedidoItem> items) => items.Sum(item => item.Subtotal);
}
