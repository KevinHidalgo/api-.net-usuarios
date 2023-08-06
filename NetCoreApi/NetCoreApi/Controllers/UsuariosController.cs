using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreApi.Models;
using System.Security.Claims;

namespace NetCoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private PridesContext context = new PridesContext();

        [HttpGet]
        [Route("listar")]
        public dynamic listarUsuario() {
          /*  var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validarToken(identity);

            if (!rToken.success) return rToken;*/

        //    var context = new PridesContext();
      
            return context.Usuarios.ToList();
        }

       [HttpGet]
        [Route("listarId")]
        public dynamic listarUsuarioId(int id)
        {

       //     var context = new PridesContext();
            var user = context.Usuarios.Find(id);
            return user;

        } 

        [HttpPost]
        [Route("guardar")]
        public dynamic guardarUsuario(Usuario usuario)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validarToken(identity);

            if (!rToken.success) return rToken;

        //    var context = new PridesContext();
            var user = new Usuario();
          
            user.Nombre = usuario.Nombre;
            user.Clave = usuario.Clave; 
            user.IdRol = usuario.IdRol;
            context.Usuarios.Add(user);
            context.SaveChanges();

            return new
            {
                success = true,
                status = 201,
                message = "Usuario Registrado",
                result = user
            };
        }

        [HttpPut]
        [Route("modificar")]
        public dynamic modificarUsuario(int id, Usuario usuario)
        {
           var identity = HttpContext.User.Identity as ClaimsIdentity;
           var rToken = Jwt.validarToken(identity);

            if (!rToken.success) return rToken;

        //    var context = new PridesContext();
            var user = context.Usuarios.Find(id);

            user.Nombre = usuario.Nombre;
            user.Clave = usuario.Clave;
            user.IdRol = usuario.IdRol;

            context.Entry(user).State=EntityState.Modified;
            context.SaveChanges();

            return new
            {
                success = true,
                status = 201,
                message = "Usuario Modificado",
                result = user
            };
        }

        [HttpDelete]
        [Route("eliminar")]
        public dynamic eliminarUsuario(int id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validarToken(identity);

            if (!rToken.success) return rToken;

          /*  Usuario usuario = rToken.result;
            if (usuario.IdRol == 2) 
            {
                return new
                {
                    success = false,
                    message = "No tienes permisos para eliminar usuarios",
                    result = ""
                };
            } */

         //   var context = new PridesContext();
            var user = context.Usuarios.Find(id);
            context.Remove(user);
            context.SaveChanges();

            return new
            {
                success = true,
                status = 201,
                message = "Usuario Eliminado"
            };
        }
      
    }
}
