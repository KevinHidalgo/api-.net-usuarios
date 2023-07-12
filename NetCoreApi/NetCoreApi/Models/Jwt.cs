using System.Security.Claims;

namespace NetCoreApi.Models
{
    public class Jwt
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }

        public static dynamic validarToken(ClaimsIdentity identity)
        {
            try
            {
                if(identity.Claims.Count() == 0)
                {
                    return new
                    {
                        success = false,
                        message = "verificar token valido",
                        result = ""
                    };
                }
                var clave = identity.Claims.FirstOrDefault(x=>x.Type == "clave").Value;
                var context = new PridesContext();
                Usuario usuario = context.Usuarios.FirstOrDefault(x => x.Clave == clave);
                return new
                {
                    success = true,
                    message = "exito",
                    result = usuario
                };
            }
            catch (Exception ex) 
            {
                return new
                {
                    success= false,
                    message= "Catch: "+ex.Message,
                    result=""
                };
            }
        }
    }
}
