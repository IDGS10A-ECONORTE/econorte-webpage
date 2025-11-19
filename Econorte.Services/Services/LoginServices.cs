using Econorte.Services.Data;
using Econorte.Services.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Econorte.Services.Services
{
    public class LoginServices
    {
        private readonly Econorte_Context _db;
        private readonly IConfiguration _config;

        public LoginServices(
            Econorte_Context db,
            IConfiguration config
            )
        {
            _db = db;
            _config = config;
        }

        //Método que valida las credenciales que manda el cliente
        public AuthenticatedUser Login(Credentials credentials)
        {
            if (credentials == null)
                return new();

            var row = _db.Users
                .FirstOrDefault(u => u.Email == credentials.Email && u.Active);

            if (row == null)
                return new();

            // Validar contraseña usando hash
            if (!BCrypt.Net.BCrypt.Verify(credentials.Password, row.Password))
                return new();

            // Crear token
            var token = GenerateJwt(row);

            return new()
            {
                Id_User = row.Id_User,
                Name = row.Name,
                Email = row.Email,
                Role = row.fk_Role.ToString(),
                Token = token
            };
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

                if (row == null)
                {

                    //Se crea el objeto con las clases de la base de datos
                    Users NewUser = new()
                    {
                        Name = user.Name,
                        Email = user.Email,
                        Password = BCrypt.Net.BCrypt.HashPassword(user.Password),   // <-- Guardar hasheado
                        Phone = user.Phone,
                        fk_Role = 2,
                        Active = true,
                    };
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

        public string GenerateJwt(Users user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? ""));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id_User.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.fk_Role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public void CloseSessions()
        {
            var users = _db.Users.Where(u => u.Login).ToList();

            foreach (var user in users)
            {
                if (user.LastLog != null)
                {
                    if (CalculateDays(user.LastLog.Value) >= 10)
                        user.Login = false;
                }
            }

            _db.SaveChanges();
        }

        static int CalculateDays(DateTime date)
        {
            // Fecha actual sin hora
            DateTime currentDate = DateTime.Now.Date;

            // Fecha recibida sin hora
            DateTime parameterDate = date.Date;

            // Calcular diferencia
            TimeSpan difference = currentDate - parameterDate;

            // Retornar días transcurridos (puede ser negativo si la fecha es futura)
            return difference.Days;
        }
    }
}