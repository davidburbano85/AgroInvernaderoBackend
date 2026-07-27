using invernaderoInteligenteBackend.Aplicacion.Interfaces.IWebSockets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;

namespace invernaderoInteligenteBackend.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WebSocketControlController : ControllerBase
    {
        private readonly IWebSocketServicio _webSocketServicio;

        public WebSocketControlController(
            IWebSocketServicio webSocketServicio)
        {
            _webSocketServicio = webSocketServicio;
        }




        [HttpGet("conectar")]
        public async Task Conectar()
        {
            Console.WriteLine("[1] Entró al endpoint.");

            Console.WriteLine("[2] IsWebSocketRequest = " + HttpContext.WebSockets.IsWebSocketRequest);

            if (!HttpContext.WebSockets.IsWebSocketRequest)
            {
                Console.WriteLine("[3] No es una petición WebSocket.");
                HttpContext.Response.StatusCode = 400;
                return;
            }

            Console.WriteLine("[4] Aceptando conexión...");

            WebSocket conexion =
                await HttpContext.WebSockets.AcceptWebSocketAsync();

            Console.WriteLine("[5] Conexión aceptada.");

            Console.WriteLine("[6] Llamando RegistrarConexionAsync...");

            await _webSocketServicio.RegistrarConexionAsync(conexion);

            Console.WriteLine("[7] Registro completado.");

            var buffer = new byte[4096];

            while (conexion.State == WebSocketState.Open)
            {
                Console.WriteLine("[8] Esperando mensaje...");

                var resultado = await conexion.ReceiveAsync(
                    new ArraySegment<byte>(buffer),
                    CancellationToken.None
                );

                Console.WriteLine("[9] Mensaje recibido.");
            }
        }



    }
}