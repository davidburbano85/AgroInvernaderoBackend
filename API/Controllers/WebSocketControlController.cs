using invernaderoInteligenteBackend.Aplicacion.Interfaces.IWebSockets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;

namespace invernaderoInteligenteBackend.Api.Controllers
{
   // [Authorize]
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
            Console.WriteLine("======================================");
            Console.WriteLine("[1] Entró al endpoint WebSocket");
            Console.WriteLine("======================================");


            Console.WriteLine("[2] IsWebSocketRequest = "
                + HttpContext.WebSockets.IsWebSocketRequest);



            if (!HttpContext.WebSockets.IsWebSocketRequest)
            {
                Console.WriteLine("[3] No es una petición WebSocket.");

                HttpContext.Response.StatusCode = 400;

                return;
            }



            Console.WriteLine("[4] Aceptando conexión WebSocket...");



            WebSocket conexion =
                await HttpContext.WebSockets.AcceptWebSocketAsync();



            Console.WriteLine("[5] Conexión WebSocket aceptada.");



            try
            {

                Console.WriteLine("[6] Registrando conexión en servicio...");


                await _webSocketServicio.RegistrarConexionAsync(conexion);



                Console.WriteLine("[7] Registro completado correctamente.");



                var buffer = new byte[4096];



                Console.WriteLine("[7.1] Entrando al ciclo receptor.");



                while (conexion.State == WebSocketState.Open)
                {

                    Console.WriteLine("[8] Esperando mensaje del ESP32...");



                    WebSocketReceiveResult resultado =
                        await conexion.ReceiveAsync(
                            new ArraySegment<byte>(buffer),
                            CancellationToken.None
                        );



                    Console.WriteLine("[9] Mensaje recibido.");

                    Console.WriteLine(
                        "[10] Tipo mensaje: "
                        + resultado.MessageType
                    );


                    Console.WriteLine(
                        "[11] Cantidad bytes: "
                        + resultado.Count
                    );



                    if (resultado.MessageType == WebSocketMessageType.Close)
                    {

                        Console.WriteLine("[12] ESP32 solicitó cierre.");



                        await conexion.CloseAsync(
                            WebSocketCloseStatus.NormalClosure,
                            "Cierre solicitado por cliente",
                            CancellationToken.None
                        );


                        break;
                    }



                    string mensaje =
                        Encoding.UTF8.GetString(
                            buffer,
                            0,
                            resultado.Count
                        );



                    Console.WriteLine("==============================");
                    Console.WriteLine("[13] MENSAJE DEL ESP32:");
                    Console.WriteLine(mensaje);
                    Console.WriteLine("==============================");


                }


            }
            catch (Exception ex)
            {

                Console.WriteLine("======================================");
                Console.WriteLine("[ERROR WEBSOCKET CONTROLLER]");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine("======================================");

            }
            finally
            {

                Console.WriteLine("======================================");
                Console.WriteLine("[14] WebSocket finalizado.");
                Console.WriteLine("Estado final: " + conexion.State);
                Console.WriteLine("======================================");

            }

        }
    }

}

   
