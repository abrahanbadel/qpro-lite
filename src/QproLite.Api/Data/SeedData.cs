using QproLite.Api.Domain;

namespace QproLite.Api.Data;

public static class SeedData
{
    /// <summary>Carga la carta de demostración si la base de datos está vacía.</summary>
    public static void Sembrar(AppDbContext db)
    {
        if (db.Productos.Any())
        {
            return;
        }

        db.Productos.AddRange(
            new Producto { Nombre = "Carimañola de queso", Categoria = "Entradas", Precio = 4500, Stock = 30 },
            new Producto { Nombre = "Patacón con suero", Categoria = "Entradas", Precio = 9000, Stock = 20 },
            new Producto { Nombre = "Mote de queso", Categoria = "Platos fuertes", Precio = 22000, Stock = 15 },
            new Producto { Nombre = "Bandeja de cerdo frito", Categoria = "Platos fuertes", Precio = 28000, Stock = 12 },
            new Producto { Nombre = "Mojarra frita", Categoria = "Platos fuertes", Precio = 32000, Stock = 0, Disponible = false },
            new Producto { Nombre = "Jugo de corozo", Categoria = "Bebidas", Precio = 6000, Stock = 25 },
            new Producto { Nombre = "Limonada de coco", Categoria = "Bebidas", Precio = 9500, Stock = 4 },
            new Producto { Nombre = "Agua de panela con limón", Categoria = "Bebidas", Precio = 4000, Stock = 40 });
        db.SaveChanges();
    }
}
