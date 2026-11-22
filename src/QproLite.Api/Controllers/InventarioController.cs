using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QproLite.Api.Data;
using QproLite.Api.Domain;

namespace QproLite.Api.Controllers;

public record AjusteDeExistencias(int Stock);

[ApiController]
[Route("api/inventario")]
public class InventarioController(AppDbContext db) : ControllerBase
{
    /// <summary>Existencias de cada producto, con alerta cuando quedan pocas unidades.</summary>
    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var productos = await db.Productos.AsNoTracking().OrderBy(producto => producto.Nombre).ToListAsync();
        return Ok(productos.Select(Respuesta));
    }

    /// <summary>Repone (o corrige) las existencias de un producto.</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Reponer(int id, AjusteDeExistencias ajuste)
    {
        if (ajuste.Stock < 0)
        {
            return BadRequest(new { error = "Las existencias no pueden ser negativas" });
        }

        var producto = await db.Productos.FindAsync(id);
        if (producto is null)
        {
            return NotFound(new { error = "El producto no existe" });
        }

        Inventario.Reponer(producto, ajuste.Stock);
        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict(new { error = "Las existencias cambiaron. Consulta el inventario antes de reponer." });
        }

        return Ok(Respuesta(producto));
    }

    private static object Respuesta(Producto producto) => new
    {
        producto.Id,
        producto.Nombre,
        producto.Categoria,
        producto.Stock,
        producto.Disponible,
        StockBajo = Inventario.TieneStockBajo(producto),
    };
}
