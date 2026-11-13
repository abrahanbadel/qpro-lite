namespace QproLite.Api.Domain;

public record ProductoDeCarta(int Id, string Nombre, decimal Precio, bool Agotado);

public record CategoriaDeCarta(string Nombre, IReadOnlyList<ProductoDeCarta> Productos);

public static class Carta
{
    /// <summary>Organiza los productos en las categorías que ve el comensal.</summary>
    public static IReadOnlyList<CategoriaDeCarta> Organizar(IEnumerable<Producto> productos)
    {
        // Pendiente: aún no hay implementación (etapa ROJA del ciclo TDD).
        return [];
    }
}
