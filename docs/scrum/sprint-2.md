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

## Daily scrum (registro breve)

| Día | Integrante | Qué hice | Qué haré | Bloqueos |
|---|---|---|---|---|
| 1 | Albeiro | Publiqué el sprint backlog y escribí las pruebas de la cuenta (rojo). | Implementar el cálculo y los endpoints de caja. | Ninguno |
| 1 | Mateo | Apliqué las mejoras de la retrospectiva al pipeline y agregué la plantilla de pull request. | Empezar HU-05 con TDD. | Ninguno |
| 1 | Abrahan | Escribí las pruebas del resumen de ventas (rojo) y avisé que tocaré `index.html`. | Implementar el resumen y el endpoint. | Ninguno |
| 2 | Albeiro | Terminé caja con sus pruebas funcionales; pull request aprobado y fusionado. | Revisar el pull request de Abrahan. | Ninguno |
| 2 | Mateo | Integré el descuento de existencias al registro de pedidos; pull request fusionado. | Preparar la rama `release/v1.0.0`. | Ninguno |
| 2 | Abrahan | Terminé el reporte y su pantalla. | Abrir el pull request. | Conflicto en `index.html` con HU-05; se resuelve fusionando `develop` en mi rama. |
| 3 | Albeiro | Aprobé HU-06 tras la resolución del conflicto. | Registrar la review y la retrospectiva. | Ninguno |
| 3 | Mateo | Preparé la versión 1.0.0 y probé la imagen Docker. | Publicar la etiqueta `v1.0.0`. | Ninguno |
| 3 | Abrahan | Resolví el conflicto y ajusté los estilos en la rama de la versión. | Aceptar las historias en la review. | Ninguno |

## Sprint review

- **Incremento entregado:** `v1.0.0`, ejecutándose en un contenedor Docker en `http://localhost:8080`.
- **Demostración:** se pidió desde la mesa 3 hasta agotar la limonada de coco, que quedó marcada como agotada en
  la carta; se cobró la cuenta con propina y el reporte del día mostró la venta.
- **Historias aceptadas:** HU-04, HU-05 y HU-06 (13 de 13 puntos). Las tres acciones de mejora de la retrospectiva
  del sprint 1 quedaron aplicadas.
- **Retroalimentación del Product Owner:** el reporte debe trabajar con la hora de Colombia; se revisará después
  de publicar la versión.

## Sprint retrospective

| Qué salió bien | Qué se puede mejorar | Acción siguiente | Responsable |
|---|---|---|---|
| El umbral de cobertura dio confianza para refactorizar. | El conflicto en `index.html` se detectó al abrir el pull request. | Actualizar la rama con `develop` antes de abrir el pull request. | Todos |
| La plantilla de pull request agilizó las revisiones. | Las fechas del reporte no se probaron con horas cercanas a la medianoche. | Agregar casos de borde de fecha y hora a las pruebas funcionales. | Abrahan |
| Avisar en la daily sobre archivos compartidos evitó sorpresas. | | | |
