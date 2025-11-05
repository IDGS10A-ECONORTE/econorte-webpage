document.addEventListener("DOMContentLoaded", async () => {
    const tableBody = document.querySelector("#sensorsTable tbody");
    const sensorCount = document.getElementById("sensorCount");
    const lastUpdate = document.getElementById("lastUpdate");

    //// URL del API (se mantiene, pero aún no se usa)
    //const userId = localStorage.getItem("userId") || 1;
    //const API_URL = `https://localhost:7168/Services/GetSensors/${userId}`;

    // Datos de ejemplo estáticos (estructura igual a la del backend)
    const sensores = [
        {
            Id_Sensor: 1,
            Name: "Sensor Sierra Verde",
            LastParameters: {
                Id_Sensor: 1,
                Date: "2025-10-16T14:32:00",
                Temperature: 34.5,
                Humidity: 55,
                Gas_Level: 12,
                Vibration: 0.2,
                Earthquake_Status: false,
                Fire_Status: false
            },
            LogParameters: [
                {
                    Id_Sensor: 1,
                    Date: "2025-10-16T14:32:00",
                    Temperature: 34.5,
                    Humidity: 55,
                    Gas_Level: 12,
                    Vibration: 0.2,
                    Earthquake_Status: false,
                    Fire_Status: false
                },
                {
                    Id_Sensor: 1,
                    Date: "2025-10-16T13:30:00",
                    Temperature: 33.8,
                    Humidity: 56,
                    Gas_Level: 10,
                    Vibration: 0.1,
                    Earthquake_Status: false,
                    Fire_Status: false
                },
                {
                    Id_Sensor: 1,
                    Date: "2025-10-16T12:30:00",
                    Temperature: 35.0,
                    Humidity: 54,
                    Gas_Level: 14,
                    Vibration: 0.3,
                    Earthquake_Status: false,
                    Fire_Status: false
                }
            ]
        },
        {
            Id_Sensor: 2,
            Name: "Sensor Bosque del Norte",
            LastParameters: {
                Id_Sensor: 2,
                Date: "2025-10-16T14:20:00",
                Temperature: 52.3,
                Humidity: 28,
                Gas_Level: 68,
                Vibration: 0.1,
                Earthquake_Status: false,
                Fire_Status: true
            },
            LogParameters: [
                {
                    Id_Sensor: 2,
                    Date: "2025-10-16T14:20:00",
                    Temperature: 52.3,
                    Humidity: 28,
                    Gas_Level: 68,
                    Vibration: 0.1,
                    Earthquake_Status: false,
                    Fire_Status: true
                },
                {
                    Id_Sensor: 2,
                    Date: "2025-10-16T13:15:00",
                    Temperature: 50.9,
                    Humidity: 31,
                    Gas_Level: 65,
                    Vibration: 0.2,
                    Earthquake_Status: false,
                    Fire_Status: false
                },
                {
                    Id_Sensor: 2,
                    Date: "2025-10-16T12:10:00",
                    Temperature: 49.7,
                    Humidity: 33,
                    Gas_Level: 58,
                    Vibration: 0.1,
                    Earthquake_Status: false,
                    Fire_Status: false
                }
            ]
        },
        {
            Id_Sensor: 3,
            Name: "Sensor Valle del Sol",
            LastParameters: {
                Id_Sensor: 3,
                Date: "2025-10-16T14:15:00",
                Temperature: 26.8,
                Humidity: 70,
                Gas_Level: 8,
                Vibration: 0.7,
                Earthquake_Status: true,
                Fire_Status: false
            },
            LogParameters: [
                {
                    Id_Sensor: 3,
                    Date: "2025-10-16T14:15:00",
                    Temperature: 26.8,
                    Humidity: 70,
                    Gas_Level: 8,
                    Vibration: 0.7,
                    Earthquake_Status: true,
                    Fire_Status: false
                },
                {
                    Id_Sensor: 3,
                    Date: "2025-10-16T13:10:00",
                    Temperature: 27.2,
                    Humidity: 69,
                    Gas_Level: 9,
                    Vibration: 0.6,
                    Earthquake_Status: false,
                    Fire_Status: false
                },
                {
                    Id_Sensor: 3,
                    Date: "2025-10-16T12:05:00",
                    Temperature: 26.5,
                    Humidity: 71,
                    Gas_Level: 8,
                    Vibration: 0.5,
                    Earthquake_Status: false,
                    Fire_Status: false
                }
            ]
        }
    ];

    // Actualizar resumen general
    sensorCount.textContent = sensores.length.toString();
    lastUpdate.textContent = new Date().toLocaleString();

    // Limpiar tabla antes de agregar filas
    tableBody.innerHTML = "";

    // Renderizar sensores
    sensores.forEach(sensor => {
        const last = sensor.LastParameters;

        // Si no hay datos recientes
        const temperatura = last?.Temperature ?? "N/A";
        const humedad = last?.Humidity ?? "N/A";
        const gas = last?.Gas_Level ?? "N/A";
        const fecha = last?.Date ? new Date(last.Date).toLocaleString() : "Sin datos";

        // Determinar estado (según valores de riesgo)
        let estado = "🟢 Activo";
        let estadoClass = "text-success fw-semibold";

        if (last?.Fire_Status === true || temperatura > 50 || gas > 50) {
            estado = "🔥 Riesgo de incendio";
            estadoClass = "text-danger fw-semibold";
        } else if (last?.Earthquake_Status === true || last?.Vibration > 0.5) {
            estado = "⚠️ Vibración detectada";
            estadoClass = "text-warning fw-semibold";
        }

        // Crear fila de la tabla
        const row = document.createElement("tr");
        row.innerHTML = `
        <td>${sensor.Id_Sensor}</td>
        <td>${sensor.Name}</td>
        <td>${temperatura}</td>
        <td>${humedad}</td>
        <td>${gas}</td>
        <td>${fecha}</td>
        <td class="${estadoClass}">${estado}</td>`;

        // Agregar evento para mostrar historial al hacer clic
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

            // Mostrar modal
            const modal = new bootstrap.Modal(document.getElementById("sensorHistoryModal"));
            const title = document.getElementById("sensorHistoryLabel");
            title.textContent = `Historial del sensor: ${sensor.Name} (${sensor.Id_Sensor})`;
            modal.show();
        });

        // Añadir fila a la tabla principal
        tableBody.appendChild(row);
    });
});