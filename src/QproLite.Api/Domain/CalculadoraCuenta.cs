namespace QproLite.Api.Domain;

public record Cuenta(decimal Subtotal, decimal PropinaSugerida, decimal Total);

public static class CalculadoraCuenta
{
    /// <summary>Calcula la cuenta de una mesa a partir de sus pedidos sin pagar.</summary>
    public static Cuenta Calcular(IEnumerable<Pedido> pedidos, bool incluirPropina = true)
    {
        decimal subtotal = 0;
        foreach (var pedido in pedidos)
        {
            foreach (var item in pedido.Items)
            {
                subtotal += item.PrecioUnitario * item.Cantidad;
            }
        }

        var propina = subtotal * 0.10m;
        if (incluirPropina)
        {
            return new Cuenta(subtotal, propina, subtotal + propina);
        }

        return new Cuenta(subtotal, propina, subtotal);
    }
}
