const Auth = {
    getToken() {
        return localStorage.getItem("token");
    },

    getUserData() {
        const data = localStorage.getItem("userData");
        return data ? JSON.parse(data) : null;
    },

    setToken(token) {
        localStorage.setItem("token", token);
    },

    setUserData(userData) {
        localStorage.setItem("userData", userData);
    },

    clearToken() {
        localStorage.removeItem("token");
    },

    clearUserData() {
        localStorage.removeItem("userData");
    },

    isLoggedIn() {
        return !!this.getToken();
    },

    logout() {
        this.clearToken();
        this.clearUserData();
        window.location.href = login;
    },
};