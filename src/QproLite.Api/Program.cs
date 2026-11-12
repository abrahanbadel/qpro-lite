using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using QproLite.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddDbContext<AppDbContext>(opciones =>
    opciones.UseSqlite(builder.Configuration.GetConnectionString("Qpro")));

var app = builder.Build();

// Crea la base de datos y carga la carta de demostración la primera vez.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    SeedData.Sembrar(db);
}

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();

var version = Assembly.GetExecutingAssembly()
    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion.Split('+')[0] ?? "desconocida";

app.MapGet("/api/version", () => new { nombre = "QPro Lite", version });
app.MapGet("/health", () => Results.Ok(new { estado = "ok" }));

app.Run();

// Permite que las pruebas levanten la API en memoria (WebApplicationFactory).
public partial class Program;
