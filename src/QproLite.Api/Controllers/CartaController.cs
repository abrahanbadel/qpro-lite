using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QproLite.Api.Data;
using QproLite.Api.Domain;

namespace QproLite.Api.Controllers;

[ApiController]
[Route("api/carta")]
public class CartaController(AppDbContext db) : ControllerBase
{
    /// <summary>Carta que ve el comensal al abrir el enlace (código QR) de su mesa.</summary>
    [HttpGet]
    public async Task<IActionResult> Obtener([FromQuery] int mesa)
    {
        if (!Restaurante.MesaExiste(mesa))
        {
            return NotFound(new { error = "La mesa no existe" });
        }

        var productos = await db.Productos.AsNoTracking().ToListAsync();
        return Ok(new { mesa, categorias = Carta.Organizar(productos) });
    }
}
