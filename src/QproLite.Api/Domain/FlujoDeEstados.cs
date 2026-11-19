namespace QproLite.Api.Domain;

public static class FlujoDeEstados
{
    /// <summary>Indica si un pedido puede pasar del estado actual al nuevo estado.</summary>
    public static bool PuedeCambiar(EstadoPedido actual, EstadoPedido nuevo)
    {
        // Pendiente: aún no hay implementación (etapa ROJA del ciclo TDD).
        return false;
    }
}
