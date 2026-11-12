namespace QproLite.Api.Domain;

public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string Categoria { get; set; } = "";
    public decimal Precio { get; set; }
    public bool Disponible { get; set; } = true;
    public int Stock { get; set; }
}
