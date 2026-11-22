namespace QproLite.Api.Domain;

public record ResumenDeVentas(decimal TotalVendido, int NumeroDePedidos, decimal TicketPromedio);

public static class ReporteDeVentas
{
    /// <summary>Resume las ventas a partir de los pedidos pagados de un día.</summary>
    public static ResumenDeVentas Resumir(IEnumerable<Pedido> pedidosPagados)
    {
        var totales = pedidosPagados.Select(pedido => CalculadoraPedido.CalcularTotal(pedido.Items)).ToList();
        var totalVendido = totales.Sum();
        return new ResumenDeVentas(totalVendido, totales.Count, TicketPromedio(totalVendido, totales.Count));
    }

    // Un día sin ventas no debe producir una división entre cero.
    private static decimal TicketPromedio(decimal totalVendido, int numeroDePedidos) =>
        numeroDePedidos == 0 ? 0 : Math.Round(totalVendido / numeroDePedidos, MidpointRounding.AwayFromZero);
}
