using invernaderoInteligenteBackend.Aplicacion.Interfaces.IWebSockets;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace invernaderoInteligenteBackend.Infraestructura.WebSockets
{

    public class WebSocketManagerRepositorio : IWebSocketManager
    {


        // Guarda las conexiones activas de los controladores.
        private readonly ConcurrentDictionary<string, WebSocket> _controladoresConectados;



        public WebSocketManagerRepositorio()
        {

            // Inicializa el almacenamiento de conexiones.
            _controladoresConectados = new ConcurrentDictionary<string, WebSocket>();

        }



        public async Task RegistrarControladorAsync(
            string idControlador,
            WebSocket conexion)
        {

            // Guarda o reemplaza la conexión del controlador.
            _controladoresConectados.AddOrUpdate(
                idControlador,
                conexion,
                (id, conexionAnterior) => conexion
            );

            await Task.CompletedTask;

        }



        public async Task DesconectarControladorAsync(
            string idControlador)
        {

            // Busca y elimina el controlador conectado.
            if (_controladoresConectados.TryRemove(
                idControlador,
                out var conexion))
            {

                // Verifica que la conexión siga abierta.
                if (conexion.State == WebSocketState.Open)
                {

                    await conexion.CloseAsync(
                        WebSocketCloseStatus.NormalClosure,
                        "Desconectado por servidor",
                        CancellationToken.None
                    );

                }


                // Libera recursos.
                conexion.Dispose();

            }

        }



        public async Task EnviarMensajeAsync(
            string idControlador,
            string mensaje)
        {

            // Busca el controlador conectado.
            if (!_controladoresConectados.TryGetValue(
                idControlador,
                out var conexion))
            {

                throw new Exception(
                    $"El controlador {idControlador} no está conectado"
                );

            }



            // Verifica que la conexión esté activa.
            if (conexion.State != WebSocketState.Open)
            {

                throw new Exception(
                    $"El controlador {idControlador} no tiene una conexión activa"
                );

            }



            // Convierte el mensaje a bytes.
            var bytes = Encoding.UTF8.GetBytes(mensaje);



            // Envía el mensaje al ESP32.
            await conexion.SendAsync(
                new ArraySegment<byte>(bytes),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None
            );

        }



        public bool EstaConectado(
            string idControlador)
        {

            return _controladoresConectados.TryGetValue(
                    idControlador,
                    out var conexion)
                &&
                conexion.State == WebSocketState.Open;

        }



        public async Task<string?> RecibirMensajeAsync(
            string idControlador)
        {

            // Busca la conexión del controlador.
            if (!_controladoresConectados.TryGetValue(
                idControlador,
                out var conexion))
            {

                throw new Exception(
                    $"El controlador {idControlador} no está conectado"
                );

            }



            // Buffer donde se almacenará el mensaje recibido.
            var buffer = new byte[4096];



            // Recibe información enviada por el ESP32.
            WebSocketReceiveResult resultado =
                await conexion.ReceiveAsync(
                    new ArraySegment<byte>(buffer),
                    CancellationToken.None
                );



            // Convierte los bytes recibidos a texto.
            string mensaje = Encoding.UTF8.GetString(
                buffer,
                0,
                resultado.Count
            );



            return mensaje;

        }


    }

}