namespace QproLite.Api.Domain;

public record ProductoDeCarta(int Id, string Nombre, decimal Precio, bool Agotado);

public record CategoriaDeCarta(string Nombre, IReadOnlyList<ProductoDeCarta> Productos);

public static class Carta
{
    /// <summary>Organiza los productos en las categorías que ve el comensal.</summary>
    public static IReadOnlyList<CategoriaDeCarta> Organizar(IEnumerable<Producto> productos)
    {
        var lista = productos.ToList();
        var nombresDeCategoria = new List<string>();
        foreach (var producto in lista)
        {
            if (!nombresDeCategoria.Contains(producto.Categoria))
            {
                nombresDeCategoria.Add(producto.Categoria);
            }
        }

        nombresDeCategoria.Sort(StringComparer.CurrentCulture);

        var carta = new List<CategoriaDeCarta>();
        foreach (var nombre in nombresDeCategoria)
        {
            var productosDeLaCategoria = new List<ProductoDeCarta>();
            foreach (var producto in lista)
            {
                if (producto.Categoria == nombre)
                {
                    productosDeLaCategoria.Add(
                        new ProductoDeCarta(producto.Id, producto.Nombre, producto.Precio, !producto.Disponible));
                }
            }

            productosDeLaCategoria.Sort((a, b) => string.Compare(a.Nombre, b.Nombre, StringComparison.CurrentCulture));
            carta.Add(new CategoriaDeCarta(nombre, productosDeLaCategoria));
        }

        return carta;
    }
}
