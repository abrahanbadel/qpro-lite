namespace QproLite.Api.Domain;

/// <summary>
/// El servidor guarda las fechas en UTC, pero el día de ventas del restaurante va de medianoche a medianoche
/// en hora de Colombia (UTC-5, sin horario de verano).
/// </summary>
public static class HoraDeColombia
{
    private static readonly TimeSpan Desfase = TimeSpan.FromHours(-5);

    /// <summary>Fecha de Colombia que corresponde a un instante en UTC.</summary>
    public static DateOnly Fecha(DateTime instanteUtc) => DateOnly.FromDateTime(instanteUtc + Desfase);

    /// <summary>Instante en UTC en el que empieza un día de Colombia.</summary>
    public static DateTime InicioDelDiaEnUtc(DateOnly dia) =>
        dia.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc) - Desfase;
}
