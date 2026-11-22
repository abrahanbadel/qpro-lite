using QproLite.Api.Domain;

namespace QproLite.Tests.Unitarias;

[Trait("Categoria", "Unitaria")]
public class ReporteDeVentasTests
{
    private static readonly DateOnly Fecha = new(2026, 11, 20);
    private static readonly DateTime Mediodia = new(2026, 11, 20, 17, 0, 0, DateTimeKind.Utc);

    private static Pedido PedidoDe(decimal valor, bool pagado = true, DateTime? pagadoEn = null) => new()
    {
        Pagado = pagado,
        PagadoEn = pagado ? pagadoEn ?? Mediodia : null,
        Items = [new PedidoItem { NombreProducto = "Mote de queso", Cantidad = 1, PrecioUnitario = valor }],
    };

    [Fact]
    public void Suma_el_total_y_cuenta_los_pedidos_pagados_de_la_fecha()
    {
        var resumen = ReporteDeVentas.Resumir([PedidoDe(30000), PedidoDe(20000)], Fecha);

        Assert.Equal(50000, resumen.TotalVendido);
        Assert.Equal(2, resumen.NumeroDePedidos);
    }

    [Fact]
    public void Calcula_el_ticket_promedio()
    {
        var resumen = ReporteDeVentas.Resumir([PedidoDe(30000), PedidoDe(20000)], Fecha);

        Assert.Equal(25000, resumen.TicketPromedio);
    }

    [Fact]
    public void Ignora_los_pedidos_sin_pagar_y_los_de_otra_fecha()
    {
        var pedidos = new[] { PedidoDe(10000), PedidoDe(50000, pagado: false), PedidoDe(70000, pagadoEn: Mediodia.AddDays(1)) };

        var resumen = ReporteDeVentas.Resumir(pedidos, Fecha);

        Assert.Equal(10000, resumen.TotalVendido);
        Assert.Equal(1, resumen.NumeroDePedidos);
    }

    [Fact]
    public void Un_dia_sin_ventas_da_valores_en_cero()
    {
        var resumen = ReporteDeVentas.Resumir([], Fecha);

        Assert.Equal(0, resumen.TotalVendido);
        Assert.Equal(0, resumen.NumeroDePedidos);
        Assert.Equal(0, resumen.TicketPromedio);
    }
}
