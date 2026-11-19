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
