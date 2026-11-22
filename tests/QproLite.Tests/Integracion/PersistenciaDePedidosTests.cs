using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using QproLite.Api.Domain;
using QproLite.Tests.Soporte;

namespace QproLite.Tests.Integracion;

/// <summary>Integración API + Entity Framework Core + SQLite: lo que entra por HTTP queda guardado en la base de datos.</summary>
[Trait("Categoria", "Integracion")]
public class PersistenciaDePedidosTests
{
    [Fact]
    public async Task Un_pedido_registrado_por_la_api_queda_guardado_con_sus_items_y_el_precio_del_momento()
    {
        using var api = new ApiFactory();
        var productoId = 0;
        api.ConBaseDeDatos(db => productoId = db.Productos.Single(p => p.Nombre == "Mote de queso").Id);

        var respuesta = await api.CreateClient().PostAsJsonAsync(
            "/api/pedidos", new { mesa = 5, items = new[] { new { productoId, cantidad = 2 } } });
        respuesta.EnsureSuccessStatusCode();

        api.ConBaseDeDatos(db =>
        {
            var pedido = db.Pedidos.Include(p => p.Items).Single();
            Assert.Equal(5, pedido.Mesa);
            Assert.Equal(EstadoPedido.Pendiente, pedido.Estado);
            Assert.False(pedido.Pagado);
            var item = Assert.Single(pedido.Items);
            Assert.Equal("Mote de queso", item.NombreProducto);
            Assert.Equal(22000, item.PrecioUnitario);
            Assert.Equal(2, item.Cantidad);
        });
    }

    [Fact]
    public void Un_cambio_de_existencias_obsoleto_no_guarda_el_pedido()
    {
        using var api = new ApiFactory();
        api.ConBaseDeDatos(db =>
        {
            var producto = db.Productos.Single(p => p.Nombre == "Mote de queso");
            api.ConBaseDeDatos(otraSolicitud =>
            {
                var actualizado = otraSolicitud.Productos.Single(p => p.Id == producto.Id);
                Inventario.Reponer(actualizado, 1);
                otraSolicitud.SaveChanges();
            });

            Inventario.Descontar(producto, 2);
            db.Pedidos.Add(new Pedido { Mesa = 3, CreadoEn = DateTime.UtcNow });
            Assert.Throws<DbUpdateConcurrencyException>(() => db.SaveChanges());
        });

        api.ConBaseDeDatos(db =>
        {
            Assert.Equal(1, db.Productos.Single(p => p.Nombre == "Mote de queso").Stock);
            Assert.Empty(db.Pedidos);
        });
    }
}
