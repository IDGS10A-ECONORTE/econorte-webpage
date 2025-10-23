using Microsoft.AspNetCore.Mvc;
using Econorte.Services.Models;
using Econorte.Services.Services;
using Econorte.Services.Data;

namespace Econorte.Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly ILogger<ServicesController> _logger;
        private readonly LoginServices _loginServices;
        private readonly SensorsServices _sensorsServices;
        private readonly Econorte_Context _db;

        public ServicesController(
            ILogger<ServicesController> logger,
            LoginServices loginServices,
            SensorsServices sensorsServices,
            Econorte_Context db
            )
        {
            _logger = logger;
            _loginServices = loginServices;
            _sensorsServices = sensorsServices;
            _db = db;
        }

        //Servicios de LoginServices---------------------------------------------------------------------------------------------------------------
        //Validar las credenciales del usuario
        [HttpPost("Login")]
        public object Login([FromBody] Credentials credentials)
        {
            //Llama al método del servicio UserServices que actualiza los datos de un usuario existente
            var user = _loginServices.Login(credentials);
            return user;
        }

        //Cerrar Sesión
        [HttpPost("Logout")]
        public object Logout([FromBody] Credentials credentials)
        {
            //Llama al método del servicio UserServices que actualiza los datos de un usuario existente
            Response response = _loginServices.Logout(credentials);
            return response;
        }

        [HttpPost("Register")]
        public object Register([FromBody] Users user)
        {
            //Llama al método del servicio UserServices que actualiza los datos de un usuario existente
            Response response = _loginServices.CreateUser(user);
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
    }
}