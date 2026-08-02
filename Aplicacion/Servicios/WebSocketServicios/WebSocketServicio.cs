using invernaderoInteligenteBackend.Aplicacion.Interfaces.Context;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IRepositorios;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IWebSockets;
using System.Net.WebSockets;

namespace invernaderoInteligenteBackend.Aplicacion.Servicios.WebSocketServicio
{
    public class WebSocketServicio : IWebSocketServicio
    {
        // Referencia al administrador de conexiones.
        private readonly IWebSocketManager _webSocketManager;
        private readonly IControladorContext _controladorContext;
        private readonly IControladorIotRepositorio _controladorRepositorio;

        // Constructor.
        public WebSocketServicio(
            IWebSocketManager webSocketManager,
            IControladorContext controladorContext,
            IControladorIotRepositorio controladorIotRepositorio)
        {
            Console.WriteLine("[WebSocketServicio] Constructor");

            _webSocketManager = webSocketManager;
            _controladorContext = controladorContext;
            _controladorRepositorio = controladorIotRepositorio;
        }

        // Envía mensajes hacia el controlador IoT.
        public async Task EnviarMensajeAsync(string idControlador, string mensaje)
        {
            Console.WriteLine($"[EnviarMensajeAsync] Id: {idControlador}");
            Console.WriteLine($"[EnviarMensajeAsync] Mensaje: {mensaje}");

            await _webSocketManager.EnviarMensajeAsync(idControlador, mensaje);

            Console.WriteLine("[EnviarMensajeAsync] Mensaje enviado.");
        }

        // Verifica si el controlador está conectado.
        public bool EstaConectado(string idControlador)
        {
            Console.WriteLine($"[EstaConectado] Id: {idControlador}");

            bool conectado = _webSocketManager.EstaConectado(idControlador);

            Console.WriteLine($"[EstaConectado] Resultado: {conectado}");

            return conectado;
        }

        // Recibe mensajes enviados por el controlador IoT.
        public async Task<string?> RecibirMensajeAsync(string idControlador)
        {
            Console.WriteLine($"[RecibirMensajeAsync] Id: {idControlador}");

            var mensaje = await _webSocketManager.RecibirMensajeAsync(idControlador);

            Console.WriteLine($"[RecibirMensajeAsync] Mensaje recibido: {mensaje}");

            return mensaje;
        }

        // Desconecta el controlador IoT.
        public async Task DesconectarControladorAsync(string idControlador)
        {
            Console.WriteLine($"[DesconectarControladorAsync] Id: {idControlador}");

            await _webSocketManager.DesconectarControladorAsync(idControlador);

            Console.WriteLine("[DesconectarControladorAsync] Controlador desconectado.");
        }

        public async Task RegistrarConexionAsync(WebSocket conexion)
        {
            Console.WriteLine("======================================");
            Console.WriteLine("[RegistrarConexionAsync] INICIO");
            Console.WriteLine("======================================");

            try
            {
                Console.WriteLine("[1] Obteniendo token...");

                var token = _controladorContext.ObtenerTokenControlador();

                Console.WriteLine($"[2] Token obtenido: {token}");

                Console.WriteLine("[3] Buscando controlador...");

                var controlador =
                    await _controladorRepositorio.ObtenerControladorPorToken(token);

                Console.WriteLine("[4] Consulta finalizada.");

                if (controlador == null)
                {
                    Console.WriteLine("[5] Controlador NO encontrado.");

                    await conexion.CloseAsync(
                        WebSocketCloseStatus.PolicyViolation,
                        "Controlador no autorizado",
                        CancellationToken.None
                    );

                    Console.WriteLine("[6] WebSocket cerrado.");

                    return;
                }

                Console.WriteLine($"[7] Controlador encontrado.");
                Console.WriteLine($"    Id: {controlador.Id}");
                Console.WriteLine($"    Token: {controlador.Token}");

                Console.WriteLine("[8] Registrando conexión...");

                await _webSocketManager.RegistrarControladorAsync(
                    controlador.Id.ToString(),
                    conexion
                );

                Console.WriteLine("[9] Conexión registrada correctamente.");
                Console.WriteLine("======================================");
                Console.WriteLine("[RegistrarConexionAsync] FIN");
                Console.WriteLine("======================================");
            }
            catch (Exception ex)
            {
                Console.WriteLine("======================================");
                Console.WriteLine("[ERROR RegistrarConexionAsync]");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine("======================================");

                throw;
            }
        }
        public void GuardarMensajeRecibido(
        string idControlador,
        string mensaje)
        {
            _webSocketManager.GuardarMensajeRecibido(
                idControlador,
                mensaje
            );
        }


        public async Task<string?> ObtenerMensajeRecibidoAsync(
            string idControlador)
        {
            return await _webSocketManager.ObtenerMensajeRecibidoAsync(
                idControlador
            );
        }




    }
}