namespace QproLite.Api.Domain;

public record ProductoDeCarta(int Id, string Nombre, decimal Precio, bool Agotado);

public record CategoriaDeCarta(string Nombre, IReadOnlyList<ProductoDeCarta> Productos);

public static class Carta
{
    /// <summary>Organiza los productos en las categorías que ve el comensal.</summary>
    public static IReadOnlyList<CategoriaDeCarta> Organizar(IEnumerable<Producto> productos) =>
        productos
            .GroupBy(producto => producto.Categoria)
            .OrderBy(categoria => categoria.Key, StringComparer.CurrentCulture)
            .Select(categoria => new CategoriaDeCarta(categoria.Key, ProductosOrdenados(categoria)))
            .ToList();

    private static List<ProductoDeCarta> ProductosOrdenados(IEnumerable<Producto> productos) =>
        productos
            .OrderBy(producto => producto.Nombre, StringComparer.CurrentCulture)
            .Select(producto => new ProductoDeCarta(producto.Id, producto.Nombre, producto.Precio, !producto.Disponible))
            .ToList();
}
