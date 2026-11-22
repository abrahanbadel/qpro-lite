using System.Net;
using System.Net.Http.Json;
using QproLite.Tests.Soporte;

namespace QproLite.Tests.Funcionales;

/// <summary>HU-05 · Descontar inventario y marcar agotados.</summary>
[Trait("Categoria", "Funcional")]
public class InventarioFuncionalTests
{
    private record ProductoDto(int Id, string Nombre, bool Agotado);
    private record CategoriaDto(string Nombre, List<ProductoDto> Productos);
    private record CartaDto(List<CategoriaDto> Categorias);
    private record ExistenciasDto(int Id, string Nombre, int Stock, bool Disponible, bool StockBajo);

    private static int IdDe(ApiFactory api, string nombre)
    {
        var id = 0;
        api.ConBaseDeDatos(db => id = db.Productos.Single(p => p.Nombre == nombre).Id);
        return id;
    }

    private static int StockDe(ApiFactory api, string nombre)
    {
        var stock = 0;
        api.ConBaseDeDatos(db => stock = db.Productos.Single(p => p.Nombre == nombre).Stock);
        return stock;
    }

    private static Task<HttpResponseMessage> Pedir(ApiFactory api, string producto, int cantidad) =>
        api.CreateClient().PostAsJsonAsync(
            "/api/pedidos", new { mesa = 3, items = new[] { new { productoId = IdDe(api, producto), cantidad } } });

    [Fact]
    public async Task CA_05_1_Un_pedido_descuenta_las_existencias_del_producto()
    {
        using var api = new ApiFactory();

        var respuesta = await Pedir(api, "Mote de queso", 2);

        Assert.Equal(HttpStatusCode.Created, respuesta.StatusCode);
        Assert.Equal(13, StockDe(api, "Mote de queso"));
    }

    [Fact]
    public async Task CA_05_2_Al_pedir_las_ultimas_unidades_el_producto_aparece_agotado_en_la_carta()
    {
        using var api = new ApiFactory();

        var respuesta = await Pedir(api, "Limonada de coco", 4);

        Assert.Equal(HttpStatusCode.Created, respuesta.StatusCode);
        var carta = await api.CreateClient().GetFromJsonAsync<CartaDto>("/api/carta?mesa=3");
        var limonada = carta!.Categorias.SelectMany(c => c.Productos).Single(p => p.Nombre == "Limonada de coco");
        Assert.True(limonada.Agotado);
    }

    [Fact]
    public async Task CA_05_3_Un_pedido_que_supera_las_existencias_se_rechaza_y_no_descuenta()
    {
        using var api = new ApiFactory();

        var respuesta = await Pedir(api, "Limonada de coco", 5);

        Assert.Equal(HttpStatusCode.Conflict, respuesta.StatusCode);
        Assert.Contains("Solo quedan 4 unidades de Limonada de coco", await respuesta.Content.ReadAsStringAsync());
        Assert.Equal(4, StockDe(api, "Limonada de coco"));
        api.ConBaseDeDatos(db => Assert.Empty(db.Pedidos));
    }

    [Fact]
    public async Task CA_05_4_El_inventario_alerta_los_productos_con_pocas_existencias()
    {
        using var api = new ApiFactory();

        var inventario = await api.CreateClient().GetFromJsonAsync<List<ExistenciasDto>>("/api/inventario");

        Assert.NotNull(inventario);
        Assert.True(inventario.Single(p => p.Nombre == "Limonada de coco").StockBajo);
        Assert.True(inventario.Single(p => p.Nombre == "Mojarra frita").StockBajo);
        Assert.False(inventario.Single(p => p.Nombre == "Mote de queso").StockBajo);
    }

    [Fact]
    public async Task Reponer_existencias_vuelve_a_ofrecer_un_producto_agotado()
    {
        using var api = new ApiFactory();
        var id = IdDe(api, "Mojarra frita");

        var respuesta = await api.CreateClient().PutAsJsonAsync($"/api/inventario/{id}", new { stock = 8 });

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);
        var producto = await respuesta.Content.ReadFromJsonAsync<ExistenciasDto>();
        Assert.Equal(8, producto!.Stock);
        Assert.True(producto.Disponible);
    }
}
