using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using QproLite.Api.Data;

namespace QproLite.Tests.Soporte;

/// <summary>
/// Levanta la API completa en memoria con una base de datos SQLite nueva por cada instancia,
/// de modo que cada prueba parte de la carta de demostración y no afecta a las demás.
/// </summary>
public class ApiFactory : WebApplicationFactory<Program>
{
    private readonly SqliteConnection _conexion = new("Data Source=:memory:");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _conexion.Open();
        builder.ConfigureServices(servicios =>
        {
            servicios.RemoveAll<DbContextOptions<AppDbContext>>();
            servicios.RemoveAll<IDbContextOptionsConfiguration<AppDbContext>>();
            servicios.AddDbContext<AppDbContext>(opciones => opciones.UseSqlite(_conexion));
        });
    }

    /// <summary>Ejecuta una acción directamente sobre la base de datos de la prueba.</summary>
    public void ConBaseDeDatos(Action<AppDbContext> accion)
    {
        using var scope = Services.CreateScope();
        accion(scope.ServiceProvider.GetRequiredService<AppDbContext>());
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _conexion.Dispose();
    }
}
