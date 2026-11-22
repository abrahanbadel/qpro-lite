namespace QproLite.Api.Domain;

public record Cuenta(decimal Subtotal, decimal PropinaSugerida, decimal Total);

public static class CalculadoraCuenta
{
    /// <summary>Calcula la cuenta de una mesa a partir de sus pedidos sin pagar.</summary>
    public static Cuenta Calcular(IEnumerable<Pedido> pedidos, bool incluirPropina = true)
    {
        // Pendiente: aún no hay implementación (etapa ROJA del ciclo TDD).
        return new Cuenta(0, 0, 0);
    }
}
