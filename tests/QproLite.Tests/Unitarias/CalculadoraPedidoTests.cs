using QproLite.Api.Domain;

namespace QproLite.Tests.Unitarias;

[Trait("Categoria", "Unitaria")]
public class CalculadoraPedidoTests
{
    [Fact]
    public void El_total_de_un_item_es_el_precio_por_la_cantidad()
    {
        PedidoItem[] items = [new() { PrecioUnitario = 22000, Cantidad = 2 }];

        Assert.Equal(44000, CalculadoraPedido.CalcularTotal(items));
    }

    [Fact]
    public void El_total_suma_todos_los_items_del_pedido()
    {
        PedidoItem[] items =
        [
            new() { PrecioUnitario = 22000, Cantidad = 2 },
            new() { PrecioUnitario = 6000, Cantidad = 3 },
            new() { PrecioUnitario = 4500, Cantidad = 1 },
        ];

        Assert.Equal(66500, CalculadoraPedido.CalcularTotal(items));
    }

    [Fact]
    public void Un_pedido_sin_items_tiene_total_cero()
    {
        Assert.Equal(0, CalculadoraPedido.CalcularTotal([]));
    }
}
