# Product backlog — QPro Lite

Responsable: Product Owner (Abrahan Badel). El backlog está ordenado por prioridad (MoSCoW) y valor para el
restaurante. La estimación usa puntos de historia (serie de Fibonacci) acordados por el equipo con Planning Poker.

## Resumen priorizado

| Orden | ID | Historia | Prioridad | Puntos | Responsable | Sprint |
|---|---|---|---|---|---|---|
| 1 | HT-01 | Pipeline de integración continua y contenedor | Must | 3 | Mateo Villalobos | 0 |
| 2 | HU-01 | Ver la carta de la mesa | Must | 3 | Abrahan Badel | 1 |
| 3 | HU-02 | Registrar un pedido desde la mesa | Must | 5 | Albeiro Vergara | 1 |
| 4 | HU-03 | Gestionar las comandas en cocina | Must | 5 | Mateo Villalobos | 1 |
| 5 | HU-04 | Cobrar la cuenta de la mesa | Must | 5 | Albeiro Vergara | 2 |
| 6 | HU-05 | Descontar inventario y marcar agotados | Should | 5 | Mateo Villalobos | 2 |
| 7 | HU-06 | Consultar el reporte de ventas del día | Should | 3 | Abrahan Badel | 2 |
| 8 | HU-07 | Iniciar sesión según el rol del personal | Could | 5 | Sin asignar | Futuro |
| 9 | HU-08 | Dividir la cuenta entre comensales | Could | 5 | Sin asignar | Futuro |
| 10 | HU-09 | Gestionar pedidos a domicilio | Won't (por ahora) | 8 | Sin asignar | Futuro |
| 11 | HU-10 | Avisar a cocina en tiempo real | Won't (por ahora) | 8 | Sin asignar | Futuro |

## Historias de usuario

### HT-01 · Pipeline de integración continua y contenedor (historia técnica)

**Como** equipo de desarrollo **quiero** que cada cambio se compile, se pruebe y se empaquete en una imagen Docker
de forma automática **para** detectar errores apenas se integran y tener siempre una versión ejecutable.

- **CA-T1.1** Dado un push o pull request hacia `develop` o `main`, cuando GitHub recibe el cambio, entonces se
  ejecuta el workflow que compila, analiza el formato, corre las pruebas y construye la imagen.
- **CA-T1.2** Dado un pull request con una prueba fallida, cuando termina el workflow, entonces el pull request
  queda bloqueado y no se puede fusionar.
- **CA-T1.3** Dado el repositorio clonado, cuando se ejecuta `docker compose up`, entonces la aplicación responde
  en `http://localhost:8080`.

Tareas: escribir `ci.yml`; escribir `Dockerfile` multi-etapa y `docker-compose.yml`; documentar el plan de pruebas;
configurar la protección de ramas.

### HU-01 · Ver la carta de la mesa

**Como** comensal **quiero** ver la carta al abrir el enlace de mi mesa, organizada por categorías y con los platos
agotados marcados, **para** elegir qué pedir sin esperar al mesero.

- **CA-01.1** Dado que estoy en la mesa 3, cuando abro la carta, entonces veo los productos agrupados por
  categoría con su nombre y precio.
- **CA-01.2** Dado que un producto no está disponible, cuando veo la carta, entonces aparece marcado como agotado.
- **CA-01.3** Dado un número de mesa que no existe, cuando abro la carta, entonces recibo el mensaje «La mesa no
  existe».

Tareas: agrupar productos por categoría (TDD); endpoint `GET /api/carta`; pantalla `carta.html`; pruebas
funcionales de los tres criterios.

### HU-02 · Registrar un pedido desde la mesa

**Como** comensal **quiero** enviar mi pedido desde la carta **para** que llegue a cocina sin intermediarios.

- **CA-02.1** Dado que elegí productos disponibles, cuando confirmo el pedido, entonces queda registrado en estado
  «Pendiente» con el total calculado (precio × cantidad).
- **CA-02.2** Dado un pedido sin productos, cuando lo confirmo, entonces se rechaza con el mensaje «El pedido debe
  tener al menos un producto».
- **CA-02.3** Dado un producto agotado, cuando intento pedirlo, entonces se rechaza indicando que está agotado.
- **CA-02.4** Dada una cantidad menor que 1, cuando confirmo el pedido, entonces se rechaza con el mensaje «La
  cantidad debe ser mayor que cero».

Tareas: calcular el total del pedido (TDD); endpoint `POST /api/pedidos`; botón «Pedir» en la carta; pruebas
funcionales y prueba de integración de persistencia.

### HU-03 · Gestionar las comandas en cocina

**Como** cocinero **quiero** ver las comandas pendientes por orden de llegada y avanzar su estado **para**
organizar la preparación y avisar cuando un pedido está listo.

- **CA-03.1** Dado que hay pedidos sin entregar, cuando abro la pantalla de cocina, entonces los veo ordenados del
  más antiguo al más reciente.
- **CA-03.2** Dado un pedido «Pendiente», cuando lo paso a «EnPreparacion», entonces su estado se actualiza.
- **CA-03.3** Dado un pedido «Pendiente», cuando intento pasarlo directamente a «Listo», entonces se rechaza porque
  la transición no es válida.
- **CA-03.4** Dado un pedido «Entregado», cuando abro la pantalla de cocina, entonces ya no aparece.

Tareas: reglas de transición de estados (TDD); endpoints `GET /api/cocina/comandas` y
`PATCH /api/pedidos/{id}/estado`; pantalla `cocina.html`; pruebas funcionales.

### HU-04 · Cobrar la cuenta de la mesa

**Como** cajero **quiero** ver la cuenta de una mesa con la propina sugerida y registrar el pago **para** cobrar
rápido y sin errores de suma.

- **CA-04.1** Dado que la mesa tiene pedidos sin pagar, cuando consulto la cuenta, entonces veo el subtotal, la
  propina sugerida del 10 % y el total.
- **CA-04.2** Dado que la mesa tiene una cuenta abierta, cuando registro el pago, entonces los pedidos quedan
  pagados y la cuenta de la mesa vuelve a cero.
- **CA-04.3** Dado que la mesa no tiene consumos pendientes, cuando intento cobrar, entonces se rechaza con el
  mensaje «La mesa no tiene consumos pendientes».

Tareas: cálculo de la cuenta y la propina (TDD); endpoints `GET /api/mesas/{mesa}/cuenta` y
`POST /api/mesas/{mesa}/pago`; pantalla `caja.html`; pruebas funcionales.

### HU-05 · Descontar inventario y marcar agotados

**Como** administrador **quiero** que cada pedido descuente las existencias y que un producto sin existencias se
marque como agotado **para** no vender lo que no hay.

- **CA-05.1** Dado un producto con 15 unidades, cuando se piden 2, entonces quedan 13.
- **CA-05.2** Dado un producto con las últimas unidades, cuando se piden todas, entonces aparece agotado en la
  carta.
- **CA-05.3** Dado un producto con 4 unidades, cuando se piden 5, entonces el pedido se rechaza indicando cuántas
  quedan y las existencias no cambian.
- **CA-05.4** Dado un producto con 5 unidades o menos, cuando consulto el inventario, entonces aparece con alerta
  de existencias bajas.

Tareas: reglas de descuento de existencias (TDD); integrar el descuento al registrar pedidos; endpoints
`GET /api/inventario` y `PUT /api/inventario/{id}`; pantalla `inventario.html`; pruebas funcionales.

### HU-06 · Consultar el reporte de ventas del día

**Como** administrador **quiero** ver cuánto se vendió en un día, cuántos pedidos se cobraron y el ticket promedio
**para** tomar decisiones sin hacer cuentas a mano.

- **CA-06.1** Dado que hubo pedidos pagados en la fecha consultada, cuando abro el reporte, entonces veo el total
  vendido, el número de pedidos y el ticket promedio.
- **CA-06.2** Dado un día sin ventas, cuando abro el reporte, entonces veo los valores en cero y no un error.
- **CA-06.3** Dada una fecha con formato inválido, cuando consulto el reporte, entonces se rechaza con el mensaje
  «La fecha debe tener el formato AAAA-MM-DD».

Tareas: cálculo del resumen de ventas (TDD); endpoint `GET /api/reportes/ventas`; pantalla `reportes.html`; pruebas
funcionales.

### Historias fuera del alcance de este caso práctico

- **HU-07** Como miembro del personal quiero iniciar sesión según mi rol para ver solo mis pantallas.
- **HU-08** Como cajero quiero dividir la cuenta entre los comensales para cobrar a cada uno lo suyo.
- **HU-09** Como cliente quiero pedir a domicilio para recibir la comida en casa.
- **HU-10** Como cocinero quiero que los pedidos nuevos aparezcan sin recargar la pantalla para no perder tiempo.

## Definición de terminado (DoD)

Una historia está terminada cuando: (1) todos sus criterios de aceptación tienen una prueba funcional automatizada
en verde; (2) la lógica de negocio se escribió con TDD; (3) el pipeline de integración continua pasa; (4) otro
integrante revisó y aprobó el pull request; y (5) el cambio está integrado en `develop`.
