namespace QproLite.Api.Domain;

public enum EstadoPedido
{
    Pendiente,
    EnPreparacion,
    Listo,
    Entregado
}

public class Pedido
{
    public int Id { get; set; }
    public int Mesa { get; set; }
    public EstadoPedido Estado { get; set; } = EstadoPedido.Pendiente;
    public DateTime CreadoEn { get; set; }
    public bool Pagado { get; set; }
    public DateTime? PagadoEn { get; set; }
    public List<PedidoItem> Items { get; set; } = [];
}

public class PedidoItem
{
    public int Id { get; set; }
    public int PedidoId { get; set; }
    public int ProductoId { get; set; }
    public string NombreProducto { get; set; } = "";
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
}
