using Econorte.Services.Data;
using Econorte.Services.Models;
using Microsoft.EntityFrameworkCore;
#pragma warning disable CS8618

namespace Econorte.Services.Services
{
    public class LoginServices
    {
        private readonly Econorte_DevContext _db;
        public LoginServices(Econorte_DevContext db) { 
            _db = db;
        }

        //Método que valida las credenciales que manda el cliente
        public object Login(Credentials credentials)
        {
            //Modelo de usuario vacío
            object user = new();

            //valida que el usuario sea diferente a null
            if (credentials != null)
            {
                //Busca al usuario en la base de datos
                var row = _db.Users
                    .Where(u => u.Email == credentials.email && u.Active)
                    .FirstOrDefault();

                //valida que el usuario sea diferente a null
                if (row != null)
                {
                    //valida que la contraseña del usuario enviado sean las mismas que del usuario encontrado
                    if (credentials.password == row.Password)
                    {
                        //Mapear Usuario
                        user = new 
                        {
                            row.Id_User,
                            row.Name,
                            row.Email,
                            row.Phone,
                            row.fk_Role,
                            row.Active,
                        };
                        //propiedad para validar que el usuario tenga sesión iniciada
                        row.Login = true;
                        row.LastLog = DateTime.Now;

                        //guardamos los cambios en la base de datos
                        _db.SaveChanges();
                    }
                }
            }
            return user;
        }

        public Response Logout(Credentials credentials) 
        {
            Response response = new();

            if (credentials != null) 
            {
                var row = _db.Users
                    .Where(u => u.Email == credentials.email)
                    .FirstOrDefault();

                if (row != null) 
                { 
                    row.Login = false;
                    _db.SaveChanges();

                    response.Message = "Se ha cerrado sesión";
                    response.Success = true;
                }
            }
            return response;
        }

        //Método para crear un usuario
        public Response CreateUser(Users user)
        {

            Response response = new();

            //Manejo en caso de nulos
            if (user != null)
            {
                var row = _db.Users
                    .Where(e => e.Email == user.Email)
                    .FirstOrDefault();

                /*
                 En caso de que no se encuentre un usuario
                 se crea el objeto en la base de datos.
                 */
                if (row == null)
                {

                    //Se crea el objeto con las clases de la base de datos
                    Users NewUser = new Users
                    {
                        Name = user.Name,
                        Email = user.Email,
                        Password = user.Password,
                        Phone = user.Phone,
                        fk_Role = 2,
                        Active = true,
                        Login = false,
                    };

                    _db.Users.Add(NewUser);
                    _db.SaveChanges();

                    //Se manda esta propiedad en caso de querer utilizarla para activar una alerta Success exitosa
                    response.Success = true;
                }
                else response.Message = $"Ya existe un usuario registrado con este email '{user.Email}', favor de introducir un email diferente";
            }
            return response;
        }

        public void CloseSessions()
        {
            List<Users> users = _db.Users.Where(u => u.Login).ToList();

            if(users.Count > 0)
            {
                foreach (var user in users)
                {
                    if (user.LastLog != null) if (CalculateDays(user.LastLog.Value) >= 10) user.Login = false;
                    else user.Login = false;
                }
                _db.SaveChanges();
            }
        }

        static int CalculateDays(DateTime date)
        {
            // Fecha actual sin hora
            DateTime fechaActual = DateTime.Now.Date;

            // Fecha recibida sin hora
            DateTime fechaParametro = date.Date;

            // Calcular diferencia
            TimeSpan diferencia = fechaActual - fechaParametro;

            // Retornar días transcurridos (puede ser negativo si la fecha es futura)
            return diferencia.Days;
        }
    }
}