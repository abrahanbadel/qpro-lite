using QproLite.Api.Domain;

namespace QproLite.Tests.Unitarias;

[Trait("Categoria", "Unitaria")]
public class HoraDeColombiaTests
{
    [Fact]
    public void La_madrugada_en_UTC_todavia_es_el_dia_anterior_en_Colombia()
    {
        var instante = new DateTime(2026, 11, 14, 0, 30, 0, DateTimeKind.Utc);

        Assert.Equal(new DateOnly(2026, 11, 13), HoraDeColombia.Fecha(instante));
    }

    [Fact]
    public void El_dia_de_Colombia_empieza_a_las_cinco_de_la_manana_en_UTC()
    {
        var inicio = HoraDeColombia.InicioDelDiaEnUtc(new DateOnly(2026, 11, 13));

        Assert.Equal(new DateTime(2026, 11, 13, 5, 0, 0, DateTimeKind.Utc), inicio);
    }
}
