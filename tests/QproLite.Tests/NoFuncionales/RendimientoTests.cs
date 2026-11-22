using System.Diagnostics;
using QproLite.Tests.Soporte;

namespace QproLite.Tests.NoFuncionales;

/// <summary>Prueba no funcional de rendimiento básico: la carta es la pantalla más consultada del sistema.</summary>
[Trait("Categoria", "NoFuncional")]
public class RendimientoTests
{
    [Fact]
    public async Task La_carta_atiende_cien_consultas_seguidas_en_menos_de_cinco_segundos()
    {
        using var api = new ApiFactory();
        var http = api.CreateClient();
        (await http.GetAsync("/api/carta?mesa=1")).EnsureSuccessStatusCode(); // Calentamiento.

        var reloj = Stopwatch.StartNew();
        for (var i = 0; i < 100; i++)
        {
            (await http.GetAsync("/api/carta?mesa=1")).EnsureSuccessStatusCode();
        }

        reloj.Stop();
        Assert.True(reloj.Elapsed < TimeSpan.FromSeconds(5), $"Cien consultas tardaron {reloj.ElapsedMilliseconds} ms");
    }
}
