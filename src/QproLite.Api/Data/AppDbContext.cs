using Microsoft.EntityFrameworkCore;
using QproLite.Api.Domain;

namespace QproLite.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> opciones) : DbContext(opciones)
{
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<PedidoItem> PedidoItems => Set<PedidoItem>();
}
