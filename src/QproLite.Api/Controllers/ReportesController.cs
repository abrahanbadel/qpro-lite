using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QproLite.Api.Data;
using QproLite.Api.Domain;

namespace QproLite.Api.Controllers;

[ApiController]
public class ReportesController(AppDbContext db) : ControllerBase
{
    /// <summary>Reporte de ventas de una fecha (AAAA-MM-DD); sin fecha, el del día actual.</summary>
    [HttpGet("api/reportes/ventas")]
    public async Task<IActionResult> Ventas([FromQuery] string? fecha)
    {
        var dia = DateOnly.FromDateTime(DateTime.UtcNow);
        if (fecha is not null
            && !DateOnly.TryParseExact(fecha, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dia))
        {
            return BadRequest(new { error = "La fecha debe tener el formato AAAA-MM-DD" });
        }

        var pagados = await db.Pedidos
            .AsNoTracking()
            .Include(pedido => pedido.Items)
            .Where(pedido => pedido.Pagado)
            .ToListAsync();

        return Ok(ReporteDeVentas.Resumir(pagados, dia));
    }
}
