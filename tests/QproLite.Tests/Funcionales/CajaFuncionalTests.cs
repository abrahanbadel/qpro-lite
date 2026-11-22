using System.Net;
using System.Net.Http.Json;
using QproLite.Tests.Soporte;

namespace QproLite.Tests.Funcionales;

/// <summary>HU-04 · Cobrar la cuenta de la mesa.</summary>
[Trait("Categoria", "Funcional")]
public class CajaFuncionalTests
{
    private record CuentaDto(int Mesa, decimal Subtotal, decimal PropinaSugerida, decimal Total);
    private record PagoDto(int PedidosPagados, decimal Propina, decimal TotalPagado);

    private static async Task Pedir(ApiFactory api, HttpClient http, int mesa, string producto, int cantidad)
    {
        var productoId = 0;
        api.ConBaseDeDatos(db => productoId = db.Productos.Single(p => p.Nombre == producto).Id);
        var respuesta = await http.PostAsJsonAsync("/api/pedidos", new { mesa, items = new[] { new { productoId, cantidad } } });
        respuesta.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task CA_04_1_La_cuenta_muestra_subtotal_propina_sugerida_y_total()
    {
        using var api = new ApiFactory();
        var http = api.CreateClient();
        await Pedir(api, http, mesa: 3, "Mote de queso", 2);
        await Pedir(api, http, mesa: 3, "Jugo de corozo", 1);
        await Pedir(api, http, mesa: 8, "Patacón con suero", 1);

        var cuenta = await http.GetFromJsonAsync<CuentaDto>("/api/mesas/3/cuenta");

        Assert.Equal(50000, cuenta!.Subtotal);
        Assert.Equal(5000, cuenta.PropinaSugerida);
        Assert.Equal(55000, cuenta.Total);
    }

    [Fact]
    public async Task CA_04_2_Al_pagar_los_pedidos_quedan_pagados_y_la_cuenta_vuelve_a_cero()
    {
        using var api = new ApiFactory();
        var http = api.CreateClient();
        await Pedir(api, http, mesa: 3, "Mote de queso", 2);

        var respuesta = await http.PostAsJsonAsync("/api/mesas/3/pago", new { incluirPropina = true });

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);
        var pago = await respuesta.Content.ReadFromJsonAsync<PagoDto>();
        Assert.Equal(1, pago!.PedidosPagados);
        Assert.Equal(48400, pago.TotalPagado);
        api.ConBaseDeDatos(db => Assert.All(db.Pedidos, pedido => Assert.True(pedido.Pagado && pedido.PagadoEn != null)));
        var cuenta = await http.GetFromJsonAsync<CuentaDto>("/api/mesas/3/cuenta");
        Assert.Equal(0, cuenta!.Total);
    }

    [Fact]
    public async Task CA_04_3_No_se_puede_cobrar_una_mesa_sin_consumos_pendientes()
    {
        using var api = new ApiFactory();

        var respuesta = await api.CreateClient().PostAsJsonAsync("/api/mesas/3/pago", new { incluirPropina = true });

        Assert.Equal(HttpStatusCode.Conflict, respuesta.StatusCode);
        Assert.Contains("La mesa no tiene consumos pendientes", await respuesta.Content.ReadAsStringAsync());
    }
}
