// HU-05 · Inventario: existencias, alerta de stock bajo y reposición.
function estadoDe(producto) {
  if (!producto.disponible) return '<span class="etiqueta alerta">Agotado</span>';
  if (producto.stockBajo) return '<span class="etiqueta alerta">Existencias bajas</span>';
  return '<span class="etiqueta">Disponible</span>';
}

function filaDeProducto(producto) {
  return `
    <tr>
      <td>${producto.nombre}</td>
      <td>${producto.categoria}</td>
      <td class="num">${producto.stock}</td>
      <td>${estadoDe(producto)}</td>
      <td>
        <input type="number" min="0" value="${producto.stock}" id="stock-${producto.id}" aria-label="Existencias de ${producto.nombre}">
        <button type="button" class="secundario" data-producto="${producto.id}">Guardar</button>
      </td>
    </tr>`;
}

async function cargarInventario() {
  try {
    const productos = await api('/api/inventario');
    document.getElementById('inventario').innerHTML = productos.map(filaDeProducto).join('');
  } catch (error) {
    avisar(error.message, true);
  }
}

document.getElementById('inventario').addEventListener('click', async evento => {
  const boton = evento.target.closest('button[data-producto]');
  if (!boton) return;
  const id = boton.dataset.producto;
  try {
    const producto = await api(`/api/inventario/${id}`, {
      method: 'PUT',
      body: { stock: Number(document.getElementById(`stock-${id}`).value) },
    });
    avisar(`${producto.nombre}: ${producto.stock} unidades`);
    await cargarInventario();
  } catch (error) {
    avisar(error.message, true);
  }
});

cargarInventario();
