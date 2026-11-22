using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QproLite.Api.Data;
using QproLite.Api.Domain;

namespace QproLite.Api.Controllers;

[ApiController]
[Route("api/reportes")]
public class ReportesController(AppDbContext db) : ControllerBase
{
    private const string FormatoDeFecha = "yyyy-MM-dd";

    /// <summary>Resumen de ventas de un día (por defecto, hoy): total vendido, pedidos y ticket promedio.</summary>
    [HttpGet("ventas")]
    public async Task<IActionResult> Ventas([FromQuery] string? fecha)
    {
        var dia = DateOnly.FromDateTime(DateTime.UtcNow);
        if (!string.IsNullOrWhiteSpace(fecha)
            && !DateOnly.TryParseExact(fecha, FormatoDeFecha, CultureInfo.InvariantCulture, DateTimeStyles.None, out dia))
        {
            return BadRequest(new { error = "La fecha debe tener el formato AAAA-MM-DD" });
        }

        var inicio = dia.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        var fin = inicio.AddDays(1);
        var pedidos = await db.Pedidos
            .AsNoTracking()
            .Include(pedido => pedido.Items)
            .Where(pedido => pedido.Pagado && pedido.PagadoEn >= inicio && pedido.PagadoEn < fin)
            .ToListAsync();

        var resumen = ReporteDeVentas.Resumir(pedidos);
        return Ok(new
        {
            Fecha = dia.ToString(FormatoDeFecha, CultureInfo.InvariantCulture),
            resumen.TotalVendido,
            resumen.NumeroDePedidos,
            resumen.TicketPromedio,
        });
    }
}
