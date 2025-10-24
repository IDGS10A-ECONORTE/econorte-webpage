document.addEventListener("DOMContentLoaded", async () => {
    const tableBody = document.querySelector("#sensorsTable tbody");
    const sensorCount = document.getElementById("sensorCount");
    const lastUpdate = document.getElementById("lastUpdate");

    // 🟢 Simulación de datos (puedes reemplazar con fetch a tu API)
    const userId = 1; // Ejemplo: ID del usuario autenticado
    const API_URL = `https://tu-servidor-api.com/api/sensores?userId=${userId}`;

    // Si tuvieras un backend real, descomenta el bloque fetch 👇
    /*
    try {
        const response = await fetch(API_URL);
        if (!response.ok) throw new Error("Error al obtener sensores");
        var sensores = await response.json();
    } catch (error) {
        console.error("Error cargando sensores:", error);
        var sensores = []; // Vacío si falla
    }
    */

    //const response = await axios.get('https://localhost:7168/Services/GetSensors/1');

    // Datos simulados mientras no hay backend
    const sensores = [
        { id: "SEN-001", ubicacion: "Bosque El Águila", temperatura: 34.2, humedad: 45, gas: "Normal", ultimaLectura: "2025-10-16 14:32", estado: "Activo" },
        { id: "SEN-002", ubicacion: "Zona Norte", temperatura: 38.5, humedad: 31, gas: "Alerta", ultimaLectura: "2025-10-16 14:29", estado: "En riesgo" },
        { id: "SEN-003", ubicacion: "Sierra Verde", temperatura: 29.1, humedad: 62, gas: "Normal", ultimaLectura: "2025-10-16 14:27", estado: "Activo" }
    ];

    // 🧮 Actualizar resumen
    sensorCount.textContent = sensores.length.toString();
    lastUpdate.textContent = new Date().toLocaleString();

    // 🧾 Limpiar tabla antes de agregar filas
    tableBody.innerHTML = "";

    if (sensores.length === 0) {
        const emptyRow = document.createElement("tr");
        emptyRow.innerHTML = `<td colspan="7" class="text-center text-muted py-3">No hay sensores asignados a este usuario.</td>`;
        tableBody.appendChild(emptyRow);
        return;
    }

    // 🧩 Renderizar sensores en la tabla
    sensores.forEach(sensor => {
        const row = document.createElement("tr");

        // Determinar color del estado
        let estadoClass = "text-success fw-semibold";
        if (sensor.estado === "En riesgo") estadoClass = "text-danger fw-semibold";
        else if (sensor.estado === "Inactivo") estadoClass = "text-secondary fw-semibold";

        row.innerHTML = `
            <td>${sensor.id}</td>
            <td>${sensor.ubicacion}</td>
            <td>${sensor.temperatura.toFixed(1)}</td>
            <td>${sensor.humedad}%</td>
            <td>${sensor.gas}</td>
            <td>${sensor.ultimaLectura}</td>
            <td class="${estadoClass}">${sensor.estado}</td>
        `;

        tableBody.appendChild(row);
    });
});
