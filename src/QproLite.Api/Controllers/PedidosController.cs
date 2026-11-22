using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QproLite.Api.Data;
using QproLite.Api.Domain;

namespace QproLite.Api.Controllers;

public record ItemSolicitado(int ProductoId, int Cantidad);

public record NuevoPedido(int Mesa, List<ItemSolicitado>? Items);

[ApiController]
[Route("api/pedidos")]
public class PedidosController(AppDbContext db) : ControllerBase
{
    /// <summary>Registra el pedido que el comensal envía desde su mesa.</summary>
    [HttpPost]
    public async Task<IActionResult> Crear(NuevoPedido solicitud)
    {
        if (!Restaurante.MesaExiste(solicitud.Mesa))
        {
            return NotFound(new { error = "La mesa no existe" });
        }

        if (solicitud.Items is null || solicitud.Items.Count == 0)
        {
            return BadRequest(new { error = "El pedido debe tener al menos un producto" });
        }

        if (solicitud.Items.Any(item => item.Cantidad < 1))
        {
            return BadRequest(new { error = "La cantidad debe ser mayor que cero" });
        }

        var pedido = new Pedido { Mesa = solicitud.Mesa, CreadoEn = DateTime.UtcNow };
        foreach (var item in solicitud.Items)
        {
            var producto = await db.Productos.FindAsync(item.ProductoId);
            if (producto is null)
            {
                return NotFound(new { error = $"El producto {item.ProductoId} no existe" });
            }

            if (!producto.Disponible)
            {
                return Conflict(new { error = $"{producto.Nombre} está agotado" });
            }

            // HU-05: cada pedido descuenta existencias; si no alcanzan, se rechaza y nada se guarda.
            if (!Inventario.HayExistencias(producto, item.Cantidad))
            {
                return Conflict(new { error = Inventario.MensajeSinExistencias(producto) });
            }

            Inventario.Descontar(producto, item.Cantidad);

            pedido.Items.Add(new PedidoItem
            {
                ProductoId = producto.Id,
                NombreProducto = producto.Nombre,
                Cantidad = item.Cantidad,
                PrecioUnitario = producto.Precio,
            });
        }

        db.Pedidos.Add(pedido);
        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict(new { error = "Las existencias cambiaron. Revisa la carta antes de volver a pedir." });
        }

        return CreatedAtAction(nameof(Obtener), new { id = pedido.Id }, Respuesta(pedido));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Obtener(int id)
    {
        var pedido = await db.Pedidos.AsNoTracking().Include(p => p.Items).FirstOrDefaultAsync(p => p.Id == id);
        return pedido is null ? NotFound(new { error = "El pedido no existe" }) : Ok(Respuesta(pedido));
    }

    private static object Respuesta(Pedido pedido) => new
    {
        pedido.Id,
        pedido.Mesa,
        pedido.Estado,
        pedido.CreadoEn,
        Total = CalculadoraPedido.CalcularTotal(pedido.Items),
        Items = pedido.Items.Select(item => new
        {
            item.ProductoId,
            Nombre = item.NombreProducto,
            item.Cantidad,
            item.PrecioUnitario,
            item.Subtotal,
        }),
    };
}
