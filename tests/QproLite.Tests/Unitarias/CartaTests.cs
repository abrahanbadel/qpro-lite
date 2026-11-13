using QproLite.Api.Domain;

namespace QproLite.Tests.Unitarias;

[Trait("Categoria", "Unitaria")]
public class CartaTests
{
    private static readonly Producto[] Productos =
    [
        new() { Id = 1, Nombre = "Mote de queso", Categoria = "Platos fuertes", Precio = 22000 },
        new() { Id = 2, Nombre = "Jugo de corozo", Categoria = "Bebidas", Precio = 6000 },
        new() { Id = 3, Nombre = "Agua de panela", Categoria = "Bebidas", Precio = 4000, Disponible = false },
    ];

    [Fact]
    public void Agrupa_los_productos_por_categoria()
    {
        var carta = Carta.Organizar(Productos);

        Assert.Equal(2, carta.Count);
        Assert.Equal(2, carta.Single(c => c.Nombre == "Bebidas").Productos.Count);
        Assert.Single(carta.Single(c => c.Nombre == "Platos fuertes").Productos);
    }

    [Fact]
    public void Ordena_las_categorias_y_los_productos_alfabeticamente()
    {
        var carta = Carta.Organizar(Productos);

        Assert.Equal(["Bebidas", "Platos fuertes"], carta.Select(c => c.Nombre));
        Assert.Equal(["Agua de panela", "Jugo de corozo"], carta[0].Productos.Select(p => p.Nombre));
    }

    [Fact]
    public void Marca_como_agotado_el_producto_que_no_esta_disponible()
    {
        var bebidas = Carta.Organizar(Productos)[0].Productos;

        Assert.True(bebidas.Single(p => p.Nombre == "Agua de panela").Agotado);
        Assert.False(bebidas.Single(p => p.Nombre == "Jugo de corozo").Agotado);
    }
}
