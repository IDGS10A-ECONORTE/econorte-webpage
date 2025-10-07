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

        public ServicesController(
            ILogger<ServicesController> logger,
            LoginServices loginServices
            )
        {
            _logger = logger;
            _loginServices = loginServices;
        }

        //Servicios de LoginServices---------------------------------------------------------------------------------------------------------------
        //Validar las credenciales del usuario
        [HttpPost(Name = "Login")]
        public object Login([FromBody] Credentials credentials)
        {
            //Llama al método del servicio UserServices que actualiza los datos de un usuario existente
            var user = _loginServices.Login(credentials);
            return user;
        }

        //Cerrar Sesión
        [HttpPost(Name = "Logout")]
        public object Logout([FromBody] Credentials credentials)
        {
            //Llama al método del servicio UserServices que actualiza los datos de un usuario existente
            Response response = _loginServices.Logout(credentials);
            return response;
        }

        [HttpPost(Name = "Register")]
        public object Register([FromBody] Users user)
        {
            //Llama al método del servicio UserServices que actualiza los datos de un usuario existente
            Response response = _loginServices.CreateUser(user);
            return response;
        }

        //Llama al método del servicio LoginServices que cierra todas las sesiones activas
        [HttpPost(Name = "CloseSessions")]
        public void CloseSessions()
        {
            _loginServices.CloseSessions();
        }
    }
}
