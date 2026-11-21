# Sprint 1 — Del código QR a la cocina

Facilita: Scrum Master (Albeiro Vergara).

## Sprint planning

- **Objetivo del sprint:** que un comensal pueda ver la carta de su mesa, enviar un pedido y que cocina lo
  gestione hasta entregarlo.
- **Duración:** una semana académica.
- **Capacidad:** tres desarrolladores; se comprometen 13 puntos de historia.
- **Incremento esperado:** versión `v0.1.0` desplegada en un contenedor Docker.

### Sprint backlog

| Historia | Tarea técnica | Responsable | Estimación | Estado inicial |
|---|---|---|---|---|
| HU-01 Ver la carta de la mesa (3 pts) | Organizar productos por categoría con TDD | Abrahan | 2 h | Por hacer |
| | Endpoint `GET /api/carta` y pruebas funcionales CA-01.1 a CA-01.3 | Abrahan | 2 h | Por hacer |
| | Pantalla `carta.html` | Abrahan | 1 h | Por hacer |
| HU-02 Registrar un pedido (5 pts) | Calcular el total del pedido con TDD | Albeiro | 2 h | Por hacer |
| | Endpoint `POST /api/pedidos` y pruebas funcionales CA-02.1 a CA-02.4 | Albeiro | 3 h | Por hacer |
| | Prueba de integración de persistencia del pedido | Albeiro | 1 h | Por hacer |
| | Botón «Pedir» en la carta | Albeiro | 1 h | Por hacer |
| HU-03 Gestionar comandas en cocina (5 pts) | Reglas de transición de estados con TDD | Mateo | 2 h | Por hacer |
| | Endpoints de cocina y pruebas funcionales CA-03.1 a CA-03.4 | Mateo | 3 h | Por hacer |
| | Pantalla `cocina.html` | Mateo | 1 h | Por hacer |

### Acuerdos del equipo

- Cada historia se desarrolla en su rama `feature/HU-XX-nombre`, creada desde `develop`.
- Ningún cambio entra a `develop` sin pull request, pipeline en verde y una aprobación.
- La reunión diaria se hace por chat en tres líneas: qué hice, qué haré y qué me bloquea.

## Daily scrum (registro breve)

| Día | Integrante | Qué hice | Qué haré | Bloqueos |
|---|---|---|---|---|
| 1 | Abrahan | Creé la rama de HU-01 y escribí las pruebas de la carta (rojo). | Implementar la organización por categorías. | Ninguno |
| 1 | Albeiro | Publiqué el sprint backlog y escribí las pruebas del total del pedido. | Implementar el cálculo y el endpoint. | Ninguno |
| 1 | Mateo | Abrí el pull request de HU-03 con las pruebas en rojo. | Implementar el flujo de estados. | El pipeline bloquea el pull request hasta que las pruebas pasen (esperado). |
| 2 | Abrahan | Terminé el endpoint y la pantalla de la carta; pull request aprobado. | Revisar el pull request de Mateo. | Ninguno |
| 2 | Albeiro | Endpoint de pedidos con pruebas funcionales y de integración. | Actualizar mi rama con `develop` y agregar el botón «Pedir». | Mi rama quedó atrás de `develop`; se resuelve con `git rebase`. |
| 2 | Mateo | Pipeline en verde tras implementar el flujo; refactoricé. | Endpoints y pantalla de cocina. | Ninguno |
| 3 | Abrahan | Revisé y aprobé HU-03. | Preparar la rama `release/v0.1.0`. | Ninguno |
| 3 | Albeiro | Fusioné HU-02 después del rebase. | Registrar la review y la retrospectiva. | Ninguno |
| 3 | Mateo | Fusioné HU-03 y probé la imagen Docker. | Revisar el pull request de la versión. | Ninguno |

## Sprint review

- **Incremento entregado:** `v0.1.0`, ejecutándose en un contenedor Docker en `http://localhost:8080`.
- **Demostración:** el Product Owner abrió la carta de la mesa 3, envió un pedido y lo llevó en cocina de
  «Pendiente» a «Entregado».
- **Historias aceptadas:** HU-01, HU-02 y HU-03 (13 de 13 puntos). Todos los criterios de aceptación tienen una
  prueba funcional en verde.
- **Retroalimentación del Product Owner:** (1) un plato solo se marca como agotado a mano en la base de datos; debe
  ocurrir solo cuando se acaban las existencias; (2) hace falta cobrar la cuenta para cerrar el ciclo de la mesa.
- **Decisión:** se confirman HU-04 (caja) y HU-05 (inventario) como prioridad del sprint 2, seguidas de HU-06.

## Sprint retrospective

| Qué salió bien | Qué se puede mejorar | Acción para el sprint 2 | Responsable |
|---|---|---|---|
| El ciclo TDD en commits separados hizo fáciles las revisiones. | El pipeline reporta la cobertura, pero no exige un mínimo. | Exigir cobertura de líneas ≥ 70 % en el pipeline. | Mateo |
| El pipeline bloqueó un pull request con pruebas en rojo antes de fusionarlo. | Las descripciones de los pull requests no eran uniformes. | Agregar una plantilla de pull request con lista de verificación. | Mateo |
| El rebase mantuvo el historial de HU-02 lineal. | No se revisan vulnerabilidades en las dependencias. | Agregar al pipeline la revisión de paquetes vulnerables. | Mateo |
| | Dos historias tocaron la página de inicio sin avisarse. | Anunciar en la daily cuando se toque un archivo compartido. | Albeiro |
