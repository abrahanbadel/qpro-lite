// HU-06 · Reporte de ventas del día.
const campoFecha = document.getElementById('fecha');

async function cargarReporte() {
  try {
    const consulta = campoFecha.value ? `?fecha=${campoFecha.value}` : '';
    const reporte = await api(`/api/reportes/ventas${consulta}`);
    campoFecha.value = reporte.fecha;
    document.getElementById('total-vendido').textContent = pesos(reporte.totalVendido);
    document.getElementById('numero-pedidos').textContent = reporte.numeroDePedidos;
    document.getElementById('ticket-promedio').textContent = pesos(reporte.ticketPromedio);
    avisar('');
  } catch (error) {
    avisar(error.message, true);
  }
}

campoFecha.addEventListener('change', cargarReporte);
cargarReporte();
