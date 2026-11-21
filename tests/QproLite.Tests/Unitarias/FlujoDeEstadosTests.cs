using QproLite.Api.Domain;

namespace QproLite.Tests.Unitarias;

[Trait("Categoria", "Unitaria")]
public class FlujoDeEstadosTests
{
    [Theory]
    [InlineData(EstadoPedido.Pendiente, EstadoPedido.EnPreparacion)]
    [InlineData(EstadoPedido.EnPreparacion, EstadoPedido.Listo)]
    [InlineData(EstadoPedido.Listo, EstadoPedido.Entregado)]
    public void Un_pedido_puede_avanzar_al_estado_siguiente(EstadoPedido actual, EstadoPedido nuevo)
    {
        Assert.True(FlujoDeEstados.PuedeCambiar(actual, nuevo));
    }

    [Theory]
    [InlineData(EstadoPedido.Pendiente, EstadoPedido.Listo)]
    [InlineData(EstadoPedido.Pendiente, EstadoPedido.Entregado)]
    [InlineData(EstadoPedido.EnPreparacion, EstadoPedido.Entregado)]
    public void Un_pedido_no_puede_saltarse_estados(EstadoPedido actual, EstadoPedido nuevo)
    {
        Assert.False(FlujoDeEstados.PuedeCambiar(actual, nuevo));
    }

    [Theory]
    [InlineData(EstadoPedido.EnPreparacion, EstadoPedido.Pendiente)]
    [InlineData(EstadoPedido.Entregado, EstadoPedido.Listo)]
    [InlineData(EstadoPedido.Listo, EstadoPedido.Listo)]
    public void Un_pedido_no_puede_retroceder_ni_quedarse_en_el_mismo_estado(EstadoPedido actual, EstadoPedido nuevo)
    {
        Assert.False(FlujoDeEstados.PuedeCambiar(actual, nuevo));
    }
}
