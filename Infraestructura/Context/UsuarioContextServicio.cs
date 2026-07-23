using invernaderoInteligenteBackend.Aplicacion.Interfaces.Context;
using System.Security.Claims;

namespace invernaderoInteligenteBackend.Infraestructura.Context
{
    public class UsuarioContextServicio: IUsuarioContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UsuarioContextServicio(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public Guid ObtenerAuthUserId()
        {
            var usuario = _httpContextAccessor.HttpContext?.User;
           var sub = usuario?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? usuario?.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(sub)|| !Guid.TryParse(sub, out var userId))
                throw new Exception("No se pudo obtener el ID del usuario autenticado.");

            return userId;
        }
    }
}
