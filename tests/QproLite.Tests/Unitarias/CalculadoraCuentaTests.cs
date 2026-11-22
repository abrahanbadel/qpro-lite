using QproLite.Api.Domain;

namespace QproLite.Tests.Unitarias;

[Trait("Categoria", "Unitaria")]
public class CalculadoraCuentaTests
{
    private static Pedido PedidoDe(decimal precio, int cantidad) =>
        new() { Items = [new PedidoItem { PrecioUnitario = precio, Cantidad = cantidad }] };

    [Fact]
    public void El_subtotal_suma_todos_los_pedidos_de_la_mesa()
    {
        var cuenta = CalculadoraCuenta.Calcular([PedidoDe(22000, 2), PedidoDe(6000, 1)]);

        Assert.Equal(50000, cuenta.Subtotal);
    }

    [Fact]
    public void La_propina_sugerida_es_el_diez_por_ciento_del_subtotal()
    {
        var cuenta = CalculadoraCuenta.Calcular([PedidoDe(22000, 2), PedidoDe(6000, 1)]);

        Assert.Equal(5000, cuenta.PropinaSugerida);
        Assert.Equal(55000, cuenta.Total);
    }

    [Fact]
    public void Sin_propina_el_total_es_igual_al_subtotal()
    {
        var cuenta = CalculadoraCuenta.Calcular([PedidoDe(22000, 2)], incluirPropina: false);

        Assert.Equal(44000, cuenta.Total);
    }

    [Fact]
    public void Una_mesa_sin_pedidos_tiene_la_cuenta_en_cero()
    {
        var cuenta = CalculadoraCuenta.Calcular([]);

        Assert.Equal(new Cuenta(0, 0, 0), cuenta);
    }
}
