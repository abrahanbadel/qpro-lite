// HU-04 · Caja: cuenta de la mesa, propina sugerida y registro del pago.
const selectorDeMesa = document.getElementById('mesa');
const conPropina = document.getElementById('propina');

selectorDeMesa.innerHTML = Array.from({ length: 10 }, (_, i) => `<option value="${i + 1}">${i + 1}</option>`).join('');
selectorDeMesa.value = '3';

async function cargarCuenta() {
  try {
    const cuenta = await api(`/api/mesas/${selectorDeMesa.value}/cuenta`);
    document.getElementById('pedidos').innerHTML = cuenta.pedidos.length
      ? cuenta.pedidos.map(p => `<tr><td>#${p.id}</td><td>${p.estado}</td><td class="num">${pesos(p.total)}</td></tr>`).join('')
      : '<tr><td colspan="3">La mesa no tiene consumos pendientes.</td></tr>';
    document.getElementById('subtotal').textContent = pesos(cuenta.subtotal);
    document.getElementById('propina-valor').textContent = pesos(conPropina.checked ? cuenta.propinaSugerida : 0);
    document.getElementById('total').textContent = pesos(conPropina.checked ? cuenta.total : cuenta.subtotal);
    document.getElementById('cobrar').disabled = cuenta.pedidos.length === 0;
  } catch (error) {
    avisar(error.message, true);
  }
}

async function cobrar() {
  try {
    const pago = await api(`/api/mesas/${selectorDeMesa.value}/pago`, {
      method: 'POST',
      body: { incluirPropina: conPropina.checked },
    });
    avisar(`Mesa ${pago.mesa} cobrada: ${pesos(pago.totalPagado)}`);
    await cargarCuenta();
  } catch (error) {
    avisar(error.message, true);
  }
}

selectorDeMesa.addEventListener('change', cargarCuenta);
conPropina.addEventListener('change', cargarCuenta);
document.getElementById('cobrar').addEventListener('click', cobrar);
cargarCuenta();
