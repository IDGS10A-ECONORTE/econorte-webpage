using Econorte.Services.Data;
using Econorte.Services.Models;

namespace Econorte.Services.Services
{
    public class SensorsParametersServices
    {
        private readonly Econorte_Context _db;

        public SensorsParametersServices(Econorte_Context db)
        {
            _db = db;
        }

        //Método que registra un nuevo parámetro para un sensor
        public Response Add(Parameters parameters)
        {
            Response response = new();
            try
            {
                var existingSensor = _db.Sensors
                    .FirstOrDefault(s => s.Id_Sensor == parameters.Id_Sensor);

                if (existingSensor != null) 
                {
                    //valida que el parámetro del sensor sea diferente a null
                    if (parameters != null)
                    {
                        var NewSensorParameter = new SensorsParameters
                        {
                            fk_Sensor = parameters.Id_Sensor,
                            Date = parameters.Date,
                            Temperature = parameters.Temperature,
                            Humidity = parameters.Humidity,
                            Gas_Level = parameters.Gas_Level,
                            Vibration = parameters.Vibration,
                            Earthquake_Status = parameters.Earthquake_Status,
                            Fire_Status = parameters.Fire_Status
                        };

                        //Agrega el nuevo parámetro del sensor a la base de datos
                        _db.SensorsParameters.Add(NewSensorParameter);
                        _db.SaveChanges();

                        //Respuesta para el cliente y manejo de alertas
                        response.Success = true;
                        response.Message = "Parámetro del sensor registrado correctamente";
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "El parámetro del sensor se nulo o no existe";
                    }
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Ocurrió un error al registrar el parámetro del sensor: " + ex.Message;
            }
            return response;
        }
    }
}