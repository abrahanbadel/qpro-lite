using QproLite.Api.Domain;

namespace QproLite.Tests.Unitarias;

[Trait("Categoria", "Unitaria")]
public class InventarioTests
{
    [Fact]
    public void Descontar_reduce_las_existencias_del_producto()
    {
        var producto = new Producto { Nombre = "Mote de queso", Stock = 15 };

        Inventario.Descontar(producto, 2);

        Assert.Equal(13, producto.Stock);
        Assert.True(producto.Disponible);
    }

    [Fact]
    public void Al_llegar_a_cero_el_producto_queda_agotado()
    {
        var producto = new Producto { Nombre = "Limonada de coco", Stock = 4 };

        Inventario.Descontar(producto, 4);

        Assert.Equal(0, producto.Stock);
        Assert.False(producto.Disponible);
    }

    [Fact]
    public void No_se_puede_descontar_mas_de_lo_que_hay()
    {
        var producto = new Producto { Nombre = "Limonada de coco", Stock = 4 };

        Assert.False(Inventario.HayExistencias(producto, 5));
        Assert.Throws<InvalidOperationException>(() => Inventario.Descontar(producto, 5));
        Assert.Equal(4, producto.Stock);
    }

    [Theory]
    [InlineData(5, true)]
    [InlineData(0, true)]
    [InlineData(6, false)]
    public void Hay_alerta_de_existencias_bajas_con_cinco_unidades_o_menos(int stock, bool alerta)
    {
        Assert.Equal(alerta, Inventario.TieneStockBajo(new Producto { Stock = stock }));
    }

    [Fact]
    public void Reponer_existencias_vuelve_a_dejar_disponible_el_producto()
    {
        var producto = new Producto { Nombre = "Mojarra frita", Stock = 0, Disponible = false };

        Inventario.Reponer(producto, 10);

        Assert.Equal(10, producto.Stock);
        Assert.True(producto.Disponible);
    }
}
