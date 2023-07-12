using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetCoreApi.Models;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NetCoreApi.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        public IConfiguration _configuration;
        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public dynamic IniciarSesion([FromBody] Object optData)
        {
            var data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());
            var context = new PridesContext();

            string user = data.nombre.ToString();
            string password = data.clave.ToString();

            var usuario = context.Usuarios.Where(x => x.Nombre == user && x.Clave == password).FirstOrDefault();
            if (usuario == null) 
            {
                return new
                {
                    success = false,
                    message = "Credenciales Incorrectas",
                    status = 400,
                    result = ""
                };
            }

            var jwt = _configuration.GetSection("Jwt").Get<Jwt>();
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,jwt.Subject),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                new Claim("clave",usuario.Clave),
                new Claim("usuario",usuario.Nombre)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                jwt.Issuer,
                jwt.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: singIn
                );

            var context2 = new PridesContext();
            var sesion = new Sesione();
            Random rnd = new Random();

            var num = rnd.Next(3, 5000 + 1);

            var sesionId = context2.Sesiones.Where(x => x.IdSesion == num).FirstOrDefault();
            if (sesionId == null)
            {
                sesion.IdSesion = num;
            }
            else {
                 num = rnd.Next(3, 5000 + 1);
                 sesionId = context2.Sesiones.Where(x => x.IdSesion == num).FirstOrDefault();
                 sesion.IdSesion = num;
            }
                
            sesion.IdUsuario = usuario.IdUsuario;
        //    sesion.Token = new JwtSecurityTokenHandler().WriteToken(token);
            sesion.FechaInicio = DateTime.UtcNow;
            context2.Sesiones.Add(sesion);
            context2.SaveChanges();

            return new
            {
                success = true,
                message = "Autentico",
                status = 200,
                result = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }

    }
}
