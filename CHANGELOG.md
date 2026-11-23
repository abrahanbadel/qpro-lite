# Registro de cambios

Formato basado en [Keep a Changelog](https://keepachangelog.com/es-ES/1.1.0/) y versionado semántico.

## [1.0.1] - Hotfix

### Corregido

- El reporte de ventas usaba la fecha en UTC: las ventas cobradas después de las 7:00 p. m. aparecían en el día
  siguiente. Ahora el día de ventas se calcula con la hora de Colombia (UTC-5).

## [1.0.0] - Sprint 2

### Agregado

- HU-04: cuenta de la mesa con propina sugerida del 10 % y registro del pago.
- HU-05: descuento de existencias al registrar pedidos, agotados automáticos y alerta de stock bajo.
- HU-06: reporte de ventas del día con total vendido, pedidos cobrados y ticket promedio.
- Prueba no funcional de rendimiento básico de la carta.
- Plantilla de pull request con lista de verificación.

### Cambiado

- El pipeline exige una cobertura de líneas mínima del 70 % y revisa dependencias vulnerables.

## [0.1.0] - Sprint 1

### Agregado

- HU-01: carta de la mesa agrupada por categorías, con los productos agotados marcados.
- HU-02: registro de pedidos desde la mesa con cálculo del total.
- HU-03: pantalla de cocina con comandas por orden de llegada y flujo de estados.
- HT-01: pipeline de integración continua en GitHub Actions e imagen Docker multietapa.
