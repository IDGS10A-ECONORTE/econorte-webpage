using Econorte.Services.Data;
using Econorte.Services.Models;

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
                    //Guarda los cambios en la base de datos
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
        public List<Sensors> Get(int Id)
        {
            List<Sensors> sensors = new();
            try
            {
                //Obtiene todos los sensores del usuario
                sensors = _db.Sensors
                    .Where(s => s.fk_User == Id)
                    .ToList();
            }
            catch (Exception)
            {
                //En caso de error, devuelve una lista vacía
                sensors = new List<Sensors>();
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
                    //Guarda los cambios en la base de datos
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
                    existingSensor.Temperature = sensor.Temperature;
                    existingSensor.Humidity = sensor.Humidity;
                    existingSensor.Gas_Level = sensor.Gas_Level;
                    existingSensor.Vibration = sensor.Vibration;
                    existingSensor.Earthquake_Status = sensor.Earthquake_Status;
                    existingSensor.Fire_Status = sensor.Fire_Status;
                    existingSensor.Alarm_Intensity = sensor.Alarm_Intensity;
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