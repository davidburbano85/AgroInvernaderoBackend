using invernaderoInteligenteBackend.Aplicacion.Interfaces.IWebSockets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace invernaderoInteligenteBackend.Api.Controllers
{

    // Indicamos que esta clase es un controlador de API.
    [Authorize]
    [ApiController]

    // Ruta base para acceder a este controlador.
    [Route("api/[controller]")]
    public class WebSocketController : ControllerBase
    {


        // Variable donde guardaremos nuestro servicio.
        private readonly IWebSocketServicio _webSocketServicio;



        // Constructor.
        // ASP.NET inyectará automáticamente el servicio.
        public WebSocketController(IWebSocketServicio webSocketServicio)
        {

            // Guardamos la referencia del servicio.
            _webSocketServicio = webSocketServicio;

        }



        // Endpoint para enviar un mensaje a un controlador IoT.
        //
        // Ejemplo:
        // POST:
        // api/WebSocket/enviar/ESP32-001
        //
        // Body:
        // "MOVER_MOTOR"
        [HttpPost("enviar/{idControlador}")]
        public async Task<IActionResult> EnviarMensaje(
            string idControlador,
            [FromBody] string mensaje)
        {

            // Pedimos al servicio que envíe la orden.
            await _webSocketServicio.EnviarMensajeAsync(
                idControlador,
                mensaje
            );


            // Respondemos al frontend indicando que la orden fue enviada.
            return Ok(new
            {
                mensaje = "Orden enviada correctamente"
            });

        }



        // Endpoint para consultar si un controlador está conectado.
        //
        // Ejemplo:
        // GET:
        // api/WebSocket/estado/ESP32-001
        [HttpGet("estado/{idControlador}")]
        public IActionResult EstadoControlador(
            string idControlador)
        {

            // Consultamos al servicio.
            bool conectado = _webSocketServicio.EstaConectado(
                idControlador
            );


            // Retornamos el estado.
            return Ok(new
            {
                idControlador,
                conectado
            });

        }



        // Endpoint para desconectar un controlador.
        //
        // Ejemplo:
        // POST:
        // api/WebSocket/desconectar/ESP32-001
        [HttpPost("desconectar/{idControlador}")]
        public async Task<IActionResult> Desconectar(
            string idControlador)
        {

            // Solicitamos la desconexión al servicio.
            await _webSocketServicio.DesconectarControladorAsync(
                idControlador
            );


            // Confirmamos la operación.
            return Ok(new
            {
                mensaje = "Controlador desconectado"
            });

        }


    }

}