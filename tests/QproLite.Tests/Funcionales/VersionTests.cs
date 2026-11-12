using System.Net;
using System.Net.Http.Json;
using QproLite.Tests.Soporte;

namespace QproLite.Tests.Funcionales;

[Trait("Categoria", "Funcional")]
public class VersionTests
{
    private record VersionDto(string Nombre, string Version);

    [Fact]
    public async Task La_api_responde_su_nombre_y_version()
    {
        using var api = new ApiFactory();
        var http = api.CreateClient();

        var respuesta = await http.GetAsync("/api/version");

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);
        var cuerpo = await respuesta.Content.ReadFromJsonAsync<VersionDto>();
        Assert.Equal("QPro Lite", cuerpo!.Nombre);
        Assert.False(string.IsNullOrWhiteSpace(cuerpo.Version));
    }
}
