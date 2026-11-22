# Sprint 2 — De la cuenta al cierre del día

Facilita: Scrum Master (Albeiro Vergara).

## Sprint planning

- **Objetivo del sprint:** cerrar el ciclo de la mesa con el cobro de la cuenta, evitar vender productos sin
  existencias y dar al administrador el resumen de ventas del día.
- **Duración:** una semana académica.
- **Capacidad:** tres desarrolladores; se comprometen 13 puntos de historia más las acciones de mejora de la
  retrospectiva del sprint 1.
- **Incremento esperado:** versión `v1.0.0` desplegada en un contenedor Docker.

### Sprint backlog

| Historia | Tarea técnica | Responsable | Estimación | Estado inicial |
|---|---|---|---|---|
| HU-04 Cobrar la cuenta de la mesa (5 pts) | Calcular subtotal, propina y total con TDD | Albeiro | 2 h | Por hacer |
| | Endpoints de cuenta y pago, y pruebas funcionales CA-04.1 a CA-04.3 | Albeiro | 3 h | Por hacer |
| | Pantalla `caja.html` | Albeiro | 1 h | Por hacer |
| HU-05 Descontar inventario (5 pts) | Reglas de existencias con TDD | Mateo | 2 h | Por hacer |
| | Integrar el descuento al registrar pedidos y pruebas funcionales CA-05.1 a CA-05.4 | Mateo | 3 h | Por hacer |
| | Pantalla `inventario.html` | Mateo | 1 h | Por hacer |
| HU-06 Reporte de ventas del día (3 pts) | Calcular el resumen de ventas con TDD | Abrahan | 2 h | Por hacer |
| | Endpoint del reporte y pruebas funcionales CA-06.1 a CA-06.3 | Abrahan | 2 h | Por hacer |
| | Pantalla `reportes.html` | Abrahan | 1 h | Por hacer |
| Mejora de la retrospectiva | Exigir cobertura ≥ 70 % y revisar dependencias vulnerables en el pipeline | Mateo | 1 h | Por hacer |
| Mejora de la retrospectiva | Plantilla de pull request con lista de verificación | Mateo | 0,5 h | Por hacer |
| Mejora de la retrospectiva | Prueba de rendimiento básico de la carta | Mateo | 1 h | Por hacer |

### Retroalimentación aplicada del sprint 1

| Origen | Retroalimentación | Cómo se aplica en este sprint |
|---|---|---|
| Sprint review | Los agotados se marcan a mano. | Se prioriza HU-05: el producto se agota solo al llegar a cero existencias. |
| Sprint review | Falta cobrar la cuenta. | Se prioriza HU-04. |
| Retrospectiva | La cobertura no tiene mínimo. | El pipeline falla si la cobertura de líneas baja del 70 %. |
| Retrospectiva | Pull requests con descripciones dispares. | Plantilla `.github/pull_request_template.md`. |
| Retrospectiva | Archivos compartidos sin coordinación. | Se avisa en la daily al tocar `index.html`. |
