using QproLite.Api.Domain;

namespace QproLite.Tests.Unitarias;

[Trait("Categoria", "Unitaria")]
public class ReporteDeVentasTests
{
    private static Pedido PedidoDe(decimal total) =>
        new() { Pagado = true, Items = [new PedidoItem { PrecioUnitario = total, Cantidad = 1 }] };

    [Fact]
    public void Resume_el_total_vendido_y_el_numero_de_pedidos()
    {
        var resumen = ReporteDeVentas.Resumir([PedidoDe(44000), PedidoDe(6000), PedidoDe(28000)]);

        Assert.Equal(78000, resumen.TotalVendido);
        Assert.Equal(3, resumen.NumeroDePedidos);
    }

    [Fact]
    public void El_ticket_promedio_es_el_total_dividido_entre_los_pedidos()
    {
        var resumen = ReporteDeVentas.Resumir([PedidoDe(44000), PedidoDe(6000), PedidoDe(28000)]);

        Assert.Equal(26000, resumen.TicketPromedio);
    }

    [Fact]
    public void El_ticket_promedio_se_redondea_al_peso()
    {
        var resumen = ReporteDeVentas.Resumir([PedidoDe(10000), PedidoDe(10000), PedidoDe(5000)]);

        Assert.Equal(8333, resumen.TicketPromedio);
    }

    [Fact]
    public void Un_dia_sin_ventas_tiene_el_resumen_en_cero()
    {
        var resumen = ReporteDeVentas.Resumir([]);

        Assert.Equal(new ResumenDeVentas(0, 0, 0), resumen);
    }
}
