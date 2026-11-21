// HU-01 · Carta de la mesa. El número de mesa llega en la URL, como en el código QR: carta.html?mesa=3
// HU-02 · El comensal elige cantidades y envía el pedido a cocina.
const mesa = Number(new URLSearchParams(location.search).get('mesa') ?? 1);

function filaDeProducto(producto) {
  const estado = producto.agotado ? '<span class="etiqueta alerta">Agotado</span>' : '';
  const cantidad = producto.agotado
    ? ''
    : `<input type="number" min="0" max="20" value="0" data-producto="${producto.id}" aria-label="Cantidad de ${producto.nombre}">`;
  return `
    <tr>
      <td class="${producto.agotado ? 'agotado' : ''}">${producto.nombre}</td>
      <td>${estado}</td>
      <td class="num">${pesos(producto.precio)}</td>
      <td class="num">${cantidad}</td>
    </tr>`;
}

function tablaDeCategoria(categoria) {
  return `
    <h2>${categoria.nombre}</h2>
    <table>
      <thead><tr><th>Producto</th><th></th><th class="num">Precio</th><th class="num">Cantidad</th></tr></thead>
      <tbody>${categoria.productos.map(filaDeProducto).join('')}</tbody>
    </table>`;
}

async function cargarCarta() {
  try {
    const carta = await api(`/api/carta?mesa=${mesa}`);
    document.getElementById('mesa-titulo').textContent = `Mesa ${carta.mesa}`;
    document.getElementById('carta').innerHTML = carta.categorias.map(tablaDeCategoria).join('');
  } catch (error) {
    avisar(error.message, true);
  }
}

async function enviarPedido() {
  const items = [...document.querySelectorAll('input[data-producto]')]
    .map(campo => ({ productoId: Number(campo.dataset.producto), cantidad: Number(campo.value) }))
    .filter(item => item.cantidad > 0);
  try {
    const pedido = await api('/api/pedidos', { method: 'POST', body: { mesa, items } });
    avisar(`Pedido #${pedido.id} enviado a cocina. Total: ${pesos(pedido.total)}`);
    await cargarCarta();
  } catch (error) {
    avisar(error.message, true);
  }
}

document.getElementById('pedir').addEventListener('click', enviarPedido);
cargarCarta();
