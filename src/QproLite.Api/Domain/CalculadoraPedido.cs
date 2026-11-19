namespace QproLite.Api.Domain;

public static class CalculadoraPedido
{
    /// <summary>Total del pedido: suma de precio unitario por cantidad de cada ítem.</summary>
    public static decimal CalcularTotal(IEnumerable<PedidoItem> items)
    {
        // Pendiente: aún no hay implementación (etapa ROJA del ciclo TDD).
        return -1;
    }
}
