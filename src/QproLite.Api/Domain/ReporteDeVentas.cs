namespace QproLite.Api.Domain;

public record ResumenDeVentas(DateOnly Fecha, decimal TotalVendido, int NumeroDePedidos, decimal TicketPromedio);

public static class ReporteDeVentas
{
    /// <summary>Resume los pedidos pagados en la fecha indicada.</summary>
    public static ResumenDeVentas Resumir(IEnumerable<Pedido> pedidos, DateOnly fecha)
    {
        // Pendiente: aún no hay implementación (etapa ROJA del ciclo TDD).
        return new ResumenDeVentas(fecha, 0, 0, 0);
    }
}
