// HU-06 · Pantalla del reporte de ventas de un día.
const campoFecha = document.getElementById('fecha');

async function cargarReporte() {
  try {
    const resumen = await api(`/api/reportes/ventas?fecha=${encodeURIComponent(campoFecha.value)}`);
    document.getElementById('total').textContent = pesos(resumen.totalVendido);
    document.getElementById('pedidos').textContent = resumen.numeroDePedidos;
    document.getElementById('ticket').textContent = pesos(resumen.ticketPromedio);
    avisar('');
  } catch (error) {
    avisar(error.message, true);
  }
}

document.getElementById('consulta').addEventListener('submit', evento => {
  evento.preventDefault();
  cargarReporte();
});

campoFecha.value = new Date().toLocaleDateString('en-CA'); // en-CA da el formato AAAA-MM-DD.
cargarReporte();
