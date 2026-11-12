namespace QproLite.Api.Domain;

public static class FlujoDeEstados
{
    /// <summary>Indica si un pedido puede pasar del estado actual al nuevo estado.</summary>
    public static bool PuedeCambiar(EstadoPedido actual, EstadoPedido nuevo)
    {
        if (actual == EstadoPedido.Pendiente && nuevo == EstadoPedido.EnPreparacion)
        {
            return true;
        }

        if (actual == EstadoPedido.EnPreparacion && nuevo == EstadoPedido.Listo)
        {
            return true;
        }

        if (actual == EstadoPedido.Listo && nuevo == EstadoPedido.Entregado)
        {
            return true;
        }

        return false;
    }
}
