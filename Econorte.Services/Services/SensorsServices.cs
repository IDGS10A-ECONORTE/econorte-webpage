using Econorte.Services.Data;
using Econorte.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace Econorte.Services.Services
{
    public class SensorsServices
    {
        private readonly Econorte_Context _db;
        public SensorsServices(Econorte_Context db)
        {
            _db = db;
        }

        //Método que registra un nuevo sensor
        public Response Create(Sensors sensor)
        {
            Response response = new();
            try
            {
                //valida que el sensor sea diferente a null
                if (sensor != null)
                {
                    //Agrega el nuevo sensor a la base de datos
                    _db.Sensors.Add(sensor);
                    _db.SaveChanges();

                    //Respuesta para el cliente y manejo de alertas
                    response.Success = true;
                    response.Message = "Sensor registrado correctamente";
                }
                else
                {
                    response.Success = false;
                    response.Message = "El sensor no puede ser nulo";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Ocurrió un error al registrar el sensor: " + ex.Message;
            }
            return response;
        }

        //Método que obtiene todos los sensores de un usuario
        public object Get(int Id)
        {
            var sensors = new List<Sensor>();
            try
            {
                //Obtiene todos los sensores del usuario
                sensors = _db.Sensors
                    .Where(s => s.fk_User == Id)
                    .Select(s => new Sensor
                    {
                        Id_Sensor = s.Id_Sensor,
                        Name = s.Name,
                    })
                    .ToList();

                foreach (var sensor in sensors)
                {
                    //Obtiene el último parámetro del sensor
                    var lastParameters = _db.SensorsParameters
                        .Where(sp => sp.fk_Sensor == sensor.Id_Sensor)
                        .OrderByDescending(sp => sp.Date)
                        .FirstOrDefault();

                    if (lastParameters != null)
                    {
                        sensor.LastParameters = new Parameters
                        {
                            Id_Sensor = lastParameters.fk_Sensor,
                            Date = lastParameters.Date ?? new(),
                            Temperature = lastParameters.Temperature ?? new(),
                            Humidity = lastParameters.Humidity ?? new(),
                            Gas_Level = lastParameters.Gas_Level ?? new(),
                            Vibration = lastParameters.Vibration ?? new(),
                            Earthquake_Status = lastParameters.Earthquake_Status ?? new(),
                            Fire_Status = lastParameters.Fire_Status ?? new(),
                        };
                    }

                    //Obtiene todos los parámetros del sensor
                    var parameters = _db.SensorsParameters
                        .Where(sp => sp.fk_Sensor == sensor.Id_Sensor)
                        .Select(sp => new Parameters
                        {
                            Id_Sensor = sp.fk_Sensor,
                            Date = sp.Date ?? new(),
                            Temperature = sp.Temperature ?? new(),
                            Humidity = sp.Humidity ?? new(),
                            Gas_Level = sp.Gas_Level ?? new(),
                            Vibration = sp.Vibration ?? new(),
                            Earthquake_Status = sp.Earthquake_Status ?? new(),
                            Fire_Status = sp.Fire_Status ?? new(),
                        })
                        .ToList();
                    sensor.LogParameters = parameters;
                }
            }
            catch (Exception)
            {
                //En caso de error, devuelve una lista vacía
                sensors = new();
            }
            return sensors;
        }

        //Método que elimina un sensor
        public Response Delete(int Id)
        {
            Response response = new();
            try
            {
                //Busca el sensor en la base de datos
                var sensor = _db.Sensors
                    .Where(s => s.Id_Sensor == Id)
                    .FirstOrDefault();
                //valida que el sensor sea diferente a null
                if (sensor != null)
                {
                    //Elimina el sensor de la base de datos
                    _db.Sensors.Remove(sensor);
                    _db.SaveChanges();

                    //Respuesta para el cliente y manejo de alertas
                    response.Success = true;
                    response.Message = "Sensor eliminado correctamente";
                }
                else
                {
                    response.Success = false;
                    response.Message = "El sensor no existe";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Ocurrió un error al eliminar el sensor: " + ex.Message;
            }
            return response;
        }

        //Método que actualiza un sensor
        public Response Update(Sensors sensor)
        {
            Response response = new();
            try
            {
                //Busca el sensor en la base de datos
                var existingSensor = _db.Sensors
                    .Where(s => s.Id_Sensor == sensor.Id_Sensor)
                    .FirstOrDefault();
                //valida que el sensor sea diferente a null
                if (existingSensor != null)
                {
                    //Actualiza los datos del sensor
                    existingSensor.Name = sensor.Name;
                    //Guarda los cambios en la base de datos
                    _db.SaveChanges();
                    //Respuesta para el cliente y manejo de alertas
                    response.Success = true;
                    response.Message = "Sensor actualizado correctamente";
                }
                else
                {
                    response.Success = false;
                    response.Message = "El sensor no existe";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Ocurrió un error al actualizar el sensor: " + ex.Message;
            }
            return response;
        }
    }
}