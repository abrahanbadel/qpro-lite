using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QproLite.Api.Data;
using QproLite.Api.Domain;

namespace QproLite.Api.Controllers;

public record PagoDeCuenta(bool IncluirPropina = true);

[ApiController]
[Route("api/mesas/{mesa:int}")]
public class CajaController(AppDbContext db) : ControllerBase
{
    /// <summary>Cuenta abierta de la mesa: pedidos sin pagar, subtotal, propina sugerida y total.</summary>
    [HttpGet("cuenta")]
    public async Task<IActionResult> Cuenta(int mesa)
    {
        if (!Restaurante.MesaExiste(mesa))
        {
            return NotFound(new { error = "La mesa no existe" });
        }

        var pedidos = await PedidosSinPagar(mesa).AsNoTracking().ToListAsync();
        var cuenta = CalculadoraCuenta.Calcular(pedidos);
        return Ok(new
        {
            mesa,
            Pedidos = pedidos.Select(pedido => new
            {
                pedido.Id,
                pedido.Estado,
                Total = CalculadoraPedido.CalcularTotal(pedido.Items),
            }),
            cuenta.Subtotal,
            cuenta.PropinaSugerida,
            cuenta.Total,
        });
    }

    /// <summary>Registra el pago de la cuenta y deja la mesa sin consumos pendientes.</summary>
    [HttpPost("pago")]
    public async Task<IActionResult> Pagar(int mesa, PagoDeCuenta pago)
    {
        if (!Restaurante.MesaExiste(mesa))
        {
            return NotFound(new { error = "La mesa no existe" });
        }

        var pedidos = await PedidosSinPagar(mesa).ToListAsync();
        if (pedidos.Count == 0)
        {
            return Conflict(new { error = "La mesa no tiene consumos pendientes" });
        }

        var cuenta = CalculadoraCuenta.Calcular(pedidos, pago.IncluirPropina);
        var ahora = DateTime.UtcNow;
        foreach (var pedido in pedidos)
        {
            pedido.Pagado = true;
            pedido.PagadoEn = ahora;
        }

        await db.SaveChangesAsync();
        return Ok(new
        {
            mesa,
            PedidosPagados = pedidos.Count,
            cuenta.Subtotal,
            Propina = pago.IncluirPropina ? cuenta.PropinaSugerida : 0,
            TotalPagado = cuenta.Total,
        });
    }

    private IQueryable<Pedido> PedidosSinPagar(int mesa) =>
        db.Pedidos.Include(pedido => pedido.Items).Where(pedido => pedido.Mesa == mesa && !pedido.Pagado);
}
