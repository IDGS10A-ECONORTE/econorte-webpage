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
const user = Auth.getUserData();

//console.log("Token:", token);
//console.log("User Data:", user);

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

document.addEventListener("DOMContentLoaded", () => {
    const btnLogout = document.getElementById("btnLogout");

    if (token) {
        btnLogout.classList.remove("d-none");
    } else {
        btnLogout.classList.add("d-none");
    }

    btnLogout.addEventListener("click", () => {
        if (token) Auth.logout();
    });
});