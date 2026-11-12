namespace QproLite.Api.Domain;

public static class FlujoDeEstados
{
    /// <summary>Indica si un pedido puede pasar del estado actual al nuevo estado.</summary>
    public static bool PuedeCambiar(EstadoPedido actual, EstadoPedido nuevo) => Siguiente(actual) == nuevo;

    /// <summary>Estado que sigue en el flujo de cocina, o null si el pedido ya terminó.</summary>
    public static EstadoPedido? Siguiente(EstadoPedido actual) => actual switch
    {
        EstadoPedido.Pendiente => EstadoPedido.EnPreparacion,
        EstadoPedido.EnPreparacion => EstadoPedido.Listo,
        EstadoPedido.Listo => EstadoPedido.Entregado,
        _ => null,
    };
}
