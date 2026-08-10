using invernaderoInteligenteBackend.Aplicacion.Interfaces.Context;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IRepositorios;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IServicios.ISignalR;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IWebSockets;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;

namespace invernaderoInteligenteBackend.Api.Controllers
{
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
            Console.WriteLine("[WebSocket] ENTRÓ AL ENDPOINT");
            Console.WriteLine("======================================");

            // =====================================================
            // VERIFICAR REQUEST WEBSOCKET
            // =====================================================

            if (!HttpContext.WebSockets.IsWebSocketRequest)
            {
                Console.WriteLine(
                    "[WebSocket] ERROR: No es una petición WebSocket"
                );

                HttpContext.Response.StatusCode = 400;
                return;
            }

            // =====================================================
            // ACEPTAR CONEXIÓN
            // =====================================================

            Console.WriteLine(
                "[WebSocket] Aceptando conexión..."
            );

            WebSocket conexion =
                await HttpContext.WebSockets.AcceptWebSocketAsync();

            Console.WriteLine(
                "[WebSocket] Conexión aceptada"
            );

            string? idControlador = null;

            try
            {
                // =================================================
                // OBTENER TOKEN DEL ESP32
                // =================================================

                Console.WriteLine(
                    "[WebSocket] Obteniendo token del controlador..."
                );

                var token =
                    _controladorContext.ObtenerTokenControlador();

                Console.WriteLine(
                    $"[WebSocket] Token: {token}"
                );

                // =================================================
                // BUSCAR CONTROLADOR
                // =================================================

                var controlador =
                    await _controladorIotRepositorio
                        .ObtenerControladorPorToken(token);

                if (controlador == null)
                {
                    Console.WriteLine(
                        "[WebSocket] ERROR: Controlador no encontrado"
                    );

                    await conexion.CloseAsync(
                        WebSocketCloseStatus.PolicyViolation,
                        "Controlador no autorizado",
                        CancellationToken.None
                    );

                    return;
                }

                // =================================================
                // OBTENER ID CONTROLADOR
                // =================================================

                idControlador =
                    controlador.Id.ToString();

                Console.WriteLine(
                    $"[WebSocket] CONTROLADOR ID: {idControlador}"
                );

                // =================================================
                // REGISTRAR WEBSOCKET
                // =================================================

                await _webSocketServicio
                    .RegistrarConexionAsync(conexion);

                Console.WriteLine(
                    $"[WebSocket] Controlador {idControlador} registrado"
                );

                // =================================================
                // BUFFER
                // =================================================

                var buffer = new byte[4096];

                // =================================================
                // RECIBIR MENSAJES DEL ESP32
                // =================================================

                while (
                    conexion.State == WebSocketState.Open
                )
                {
                    try
                    {
                        Console.WriteLine(
                            "======================================"
                        );

                        Console.WriteLine(
                            $"[WebSocket] Esperando mensaje del ESP32..."
                        );

                        var resultado =
                            await conexion.ReceiveAsync(
                                new ArraySegment<byte>(buffer),
                                CancellationToken.None
                            );

                        Console.WriteLine(
                            "[WebSocket] Receive terminado"
                        );

                        Console.WriteLine(
                            $"[WebSocket] Tipo: {resultado.MessageType}"
                        );

                        Console.WriteLine(
                            $"[WebSocket] Bytes: {resultado.Count}"
                        );

                        // =========================================
                        // CIERRE
                        // =========================================

                        if (
                            resultado.MessageType ==
                            WebSocketMessageType.Close
                        )
                        {
                            Console.WriteLine(
                                "[WebSocket] ESP32 solicitó cierre"
                            );

                            await conexion.CloseAsync(
                                WebSocketCloseStatus.NormalClosure,
                                "Cierre normal",
                                CancellationToken.None
                            );

                            break;
                        }

                        // =========================================
                        // IGNORAR MENSAJES QUE NO SEAN TEXTO
                        // =========================================

                        if (
                            resultado.MessageType !=
                            WebSocketMessageType.Text
                        )
                        {
                            Console.WriteLine(
                                "[WebSocket] Mensaje no textual ignorado"
                            );

                            continue;
                        }

                        // =========================================
                        // CONVERTIR MENSAJE
                        // =========================================

                        var mensaje =
                            Encoding.UTF8.GetString(
                                buffer,
                                0,
                                resultado.Count
                            );

                        Console.WriteLine(
                            "======================================"
                        );

                        Console.WriteLine(
                            "[WebSocket] MENSAJE REAL DEL ESP32:"
                        );

                        Console.WriteLine(
                            mensaje
                        );

                        Console.WriteLine(
                            $"[WebSocket] ID CONTROLADOR: {idControlador}"
                        );

                        Console.WriteLine(
                            "======================================"
                        );

                        // =========================================
                        // GUARDAR MENSAJE
                        // =========================================

                        _webSocketServicio
                            .GuardarMensajeRecibido(
                                idControlador,
                                mensaje
                            );

                        Console.WriteLine(
                            "[WebSocket] Mensaje guardado"
                        );

                        // =========================================
                        // ENVIAR MENSAJE REAL DEL ESP32
                        // A ANGULAR MEDIANTE SIGNALR
                        // =========================================

                        await _signalRServicio
                            .EnviarMensajeAsync(
                                idControlador,
                                mensaje
                            );

                        Console.WriteLine(
                            "[SignalR] Mensaje del ESP32 enviado a Angular"
                        );

                        Console.WriteLine(
                            $"[SignalR] Grupo: Controlador_{idControlador}"
                        );

                        Console.WriteLine(
                            "======================================"
                        );
                    }
                    catch (WebSocketException ex)
                    {
                        Console.WriteLine(
                            "[WebSocket] Conexión cerrada por el ESP32"
                        );

                        Console.WriteLine(
                            $"[WebSocket] {ex.Message}"
                        );

                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            "[WebSocket] ERROR procesando mensaje"
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
                Console.WriteLine(
                    "======================================"
                );

                Console.WriteLine(
                    "[WebSocket] ERROR GENERAL"
                );

                Console.WriteLine(
                    ex.Message
                );

                Console.WriteLine(
                    ex.StackTrace
                );

                Console.WriteLine(
                    "======================================"
                );
            }
            finally
            {
                // =============================================
                // LIMPIAR CONEXIÓN
                // =============================================

                Console.WriteLine(
                    "[WebSocket] FINALIZANDO SOCKET"
                );

                if (!string.IsNullOrEmpty(idControlador))
                {
                    await _webSocketServicio
                        .DesconectarControladorAsync(
                            idControlador
                        );
                }

                Console.WriteLine(
                    "[WebSocket] SOCKET LIMPIO"
                );
            }
        }
    }
}