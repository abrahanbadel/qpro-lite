// Utilidades compartidas por todas las pantallas de QPro Lite.

/** Llama a la API y devuelve el JSON; si la API responde con error, lanza su mensaje. */
async function api(ruta, opciones = {}) {
  const respuesta = await fetch(ruta, {
    headers: { 'Content-Type': 'application/json' },
    ...opciones,
    body: opciones.body ? JSON.stringify(opciones.body) : undefined,
  });
  const cuerpo = await respuesta.json().catch(() => null);
  if (!respuesta.ok) {
    throw new Error(cuerpo?.error ?? `Error ${respuesta.status}`);
  }
  return cuerpo;
}

/** Formatea un valor en pesos colombianos. */
function pesos(valor) {
  return new Intl.NumberFormat('es-CO', { style: 'currency', currency: 'COP', maximumFractionDigits: 0 }).format(valor);
}

/** Muestra un mensaje de estado (éxito o error) en el elemento #aviso de la pantalla. */
function avisar(texto, esError = false) {
  const aviso = document.getElementById('aviso');
  if (!aviso) return;
  aviso.textContent = texto;
  aviso.className = esError ? 'aviso error' : 'aviso';
}

// Pie de página con la versión desplegada: permite comprobar qué incremento está en ejecución.
api('/api/version')
  .then(v => { document.getElementById('pie').textContent = `${v.nombre} · versión ${v.version}`; })
  .catch(() => { document.getElementById('pie').textContent = 'QPro Lite'; });
