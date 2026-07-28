using invernaderoInteligenteBackend.Aplicacion.Interfaces.Context;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IRepositorios;
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
        private readonly IControladorIotRepositorio _controladorIotRepositorio;
        private readonly IControladorContext _controladorContext;

        public WebSocketControlController(
            IWebSocketServicio webSocketServicio, 
            IControladorIotRepositorio controladorIotRepositorio,
            IControladorContext controladorContext)

        {
            _webSocketServicio = webSocketServicio;
            _controladorIotRepositorio = controladorIotRepositorio;
            _controladorContext = controladorContext;
        }




        [HttpGet("conectar")]
        public async Task Conectar()
        {
            Console.WriteLine("======================================");
            Console.WriteLine("[1] Entró al endpoint WebSocket");
            Console.WriteLine("======================================");


            if (!HttpContext.WebSockets.IsWebSocketRequest)
            {
                Console.WriteLine("[ERROR] No es WebSocket");

                HttpContext.Response.StatusCode = 400;

                return;
            }


            Console.WriteLine("[2] Aceptando WebSocket");


            WebSocket conexion =
                await HttpContext.WebSockets.AcceptWebSocketAsync();


            Console.WriteLine("[3] WebSocket aceptado");


            string idControlador = null;


            try
            {

                Console.WriteLine("[4] Registrando controlador");


                var token =
                    _controladorContext.ObtenerTokenControlador();


                Console.WriteLine($"TOKEN: {token}");


                var controlador =
                    await _controladorIotRepositorio
                        .ObtenerControladorPorToken(token);



                if (controlador == null)
                {

                    Console.WriteLine("[ERROR] Controlador no encontrado");


                    await conexion.CloseAsync(
                        WebSocketCloseStatus.PolicyViolation,
                        "Controlador no autorizado",
                        CancellationToken.None
                    );


                    return;
                }



                idControlador =
                    controlador.Id.ToString();



                Console.WriteLine(
                    $"CONTROLADOR ID: {idControlador}"
                );



                await _webSocketServicio
                    .RegistrarConexionAsync(conexion);



                Console.WriteLine("[5] Conexion registrada");



                var buffer = new byte[4096];



                while (conexion.State == WebSocketState.Open)
                {
                    try
                    {
                        Console.WriteLine("Esperando ReceiveAsync...");

                        var resultado = await conexion.ReceiveAsync(
                            new ArraySegment<byte>(buffer),
                            CancellationToken.None);

                        Console.WriteLine("ReceiveAsync terminó");

                        Console.WriteLine($"Tipo: {resultado.MessageType}");
                        Console.WriteLine($"Bytes: {resultado.Count}");

                        if (resultado.MessageType == WebSocketMessageType.Close)
                        {
                            Console.WriteLine("El cliente pidió cerrar");

                            await conexion.CloseAsync(
                                WebSocketCloseStatus.NormalClosure,
                                "OK",
                                CancellationToken.None);

                            break;
                        }

                        Console.WriteLine(
                            Encoding.UTF8.GetString(buffer, 0, resultado.Count));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("EXCEPCION EN RECEIVE");
                        Console.WriteLine(ex.GetType().FullName);
                        Console.WriteLine(ex.Message);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("==============================");
                Console.WriteLine("ERROR WEBSOCKET");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine("==============================");

            }
            finally
            {

                Console.WriteLine("[10] FINALIZANDO SOCKET");


                if (!string.IsNullOrEmpty(idControlador))
                {

                    await _webSocketServicio
                        .DesconectarControladorAsync(idControlador);

                }


                Console.WriteLine("[11] SOCKET LIMPIO");

            }
        }

    }

}

   
