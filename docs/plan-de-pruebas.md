# Plan de pruebas ágil — QPro Lite

Responsable: Mateo Villalobos (QA y DevOps). Todo el equipo escribe y mantiene pruebas.

## Alcance

Se prueban las historias HU-01 a HU-06 y la historia técnica HT-01. Quedan fuera las historias HU-07 a HU-10, que
no se construyen en este caso práctico.

## Tipos de prueba

| Tipo | Qué verifica | Herramienta | Dónde vive | Cuándo se ejecuta |
|---|---|---|---|---|
| Unitaria (TDD) | Reglas de negocio aisladas | xUnit | `tests/QproLite.Tests/Unitarias` | En cada commit local y en CI |
| Funcional | Cada criterio de aceptación, a través de la API HTTP | xUnit + WebApplicationFactory | `tests/QproLite.Tests/Funcionales` | En CI y antes de cada pull request |
| Integración | API + Entity Framework Core + SQLite reales | xUnit + SQLite | `tests/QproLite.Tests/Integracion` | En CI |
| No funcional: cobertura | Porcentaje de líneas ejecutadas por las pruebas | Coverlet | Paso «Reporte de cobertura» de CI | En CI |
| Sistema (humo) | La imagen Docker arranca y responde | Docker + curl | Job «Construir imagen Docker» de CI | En CI |

Cada prueba lleva la etiqueta `Categoria` (`Unitaria`, `Funcional` o `Integracion`) para ejecutarla por separado:

```bash
dotnet test --filter "Categoria=Unitaria"
```

## Estrategia

1. **TDD para las reglas de negocio.** Primero la prueba que falla (rojo), luego el código mínimo que la hace
   pasar (verde) y por último la limpieza del código con las pruebas en verde (refactorización). Cada etapa queda en
   un commit separado: `test:`, `feat:` y `refactor:`.
2. **Una prueba funcional por criterio de aceptación.** El nombre de la prueba empieza con el identificador del
   criterio (por ejemplo, `CA_02_3_...`) para mantener la trazabilidad.
3. **Base de datos aislada.** Cada prueba funcional o de integración levanta la API con una base SQLite nueva en
   memoria, cargada con la carta de demostración.

## Criterios de entrada y salida

- **Entrada:** la historia tiene criterios de aceptación escritos en formato Dado/Cuando/Entonces y la rama parte de
  `develop` actualizado.
- **Salida:** todas las pruebas pasan en local y en el pipeline, cada criterio tiene su prueba funcional y otro
  integrante aprobó el pull request.

## Matriz de trazabilidad

La matriz completa (historia → criterio → caso de prueba → resultado) se mantiene en el informe final y se
actualiza al cierre de cada sprint. Los casos de prueba se localizan por el prefijo del criterio en
`tests/QproLite.Tests/Funcionales`.

## Gestión de defectos

Un defecto encontrado en `develop` se corrige en la rama de la historia o en una rama `feature/*` nueva. Un defecto
encontrado en producción (`main`) se corrige con una rama `hotfix/*`, siempre empezando por una prueba que lo
reproduzca.
