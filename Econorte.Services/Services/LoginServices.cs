using Econorte.Services.Data;
using Econorte.Services.Models;

namespace Econorte.Services.Services
{
    public class LoginServices
    {
        private readonly Econorte_Context _db;
        public LoginServices(
            Econorte_Context db
            )
        {
            _db = db;
        }

        //Método que valida las credenciales que manda el cliente
        public Users Login(Credentials credentials)
        {
            //Modelo de usuario vacío
            Users user = new();

            //valida que el usuario sea diferente a null
            if (credentials != null)
            {
                //Busca al usuario en la base de datos
                var row = _db.Users
                    .Where(u => u.Email == credentials.Email && u.Active)
                    .FirstOrDefault();

                //valida que el usuario sea diferente a null
                if (row != null)
                {
                    //valida que la contraseña del usuario enviado sean las mismas que del usuario encontrado
                    if (credentials.Password == row.Password)
                    {
                        //Mapear Usuario
                        user.Id_User = row.Id_User;
                        user.Name = row.Name;
                        user.Email = row.Email;
                        user.Phone = row.Phone;
                        user.fk_Role = row.fk_Role;
                        user.Active = row.Active;

                        ////Respuesta para el cliente y manejo de alertas
                        //response.Success = true;

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
                    .Where(u => u.Email == credentials.Email)
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

            //Por defecto esta propiedad será False
            response.Success = false;

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
                    Users NewUser = new()
                    {
                        Name = user.Name,
                        Email = user.Email,
                        Password = user.Password,
                        Phone = user.Phone,
                        fk_Role = 2,
                        Active = true,
                    };

                    //Se manda esta propiedad en caso de querer utilizarla para activar una alerta Success exitosa
                    response.Success = true;

                    _db.Users.Add(NewUser);
                    _db.SaveChanges();
                }
                else
                {
                    //De lo contrario, Se añade este mensaje para advertir que ya existe un usuario con ese Email.
                    response.Message = $"Ya existe un usuario registrado con este email '{user.Email}', favor de introducir un email diferente";
                }
            }
            return response;
        }

        public void CloseSessions()
        {
            List<Users> users = _db.Users.Where(u => u.Login).ToList();

            if (users.Count > 0)
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