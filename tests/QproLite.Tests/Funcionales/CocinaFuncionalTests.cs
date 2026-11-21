using System.Net;
using System.Net.Http.Json;
using QproLite.Api.Domain;
using QproLite.Tests.Soporte;

namespace QproLite.Tests.Funcionales;

/// <summary>HU-03 · Gestionar las comandas en cocina.</summary>
[Trait("Categoria", "Funcional")]
public class CocinaFuncionalTests
{
    private record ComandaDto(int Id, int Mesa, string Estado);

    private static readonly DateTime Mediodia = new(2026, 11, 13, 17, 0, 0, DateTimeKind.Utc);

    private static int CrearPedido(ApiFactory api, int mesa, EstadoPedido estado, DateTime creadoEn)
    {
        var pedido = new Pedido
        {
            Mesa = mesa,
            Estado = estado,
            CreadoEn = creadoEn,
            Items = [new PedidoItem { ProductoId = 1, NombreProducto = "Mote de queso", Cantidad = 1, PrecioUnitario = 22000 }],
        };
        api.ConBaseDeDatos(db =>
        {
            db.Pedidos.Add(pedido);
            db.SaveChanges();
        });
        return pedido.Id;
    }

    [Fact]
    public async Task CA_03_1_Las_comandas_se_muestran_de_la_mas_antigua_a_la_mas_reciente()
    {
        using var api = new ApiFactory();
        var reciente = CrearPedido(api, mesa: 2, EstadoPedido.Pendiente, Mediodia.AddMinutes(10));
        var antigua = CrearPedido(api, mesa: 7, EstadoPedido.EnPreparacion, Mediodia);

        var comandas = await api.CreateClient().GetFromJsonAsync<List<ComandaDto>>("/api/cocina/comandas");

        Assert.Equal([antigua, reciente], comandas!.Select(c => c.Id));
    }

    [Fact]
    public async Task CA_03_2_Un_pedido_pendiente_puede_pasar_a_preparacion()
    {
        using var api = new ApiFactory();
        var id = CrearPedido(api, mesa: 2, EstadoPedido.Pendiente, Mediodia);

        var respuesta = await api.CreateClient().PatchAsJsonAsync($"/api/pedidos/{id}/estado", new { estado = "EnPreparacion" });

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);
        api.ConBaseDeDatos(db => Assert.Equal(EstadoPedido.EnPreparacion, db.Pedidos.Single(p => p.Id == id).Estado));
    }

    [Fact]
    public async Task CA_03_3_Un_pedido_pendiente_no_puede_pasar_directamente_a_listo()
    {
        using var api = new ApiFactory();
        var id = CrearPedido(api, mesa: 2, EstadoPedido.Pendiente, Mediodia);

        var respuesta = await api.CreateClient().PatchAsJsonAsync($"/api/pedidos/{id}/estado", new { estado = "Listo" });

        Assert.Equal(HttpStatusCode.Conflict, respuesta.StatusCode);
        Assert.Contains("No se puede pasar de Pendiente a Listo", await respuesta.Content.ReadAsStringAsync());
        api.ConBaseDeDatos(db => Assert.Equal(EstadoPedido.Pendiente, db.Pedidos.Single(p => p.Id == id).Estado));
    }

    [Fact]
    public async Task CA_03_4_Un_pedido_entregado_ya_no_aparece_en_cocina()
    {
        using var api = new ApiFactory();
        CrearPedido(api, mesa: 4, EstadoPedido.Entregado, Mediodia);
        var listo = CrearPedido(api, mesa: 5, EstadoPedido.Listo, Mediodia);

        var comandas = await api.CreateClient().GetFromJsonAsync<List<ComandaDto>>("/api/cocina/comandas");

        Assert.Equal([listo], comandas!.Select(c => c.Id));
    }
}
