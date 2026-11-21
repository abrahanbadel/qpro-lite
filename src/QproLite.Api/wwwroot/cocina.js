// HU-03 · Pantalla de cocina: comandas por orden de llegada y avance de estado.
const NOMBRES = { Pendiente: 'Pendiente', EnPreparacion: 'En preparación', Listo: 'Listo', Entregado: 'Entregado' };

function filaDeComanda(comanda) {
  const productos = comanda.items.map(item => `${item.cantidad} × ${item.nombre}`).join('<br>');
  const boton = comanda.siguiente
    ? `<button type="button" data-pedido="${comanda.id}" data-estado="${comanda.siguiente}">Pasar a ${NOMBRES[comanda.siguiente]}</button>`
    : '';
  return `
    <tr>
      <td>#${comanda.id}</td>
      <td>Mesa ${comanda.mesa}</td>
      <td>${productos}</td>
      <td><span class="etiqueta">${NOMBRES[comanda.estado]}</span></td>
      <td class="num">${boton}</td>
    </tr>`;
}

async function cargarComandas() {
  try {
    const comandas = await api('/api/cocina/comandas');
    document.getElementById('comandas').innerHTML = comandas.length
      ? comandas.map(filaDeComanda).join('')
      : '<tr><td colspan="5">No hay comandas pendientes.</td></tr>';
  } catch (error) {
    avisar(error.message, true);
  }
}

document.getElementById('comandas').addEventListener('click', async evento => {
  const boton = evento.target.closest('button[data-pedido]');
  if (!boton) return;
  try {
    await api(`/api/pedidos/${boton.dataset.pedido}/estado`, { method: 'PATCH', body: { estado: boton.dataset.estado } });
    avisar(`Pedido #${boton.dataset.pedido}: ${NOMBRES[boton.dataset.estado]}`);
    await cargarComandas();
  } catch (error) {
    avisar(error.message, true);
  }
});

cargarComandas();
setInterval(cargarComandas, 5000); // La pantalla se refresca sola cada 5 segundos.
