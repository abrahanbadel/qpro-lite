using System.Net;
using System.Net.Http.Json;
using QproLite.Api.Domain;
using QproLite.Tests.Soporte;

namespace QproLite.Tests.Funcionales;

/// <summary>HU-06 · Consultar el reporte de ventas del día.</summary>
[Trait("Categoria", "Funcional")]
public class ReportesFuncionalTests
{
    private record ReporteDto(string Fecha, decimal TotalVendido, int NumeroDePedidos, decimal TicketPromedio);

    private static void RegistrarVenta(ApiFactory api, decimal total, DateTime pagadoEn) =>
        api.ConBaseDeDatos(db =>
        {
            db.Pedidos.Add(new Pedido
            {
                Mesa = 1,
                Estado = EstadoPedido.Entregado,
                CreadoEn = pagadoEn.AddMinutes(-40),
                Pagado = true,
                PagadoEn = pagadoEn,
                Items = [new PedidoItem { ProductoId = 1, NombreProducto = "Mote de queso", Cantidad = 1, PrecioUnitario = total }],
            });
            db.SaveChanges();
        });

    [Fact]
    public async Task CA_06_1_El_reporte_muestra_total_vendido_numero_de_pedidos_y_ticket_promedio()
    {
        using var api = new ApiFactory();
        RegistrarVenta(api, 44000, new DateTime(2026, 11, 13, 17, 0, 0, DateTimeKind.Utc));
        RegistrarVenta(api, 28000, new DateTime(2026, 11, 13, 18, 30, 0, DateTimeKind.Utc));
        RegistrarVenta(api, 99000, new DateTime(2026, 11, 12, 18, 0, 0, DateTimeKind.Utc));

        var reporte = await api.CreateClient().GetFromJsonAsync<ReporteDto>("/api/reportes/ventas?fecha=2026-11-13");

        Assert.Equal("2026-11-13", reporte!.Fecha);
        Assert.Equal(72000, reporte.TotalVendido);
        Assert.Equal(2, reporte.NumeroDePedidos);
        Assert.Equal(36000, reporte.TicketPromedio);
    }

    [Fact]
    public async Task CA_06_2_Un_dia_sin_ventas_muestra_los_valores_en_cero()
    {
        using var api = new ApiFactory();

        var respuesta = await api.CreateClient().GetAsync("/api/reportes/ventas?fecha=2026-11-13");

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);
        var reporte = await respuesta.Content.ReadFromJsonAsync<ReporteDto>();
        Assert.Equal(0, reporte!.TotalVendido);
        Assert.Equal(0, reporte.NumeroDePedidos);
        Assert.Equal(0, reporte.TicketPromedio);
    }

    [Theory]
    [InlineData("13/11/2026")]
    [InlineData("ayer")]
    public async Task CA_06_3_Una_fecha_con_formato_invalido_se_rechaza(string fecha)
    {
        using var api = new ApiFactory();

        var respuesta = await api.CreateClient().GetAsync($"/api/reportes/ventas?fecha={fecha}");

        Assert.Equal(HttpStatusCode.BadRequest, respuesta.StatusCode);
        Assert.Contains("La fecha debe tener el formato AAAA-MM-DD", await respuesta.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task Una_venta_de_la_noche_cuenta_en_el_dia_de_Colombia_y_no_en_el_dia_siguiente()
    {
        using var api = new ApiFactory();
        // 7:30 p. m. del 13 de noviembre en Colombia (UTC-5) = 00:30 del 14 de noviembre en UTC.
        RegistrarVenta(api, 50000, new DateTime(2026, 11, 14, 0, 30, 0, DateTimeKind.Utc));
        var http = api.CreateClient();

        var del13 = await http.GetFromJsonAsync<ReporteDto>("/api/reportes/ventas?fecha=2026-11-13");
        var del14 = await http.GetFromJsonAsync<ReporteDto>("/api/reportes/ventas?fecha=2026-11-14");

        Assert.Equal(50000, del13!.TotalVendido);
        Assert.Equal(0, del14!.TotalVendido);
    }

    [Fact]
    public async Task Sin_fecha_el_reporte_corresponde_al_dia_de_hoy()
    {
        using var api = new ApiFactory();

        var respuesta = await api.CreateClient().GetAsync("/api/reportes/ventas");

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);
    }
}
