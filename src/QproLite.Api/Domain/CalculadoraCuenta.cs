namespace QproLite.Api.Domain;

public record Cuenta(decimal Subtotal, decimal PropinaSugerida, decimal Total);

public static class CalculadoraCuenta
{
    public const decimal PorcentajeDePropina = 0.10m;

    /// <summary>Calcula la cuenta de una mesa a partir de sus pedidos sin pagar.</summary>
    public static Cuenta Calcular(IEnumerable<Pedido> pedidos, bool incluirPropina = true)
    {
        var subtotal = pedidos.Sum(pedido => CalculadoraPedido.CalcularTotal(pedido.Items));
        var propina = subtotal * PorcentajeDePropina;
        return new Cuenta(subtotal, propina, incluirPropina ? subtotal + propina : subtotal);
    }
}
