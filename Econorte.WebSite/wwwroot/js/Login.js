if (token) window.location.href = index;

document.addEventListener("DOMContentLoaded", () => {

    let isLogin = true;

    // Elementos comunes
    const loginSection = document.getElementById("loginSection");
    const registerSection = document.getElementById("registerSection");
    const switchToRegister = document.getElementById("switchToRegister");
    const switchToLogin = document.getElementById("switchToLogin");

    // LOGIN
    const loginEmail = document.getElementById("loginEmail");
    const loginPassword = document.getElementById("loginPassword");
    const btnLogin = document.getElementById("btnLogin");

    // REGISTRO
    const registerName = document.getElementById("registerName");
    const registerEmail = document.getElementById("registerEmail");
    const registerPhone = document.getElementById("registerPhone");
    const registerPassword = document.getElementById("registerPassword");
    const registerConfirmPassword = document.getElementById("registerConfirmPassword");
    const btnRegister = document.getElementById("btnRegister");

    // -------- Alternar formularios --------
    function toggleForm() {
        isLogin = !isLogin;
        loginSection.style.display = isLogin ? "block" : "none";
        registerSection.style.display = isLogin ? "none" : "block";
        toggleButtons(); // actualiza estado de botones
    }

    switchToRegister.addEventListener("click", e => {
        e.preventDefault();
        if (isLogin) toggleForm();
    });

    switchToLogin.addEventListener("click", e => {
        e.preventDefault();
        if (!isLogin) toggleForm();
    });

    // -------- Validación general --------
    function toggleButtons() {
        if (isLogin) {
            const emailFilled = loginEmail.value.trim().length > 0;
            const passwordFilled = loginPassword.value.trim().length > 0;
            btnLogin.disabled = !(emailFilled && passwordFilled);
        } else {
            const allFieldsFilled =
                registerName.value.trim().length > 0 &&
                registerEmail.value.trim().length > 0 &&
                registerPhone.value.trim().length > 0 &&
                registerPassword.value.trim().length > 0 &&
                registerConfirmPassword.value.trim().length > 0;

            const passwordsMatch =
                registerPassword.value === registerConfirmPassword.value;

            btnRegister.disabled = !(allFieldsFilled && passwordsMatch);
        }
    }

    // Escuchar cambios en todos los inputs
    [
        loginEmail, loginPassword,
        registerName, registerEmail, registerPhone,
        registerPassword, registerConfirmPassword
    ].forEach(input => input.addEventListener("input", toggleButtons));

    // Estado inicial
    toggleButtons();

    // ----------- LOGIN -----------
    btnLogin.addEventListener("click", async () => {
        const email = loginEmail.value.trim();
        const password = loginPassword.value.trim();

        if (!email || !password) {
            alert("Por favor completa ambos campos.");
            return;
        }

        try {
            const config = {
                IdApi: 1,
                BodyParams: {
                    Email: email,
                    Password: password
                }
            };

            const response = await axios.post(callApiAsync, config);
            const result = response.data;

            // Verifica usuario válido
            if (result && result.Id_User && result.Id_User !== 0) {

                // *** IMPORTANTE: guardar el token ***
                if (result.Token) {
                    Auth.setToken(result.Token);
                } else {
                    console.warn("El backend no devolvió token");
                }

                console.log("Inicio de sesión exitoso");
                window.location.href = index;
            } else {
                alert("Credenciales incorrectas. Intenta de nuevo.");
            }
        } catch (error) {
            console.error("Error en login:", error);
            alert("Ocurrió un error al iniciar sesión.");
        }
    });

    // ----------- REGISTRO -----------
    btnRegister.addEventListener("click", async () => {
        const name = registerName.value.trim();
        const email = registerEmail.value.trim();
        const phone = registerPhone.value.trim();
        const password = registerPassword.value.trim();
        const confirmPassword = registerConfirmPassword.value.trim();

        if (!name || !email || !phone || !password || !confirmPassword) {
            alert("Por favor completa todos los campos.");
            return;
        }

        if (password !== confirmPassword) {
            alert("Las contraseñas no coinciden.");
            return;
        }

        try {
            const config = {
                IdApi: 3,
                BodyParams: {
                    Name: name,
                    Email: email,
                    Phone: phone,
                    Password: password
                }
            };

            const response = await axios.post(callApiAsync, config);
            const result = response.data;

            if (result && result.Success) {
                alert("Registro exitoso. Ahora puedes iniciar sesión.");
                toggleForm(); // vuelve al login
                document.querySelector("#registerSection form")?.reset();
            } else {
                alert(result?.Message || "Error en el registro.");
            }
        } catch (error) {
            console.error("Error en registro:", error);
            alert("Ocurrió un error al registrarse.");
        }
    });
});
