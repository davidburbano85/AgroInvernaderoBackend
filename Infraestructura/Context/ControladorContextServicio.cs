using invernaderoInteligenteBackend.Aplicacion.Interfaces.Context;

namespace invernaderoInteligenteBackend.Infraestructura.Context
{
    public class ControladorContextServicio : IControladorContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ControladorContextServicio(
            IHttpContextAccessor httpContextAccessor)
        {
            Console.WriteLine("[ControladorContext] Constructor");

            _httpContextAccessor = httpContextAccessor;
        }

        public Guid ObtenerTokenControlador()
        {
            Console.WriteLine("\n========== ControladorContext ==========");

            var contexto = _httpContextAccessor.HttpContext;

            Console.WriteLine($"HttpContext null: {contexto == null}");

            if (contexto != null)
            {
                Console.WriteLine($"Método: {contexto.Request.Method}");
                Console.WriteLine($"Ruta: {contexto.Request.Path}");
                Console.WriteLine($"Es WebSocket: {contexto.WebSockets.IsWebSocketRequest}");

                Console.WriteLine("\n----- HEADERS -----");

                foreach (var header in contexto.Request.Headers)
                {
                    Console.WriteLine($"{header.Key}: {header.Value}");
                }

                Console.WriteLine("-------------------");
            }

            var token = contexto?
                .Request
                .Headers["X-Device-Token"]
                .FirstOrDefault();

            Console.WriteLine($"Header X-Device-Token: {token}");

            if (string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine("ERROR: No llegó el header X-Device-Token.");
                throw new Exception("No se pudo obtener el token del controlador.");
            }

            bool valido = Guid.TryParse(token, out var deviceToken);

            Console.WriteLine($"Guid válido: {valido}");

            if (!valido)
            {
                Console.WriteLine("ERROR: El token no tiene formato GUID.");
                throw new Exception("No se pudo obtener el token del controlador.");
            }

            Console.WriteLine($"GUID obtenido: {deviceToken}");
            Console.WriteLine("========== FIN ControladorContext ==========\n");

            return deviceToken;
        }
    }
}