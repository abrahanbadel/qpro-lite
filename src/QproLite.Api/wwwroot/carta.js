// HU-01 · Carta de la mesa. El número de mesa llega en la URL, como en el código QR: carta.html?mesa=3
const mesa = Number(new URLSearchParams(location.search).get('mesa') ?? 1);

function filaDeProducto(producto) {
  const estado = producto.agotado ? '<span class="etiqueta alerta">Agotado</span>' : '';
  return `
    <tr>
      <td class="${producto.agotado ? 'agotado' : ''}">${producto.nombre}</td>
      <td>${estado}</td>
      <td class="num">${pesos(producto.precio)}</td>
    </tr>`;
}

function tablaDeCategoria(categoria) {
  return `
    <h2>${categoria.nombre}</h2>
    <table>
      <thead><tr><th>Producto</th><th></th><th class="num">Precio</th></tr></thead>
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

cargarCarta();
