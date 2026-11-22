namespace QproLite.Api.Domain;

public record ResumenDeVentas(DateOnly Fecha, decimal TotalVendido, int NumeroDePedidos, decimal TicketPromedio);

public static class ReporteDeVentas
{
    /// <summary>Resume los pedidos pagados en la fecha indicada, según la fecha guardada en PagadoEn.</summary>
    public static ResumenDeVentas Resumir(IEnumerable<Pedido> pedidos, DateOnly fecha)
    {
        var pagados = pedidos
            .Where(pedido => pedido.Pagado && pedido.PagadoEn.HasValue && DateOnly.FromDateTime(pedido.PagadoEn.Value) == fecha)
            .ToList();
        var total = pagados.Sum(pedido => CalculadoraPedido.CalcularTotal(pedido.Items));
        return new ResumenDeVentas(fecha, total, pagados.Count, TicketPromedio(total, pagados.Count));
    }

    private static decimal TicketPromedio(decimal total, int pedidos) =>
        pedidos == 0 ? 0 : Math.Round(total / pedidos, 2);
}
