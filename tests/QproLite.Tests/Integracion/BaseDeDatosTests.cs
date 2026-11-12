using Microsoft.EntityFrameworkCore;
using QproLite.Tests.Soporte;

namespace QproLite.Tests.Integracion;

[Trait("Categoria", "Integracion")]
public class BaseDeDatosTests
{
    [Fact]
    public void Al_iniciar_la_api_se_crea_la_base_de_datos_con_la_carta_de_demostracion()
    {
        using var api = new ApiFactory();

        api.ConBaseDeDatos(db =>
        {
            Assert.Equal(8, db.Productos.Count());
            Assert.Contains(db.Productos.AsNoTracking(), p => !p.Disponible);
        });
    }
}
