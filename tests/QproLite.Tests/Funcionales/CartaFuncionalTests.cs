using System.Net;
using System.Net.Http.Json;
using QproLite.Tests.Soporte;

namespace QproLite.Tests.Funcionales;

/// <summary>HU-01 · Ver la carta de la mesa.</summary>
[Trait("Categoria", "Funcional")]
public class CartaFuncionalTests
{
    private record ProductoDto(int Id, string Nombre, decimal Precio, bool Agotado);
    private record CategoriaDto(string Nombre, List<ProductoDto> Productos);
    private record CartaDto(int Mesa, List<CategoriaDto> Categorias);

    [Fact]
    public async Task CA_01_1_La_carta_de_la_mesa_muestra_los_productos_agrupados_por_categoria()
    {
        using var api = new ApiFactory();

        var carta = await api.CreateClient().GetFromJsonAsync<CartaDto>("/api/carta?mesa=3");

        Assert.Equal(3, carta!.Mesa);
        Assert.Equal(["Bebidas", "Entradas", "Platos fuertes"], carta.Categorias.Select(c => c.Nombre));
        var mote = carta.Categorias[2].Productos.Single(p => p.Nombre == "Mote de queso");
        Assert.Equal(22000, mote.Precio);
    }

    [Fact]
    public async Task CA_01_2_Un_producto_no_disponible_aparece_marcado_como_agotado()
    {
        using var api = new ApiFactory();

        var carta = await api.CreateClient().GetFromJsonAsync<CartaDto>("/api/carta?mesa=3");

        var productos = carta!.Categorias.SelectMany(c => c.Productos).ToList();
        Assert.True(productos.Single(p => p.Nombre == "Mojarra frita").Agotado);
        Assert.False(productos.Single(p => p.Nombre == "Mote de queso").Agotado);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(11)]
    public async Task CA_01_3_Una_mesa_que_no_existe_responde_que_la_mesa_no_existe(int mesa)
    {
        using var api = new ApiFactory();

        var respuesta = await api.CreateClient().GetAsync($"/api/carta?mesa={mesa}");

        Assert.Equal(HttpStatusCode.NotFound, respuesta.StatusCode);
        Assert.Contains("La mesa no existe", await respuesta.Content.ReadAsStringAsync());
    }
}
