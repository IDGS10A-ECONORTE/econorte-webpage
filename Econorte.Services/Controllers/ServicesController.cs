using Microsoft.AspNetCore.Mvc;
using Econorte.Services.Models;
using Econorte.Services.Services;
using Econorte.Services.Data;
using Microsoft.AspNetCore.Authorization;

namespace Econorte.Services.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly LoginServices _loginServices;
        private readonly SensorsServices _sensorsServices;
        private readonly SensorsParametersServices _sensorsParametersServices;

        public ServicesController(
            LoginServices loginServices,
            SensorsServices sensorsServices,
            SensorsParametersServices sensorsParametersServices
            )
        {
            _loginServices = loginServices;
            _sensorsServices = sensorsServices;
            _sensorsParametersServices = sensorsParametersServices;
        }

        //Servicios de LoginServices---------------------------------------------------------------------------------------------------------------
        //Validar las credenciales del usuario
        [AllowAnonymous]
        [HttpPost("Login")]
        public object Login([FromBody] Credentials credentials)
        {
            var user = _loginServices.Login(credentials);
            return user;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public object Register([FromBody] Users user)
        {
            Response response = _loginServices.CreateUser(user);
            return response;
        }

        [AllowAnonymous]
        [HttpPost("Logout")]
        public object Logout([FromBody] Credentials credentials)
        {
            Response response = _loginServices.Logout(credentials);
            return response;
        }

        //Llama al método del servicio LoginServices que cierra todas las sesiones activas
        [HttpPost("CloseSessions")]
        public void CloseSessions()
        {
            _loginServices.CloseSessions();
        }

        //Servicios de SensorsServices---------------------------------------------------------------------------------------------------------------
        //Registrar un nuevo sensor
        [HttpPost("RegisterSensor")]
        public object RegisterSensor([FromBody] Sensors sensor)
        {
            //Llama al método del servicio SensorsServices que registra un nuevo sensor
            Response response = _sensorsServices.Create(sensor);
            return response;
        }

        //Obtener todos los sensores de un usuario
        [HttpGet("GetSensors/{Id}")]
        public object GetSensors(int Id)
        {
            //Llama al método del servicio SensorsServices que obtiene todos los sensores de un usuario
            var sensors = _sensorsServices.Get(Id);
            return sensors;
        }

        //Eliminar un sensor
        [HttpDelete("DeleteSensor/{Id}")]
        public object DeleteSensor(int Id)
        {
            //Llama al método del servicio SensorsServices que elimina un sensor
            Response response = _sensorsServices.Delete(Id);
            return response;
        }

        //Actualizar datos de un sensor
        [HttpPut("UpdateSensor")]
        public object UpdateSensor([FromBody] Sensors sensor)
        {
            //Llama al método del servicio SensorsServices que actualiza los datos de un sensor
            Response response = _sensorsServices.Update(sensor);
            return response;
        }

        //Servicios de SensorsParametersServices---------------------------------------------------------------------------------------------------------------
        [HttpPost("AddSensorParameter")]
        public object AddSensorParameter([FromBody] Parameters sensorParameter)
        {
            //Llama al método del servicio SensorsParametersServices que registra un nuevo parámetro para un sensor
            Response response = _sensorsParametersServices.Add(sensorParameter);
            return response;
        }
    }
}