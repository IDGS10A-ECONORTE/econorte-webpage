const currentUrl = window.location.href;

// Rutas generadas desde el layout
const loginUrl = login;
const indexUrl = "/";

// Rutas públicas (se pueden visitar sin autenticarse)
const publicPages = [
    loginUrl,
    info
];

// Token almacenado
const token = Auth.getToken();

// Función para verificar si la URL actual coincide con una pública
const isPublicPage = publicPages.some(p => currentUrl.endsWith(p));

// 1. Si NO hay token y NO es página pública redirige a login
if (!token && !isPublicPage) {
    window.location.href = loginUrl;
}

// 2. Si HAY token y estás en login redirige al index
else if (token && currentUrl.startsWith(loginUrl)) {
    window.location.href = indexUrl;
}