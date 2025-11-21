document.addEventListener("DOMContentLoaded", async () => {
    const tableBody = document.querySelector("#sensorsTable tbody");
    const sensorCount = document.getElementById("sensorCount");
    const lastUpdate = document.getElementById("lastUpdate");

    // Mostrar datos en la vista
    if (user) document.getElementById("userName").textContent = `Bienvenido ${user.Name}` ?? "Bienvenido";

    let sensores = [];

    axios.post(callApiAsync, {
        IdApi: 6,
        Param: `${user.Id_User}`,
        Token: token,
        BodyParams: null
    })
        .then(response => {
            sensores = response.data;
            console.log(sensores);
        })
        .catch(error => console.error(error));

    // Actualizar resumen general
    sensorCount.textContent = sensores.length.toString();
    lastUpdate.textContent = new Date().toLocaleString();

    // Limpiar tabla antes de agregar filas
    tableBody.innerHTML = "";

    // Renderizar sensores
    // Verificar si no hay sensores
    if (!sensores || sensores.length === 0) {
        tableBody.innerHTML = `
        <tr>
            <td colspan="7" class="text-center text-muted py-3">
                No hay datos disponibles.
            </td>
        </tr>`;
        return;
    }

    // Recorrer sensores
    sensores.forEach(sensor => {
        const last = sensor.LastParameters;

        const temperatura = last?.Temperature ?? "N/A";
        const humedad = last?.Humidity ?? "N/A";
        const gas = last?.Gas_Level ?? "N/A";
        const fecha = last?.Date ? new Date(last.Date).toLocaleString() : "Sin datos";

        let estado = "🟢 Activo";
        let estadoClass = "text-success fw-semibold";

        if (last?.Fire_Status === true || temperatura > 50 || gas > 50) {
            estado = "🔥 Riesgo de incendio";
            estadoClass = "text-danger fw-semibold";
        } else if (last?.Earthquake_Status === true || last?.Vibration > 0.5) {
            estado = "⚠️ Vibración detectada";
            estadoClass = "text-warning fw-semibold";
        }

        const row = document.createElement("tr");
        row.innerHTML = `
        <td>${sensor.Id_Sensor}</td>
        <td>${sensor.Name}</td>
        <td>${temperatura}</td>
        <td>${humedad}</td>
        <td>${gas}</td>
        <td>${fecha}</td>
        <td class="${estadoClass}">${estado}</td>
    `;

        // Eventos y modal...
        row.addEventListener("click", () => {
            const tbody = document.getElementById("historyTableBody");
            tbody.innerHTML = "";

            const historicos = sensor.LogParameters || [];

            if (historicos.length === 0) {
                tbody.innerHTML = `
                <tr>
                    <td colspan="6" class="text-center text-muted py-3">
                        Sin datos históricos.
                    </td>
                </tr>`;
            } else {
                historicos.forEach(reg => {
                    const estadoHist = reg.Fire_Status
                        ? "🔥 Riesgo de incendio"
                        : reg.Earthquake_Status
                            ? "⚠️ Vibración detectada"
                            : "✅ Normal";

                    const rowHist = document.createElement("tr");
                    rowHist.innerHTML = `
                    <td>${new Date(reg.Date).toLocaleString()}</td>
                    <td>${reg.Temperature}</td>
                    <td>${reg.Humidity}</td>
                    <td>${reg.Gas_Level}</td>
                    <td>${reg.Vibration ?? "N/A"}</td>
                    <td>${estadoHist}</td>
                `;
                    tbody.appendChild(rowHist);
                });
            }

            const modal = new bootstrap.Modal(document.getElementById("sensorHistoryModal"));
            const title = document.getElementById("sensorHistoryLabel");
            title.textContent = `Historial del sensor: ${sensor.Name} (${sensor.Id_Sensor})`;
            modal.show();
        });

        tableBody.appendChild(row);
    });
});