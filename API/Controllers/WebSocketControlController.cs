using invernaderoInteligenteBackend.Aplicacion.Interfaces.Context;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IRepositorios;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IServicios.ISignalR;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IWebSockets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
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
        private readonly ISignalRServicio _signalRServicio;


        public WebSocketControlController(
            IWebSocketServicio webSocketServicio, 
            IControladorIotRepositorio controladorIotRepositorio,
            IControladorContext controladorContext,
            ISignalRServicio signalRServicio)

        {
            _webSocketServicio = webSocketServicio;
            _controladorIotRepositorio = controladorIotRepositorio;
            _controladorContext = controladorContext;
            _signalRServicio = signalRServicio;
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
                        Console.WriteLine("======================================");
                        Console.WriteLine("[WebSocket] Esperando ReceiveAsync...");

                        var resultado = await conexion.ReceiveAsync(
                            new ArraySegment<byte>(buffer),
                            CancellationToken.None
                        );

                        Console.WriteLine("[WebSocket] Receive terminado");
                        Console.WriteLine(
                            $"[WebSocket] Tipo: {resultado.MessageType}"
                        );

                        Console.WriteLine(
                            $"[WebSocket] Bytes: {resultado.Count}"
                        );

                        // ==========================================
                        // CIERRE DEL WEBSOCKET
                        // ==========================================

                        if (resultado.MessageType == WebSocketMessageType.Close)
                        {
                            Console.WriteLine(
                                "[WebSocket] Cliente pidió cierre"
                            );

                            await conexion.CloseAsync(
                                WebSocketCloseStatus.NormalClosure,
                                "Cierre normal",
                                CancellationToken.None
                            );

                            break;
                        }

                        // ==========================================
                        // MENSAJE ESP32
                        // ==========================================

                        var mensaje = Encoding.UTF8.GetString(
                            buffer,
                            0,
                            resultado.Count
                        );

                        Console.WriteLine("==============================");
                        Console.WriteLine("MENSAJE ESP32:");
                        Console.WriteLine(mensaje);
                        Console.WriteLine(
                            $"ID CONTROLADOR: {idControlador}"
                        );
                        Console.WriteLine("==============================");

                        // ==========================================
                        // GUARDAR MENSAJE
                        // ==========================================

                        _webSocketServicio.GuardarMensajeRecibido(
                            idControlador,
                            mensaje
                        );

                        Console.WriteLine(
                            "[WebSocket] Mensaje guardado correctamente."
                        );

                        // ==========================================
                        // ENVIAR A ANGULAR MEDIANTE SIGNALR
                        // ==========================================

                        await _signalRServicio.EnviarMensajeAsync(
                            idControlador,
                            mensaje
                        );

                        Console.WriteLine(
                            $"[SignalR] Mensaje enviado al grupo Controlador_{idControlador}"
                        );

                        Console.WriteLine("==============================");
                    }
                    catch (WebSocketException ex)
                    {
                        Console.WriteLine(
                            "[WebSocket] WEBSOCKET CERRADO POR CLIENTE"
                        );

                        Console.WriteLine(
                            $"[WebSocket] {ex.Message}"
                        );

                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            "[WebSocket] ERROR PROCESANDO MENSAJE"
                        );

                        Console.WriteLine(
                            $"[WebSocket] {ex.Message}"
                        );

                        Console.WriteLine(
                            ex.StackTrace
                        );
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

   
