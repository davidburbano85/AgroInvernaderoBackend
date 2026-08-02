using invernaderoInteligenteBackend.Aplicacion.Interfaces.IWebSockets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace invernaderoInteligenteBackend.Api.Controllers
{

    // Indicamos que esta clase es un controlador de API.
   // [Authorize]
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
            try
            {
                // 1. Enviar orden al ESP32
                await _webSocketServicio.EnviarMensajeAsync(
                    idControlador,
                    mensaje
                );


                // 2. Esperar respuesta del ESP32
                var respuesta = await _webSocketServicio.RecibirMensajeAsync(
                    idControlador
                );


                // 3. Retornar respuesta al frontend
                return Ok(new
                {
                    enviado = true,
                    mensaje = "Orden enviada correctamente",
                    respuesta = respuesta
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    enviado = false,
                    error = ex.Message
                });
            }
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