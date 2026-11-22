using System.Net;
using System.Net.Http.Json;
using QproLite.Tests.Soporte;

namespace QproLite.Tests.Funcionales;

/// <summary>HU-02 · Registrar un pedido desde la mesa.</summary>
[Trait("Categoria", "Funcional")]
public class PedidosFuncionalTests
{
    private record PedidoDto(int Id, int Mesa, string Estado, decimal Total);

    private static int IdDe(ApiFactory api, string nombre)
    {
        var id = 0;
        api.ConBaseDeDatos(db => id = db.Productos.Single(p => p.Nombre == nombre).Id);
        return id;
    }

    [Fact]
    public async Task CA_02_1_Un_pedido_valido_queda_pendiente_con_el_total_calculado()
    {
        using var api = new ApiFactory();
        var pedido = new
        {
            mesa = 3,
            items = new[]
            {
                new { productoId = IdDe(api, "Mote de queso"), cantidad = 2 },
                new { productoId = IdDe(api, "Jugo de corozo"), cantidad = 3 },
            },
        };

        var respuesta = await api.CreateClient().PostAsJsonAsync("/api/pedidos", pedido);

        Assert.Equal(HttpStatusCode.Created, respuesta.StatusCode);
        var creado = await respuesta.Content.ReadFromJsonAsync<PedidoDto>();
        Assert.Equal(3, creado!.Mesa);
        Assert.Equal("Pendiente", creado.Estado);
        Assert.Equal(62000, creado.Total);
    }

    [Fact]
    public async Task CA_02_2_Un_pedido_sin_productos_se_rechaza()
    {
        using var api = new ApiFactory();

        var respuesta = await api.CreateClient().PostAsJsonAsync("/api/pedidos", new { mesa = 3, items = Array.Empty<object>() });

        Assert.Equal(HttpStatusCode.BadRequest, respuesta.StatusCode);
        Assert.Contains("El pedido debe tener al menos un producto", await respuesta.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task CA_02_3_Un_producto_agotado_no_se_puede_pedir()
    {
        using var api = new ApiFactory();
        var pedido = new { mesa = 3, items = new[] { new { productoId = IdDe(api, "Mojarra frita"), cantidad = 1 } } };

        var respuesta = await api.CreateClient().PostAsJsonAsync("/api/pedidos", pedido);

        Assert.Equal(HttpStatusCode.Conflict, respuesta.StatusCode);
        Assert.Contains("agotado", await respuesta.Content.ReadAsStringAsync());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-2)]
    public async Task CA_02_4_Una_cantidad_menor_que_uno_se_rechaza(int cantidad)
    {
        using var api = new ApiFactory();
        var pedido = new { mesa = 3, items = new[] { new { productoId = IdDe(api, "Mote de queso"), cantidad } } };

        var respuesta = await api.CreateClient().PostAsJsonAsync("/api/pedidos", pedido);

        Assert.Equal(HttpStatusCode.BadRequest, respuesta.StatusCode);
        Assert.Contains("La cantidad debe ser mayor que cero", await respuesta.Content.ReadAsStringAsync());
    }
}
