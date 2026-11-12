# QPro Lite

Caso práctico de integración continua de la Electiva Profesional en Ingeniería de Software (CECAR). QPro Lite es
una versión reducida, escrita desde cero, de algunas funciones de QPro, un sistema para restaurantes: carta por
mesa, pedidos, pantalla de cocina, caja, inventario y reporte de ventas.

> QPro es solo la referencia funcional. Este repositorio es nuevo y no contiene código del proyecto original.

## Arquitectura

| Capa | Tecnología | Carpeta |
|---|---|---|
| API REST | ASP.NET Core (.NET 10), controladores | `src/QproLite.Api` |
| Persistencia | Entity Framework Core + SQLite | `src/QproLite.Api/Data` |
| Reglas de negocio | Clases puras de C#, escritas con TDD | `src/QproLite.Api/Domain` |
| Interfaz web | HTML, CSS y JavaScript servidos por la API | `src/QproLite.Api/wwwroot` |
| Pruebas | xUnit, WebApplicationFactory, Coverlet | `tests/QproLite.Tests` |

```
Navegador ──HTTP──> API ASP.NET Core ──EF Core──> SQLite
                      │
                      └── wwwroot (pantallas estáticas)
```

## Requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- Git

## Ejecutar en local

```bash
dotnet run --project src/QproLite.Api --urls http://localhost:8080
```

Abrir <http://localhost:8080>. La base de datos SQLite (`qpro.db`) se crea sola con una carta de demostración.

## Ejecutar las pruebas

```bash
dotnet test
```

Por tipo de prueba:

```bash
dotnet test --filter "Categoria=Unitaria"
dotnet test --filter "Categoria=Funcional"
dotnet test --filter "Categoria=Integracion"
```

## Flujo de trabajo

- Metodología: Scrum (ver `docs/scrum`).
- Ramas: Gitflow (`main`, `develop`, `feature/*`, `release/*`, `hotfix/*`).
- Commits: [Conventional Commits](https://www.conventionalcommits.org/es/).
- Todo cambio entra por pull request con revisión de otro integrante.

## Equipo

| Integrante | Rol Scrum |
|---|---|
| Abrahan Jussep Badel Palmett | Product Owner y desarrollador |
| Albeiro José Vergara Pérez | Scrum Master y desarrollador |
| Mateo David Villalobos Bertel | Desarrollador, QA y DevOps |

Corporación Universitaria del Caribe (CECAR) · Facultad de Ciencias Básicas, Ingeniería y Arquitectura · 2026.
