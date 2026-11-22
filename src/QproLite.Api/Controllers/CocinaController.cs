using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QproLite.Api.Data;
using QproLite.Api.Domain;

namespace QproLite.Api.Controllers;

public record CambioDeEstado(EstadoPedido Estado);

[ApiController]
public class CocinaController(AppDbContext db) : ControllerBase
{
    /// <summary>Comandas sin entregar, de la más antigua a la más reciente.</summary>
    [HttpGet("api/cocina/comandas")]
    public async Task<IActionResult> Comandas()
    {
        var pedidos = await db.Pedidos
            .AsNoTracking()
            .Include(pedido => pedido.Items)
            .Where(pedido => pedido.Estado != EstadoPedido.Entregado)
            .OrderBy(pedido => pedido.CreadoEn)
            .ThenBy(pedido => pedido.Id)
            .ToListAsync();

        return Ok(pedidos.Select(pedido => new
        {
            pedido.Id,
            pedido.Mesa,
            pedido.Estado,
            pedido.CreadoEn,
            Siguiente = FlujoDeEstados.Siguiente(pedido.Estado),
            Items = pedido.Items.Select(item => new { Nombre = item.NombreProducto, item.Cantidad }),
        }));
    }

    /// <summary>Avanza el estado de un pedido respetando el flujo de cocina.</summary>
    [HttpPatch("api/pedidos/{id:int}/estado")]
    public async Task<IActionResult> CambiarEstado(int id, CambioDeEstado cambio)
    {
        var pedido = await db.Pedidos.FindAsync(id);
        if (pedido is null)
        {
            return NotFound(new { error = "El pedido no existe" });
        }

        if (!FlujoDeEstados.PuedeCambiar(pedido.Estado, cambio.Estado))
        {
            return Conflict(new { error = $"No se puede pasar de {pedido.Estado} a {cambio.Estado}" });
        }

        pedido.Estado = cambio.Estado;
        await db.SaveChangesAsync();
        return Ok(new { pedido.Id, pedido.Estado });
    }
}
