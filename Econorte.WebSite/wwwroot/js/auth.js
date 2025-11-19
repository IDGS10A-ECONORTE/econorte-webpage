const Auth = {
    getToken() {
        return localStorage.getItem("token");
    },

    setToken(token) {
        localStorage.setItem("token", token);
    },

    clearToken() {
        localStorage.removeItem("token");
    },

    isLoggedIn() {
        return !!this.getToken();
    },

    logout() {
        this.clearToken();
        window.location.href = login; // Ajusta la ruta
    },
};