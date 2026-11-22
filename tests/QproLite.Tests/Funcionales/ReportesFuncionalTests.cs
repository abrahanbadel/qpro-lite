using System.Net;
using System.Net.Http.Json;
using QproLite.Api.Domain;
using QproLite.Tests.Soporte;

namespace QproLite.Tests.Funcionales;

/// <summary>HU-06 · Consultar el reporte de ventas del día.</summary>
[Trait("Categoria", "Funcional")]
public class ReportesFuncionalTests
{
    private record ResumenDto(decimal TotalVendido, int NumeroDePedidos, decimal TicketPromedio);

    private record ErrorDto(string Error);

    private static readonly DateTime Mediodia = new(2026, 11, 20, 17, 0, 0, DateTimeKind.Utc);

    private static void CrearPedido(ApiFactory api, decimal valor, bool pagado)
    {
        api.ConBaseDeDatos(db =>
        {
            db.Pedidos.Add(new Pedido
            {
                Mesa = 3,
                Estado = EstadoPedido.Entregado,
                CreadoEn = Mediodia,
                Pagado = pagado,
                PagadoEn = pagado ? Mediodia : null,
                Items = [new PedidoItem { ProductoId = 1, NombreProducto = "Mote de queso", Cantidad = 1, PrecioUnitario = valor }],
            });
            db.SaveChanges();
        });
    }

    [Fact]
    public async Task CA_06_1_El_reporte_muestra_total_vendido_numero_de_pedidos_y_ticket_promedio()
    {
        using var api = new ApiFactory();
        var cliente = api.CreateClient();
        CrearPedido(api, 30000, pagado: true);
        CrearPedido(api, 20000, pagado: true);
        CrearPedido(api, 50000, pagado: false);

        var resumen = await cliente.GetFromJsonAsync<ResumenDto>("/api/reportes/ventas?fecha=2026-11-20");

        Assert.Equal(50000, resumen!.TotalVendido);
        Assert.Equal(2, resumen.NumeroDePedidos);
        Assert.Equal(25000, resumen.TicketPromedio);
    }

    [Fact]
    public async Task CA_06_2_Un_dia_sin_ventas_muestra_los_valores_en_cero()
    {
        using var api = new ApiFactory();

        var resumen = await api.CreateClient().GetFromJsonAsync<ResumenDto>("/api/reportes/ventas?fecha=2026-11-19");

        Assert.Equal(new ResumenDto(0, 0, 0), resumen);
    }

    [Fact]
    public async Task CA_06_3_Una_fecha_con_formato_invalido_se_rechaza()
    {
        using var api = new ApiFactory();

        var respuesta = await api.CreateClient().GetAsync("/api/reportes/ventas?fecha=20-11-2026");
        var error = await respuesta.Content.ReadFromJsonAsync<ErrorDto>();

        Assert.Equal(HttpStatusCode.BadRequest, respuesta.StatusCode);
        Assert.Equal("La fecha debe tener el formato AAAA-MM-DD", error!.Error);
    }
}
