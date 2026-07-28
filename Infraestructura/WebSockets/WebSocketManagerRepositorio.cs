using invernaderoInteligenteBackend.Aplicacion.Interfaces.IWebSockets;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace invernaderoInteligenteBackend.Infraestructura.WebSockets
{
    public class WebSocketManagerRepositorio : IWebSocketManager
    {
        private readonly ConcurrentDictionary<string, WebSocket> _controladoresConectados;


        public WebSocketManagerRepositorio()
        {
            Console.WriteLine("[WebSocketManager] Constructor");

            _controladoresConectados =
                new ConcurrentDictionary<string, WebSocket>();
        }



        public async Task RegistrarControladorAsync(
            string idControlador,
            WebSocket conexion)
        {

            Console.WriteLine("==============================");
            Console.WriteLine("[RegistrarControladorAsync]");
            Console.WriteLine($"ID recibido: {idControlador}");
            Console.WriteLine($"Estado conexion recibida: {conexion.State}");
            Console.WriteLine($"Cantidad antes: {_controladoresConectados.Count}");
            Console.WriteLine("==============================");


            _controladoresConectados.AddOrUpdate(
                idControlador,
                conexion,
                (id, conexionAnterior) =>
                {
                    Console.WriteLine("[AddOrUpdate] Reemplazando conexión anterior");
                    Console.WriteLine($"Estado anterior: {conexionAnterior.State}");

                    return conexion;
                }
            );


            Console.WriteLine("==============================");
            Console.WriteLine("[Conexion guardada]");
            Console.WriteLine(
                $"Existe llave {idControlador}: {_controladoresConectados.ContainsKey(idControlador)}"
            );

            Console.WriteLine(
                $"Estado guardado: {_controladoresConectados[idControlador].State}"
            );

            Console.WriteLine(
                $"Cantidad después: {_controladoresConectados.Count}"
            );

            Console.WriteLine("==============================");


            await Task.CompletedTask;
        }




        public async Task EnviarMensajeAsync(
    string idControlador,
    string mensaje)
        {
            Console.WriteLine("======================================");
            Console.WriteLine("[EnviarMensajeAsync MANAGER]");
            Console.WriteLine($"Buscando ID: {idControlador}");
            Console.WriteLine($"Mensaje: {mensaje}");
            Console.WriteLine($"Cantidad conexiones: {_controladoresConectados.Count}");
            Console.WriteLine("======================================");

            if (!_controladoresConectados.TryGetValue(
                idControlador,
                out var conexion))
            {
                Console.WriteLine("[ERROR] No existe la llave en el diccionario.");

                foreach (var item in _controladoresConectados)
                {
                    Console.WriteLine(
                        $"ID: {item.Key}  Estado: {item.Value.State}");
                }

                throw new Exception(
                    $"El controlador {idControlador} no está conectado"
                );
            }

            Console.WriteLine("[Encontrado en diccionario]");
            Console.WriteLine($"Estado socket: {conexion.State}");
            Console.WriteLine($"CloseStatus: {conexion.CloseStatus}");
            Console.WriteLine($"CloseStatusDescription: {conexion.CloseStatusDescription}");

            Console.WriteLine($"SubProtocol: {conexion.SubProtocol}");

            Console.WriteLine($"CanSend: {conexion.State == WebSocketState.Open}");

            if (conexion.State != WebSocketState.Open)
            {
                Console.WriteLine("[ERROR] El socket NO está abierto.");

                throw new Exception(
                    $"El controlador {idControlador} no tiene una conexión activa"
                );
            }

            try
            {
                Console.WriteLine("[Enviando mensaje...]");

                var bytes = Encoding.UTF8.GetBytes(mensaje);

                await conexion.SendAsync(
                    new ArraySegment<byte>(bytes),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None
                );

                Console.WriteLine("[Mensaje enviado correctamente]");
                Console.WriteLine($"Estado después del envío: {conexion.State}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("======================================");
                Console.WriteLine("[ERROR EN SendAsync]");
                Console.WriteLine($"Tipo: {ex.GetType().FullName}");
                Console.WriteLine($"Mensaje: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine($"Estado actual: {conexion.State}");
                Console.WriteLine($"CloseStatus: {conexion.CloseStatus}");
                Console.WriteLine($"CloseStatusDescription: {conexion.CloseStatusDescription}");
                Console.WriteLine("======================================");

                throw;
            }
        }


        public bool EstaConectado(string idControlador)
        {

            Console.WriteLine("==============================");
            Console.WriteLine("[EstaConectado]");
            Console.WriteLine($"Buscando: {idControlador}");
            Console.WriteLine($"Cantidad: {_controladoresConectados.Count}");

            bool existe =
                _controladoresConectados.TryGetValue(
                    idControlador,
                    out var conexion);


            Console.WriteLine($"Existe: {existe}");


            if (existe)
            {
                Console.WriteLine($"Estado: {conexion.State}");
            }

            Console.WriteLine("==============================");


            return existe &&
                conexion.State == WebSocketState.Open;
        }





        public async Task DesconectarControladorAsync(
            string idControlador)
        {

            Console.WriteLine("==============================");
            Console.WriteLine("[DesconectarControladorAsync]");
            Console.WriteLine($"ID: {idControlador}");
            Console.WriteLine("==============================");


            if (_controladoresConectados.TryRemove(
                idControlador,
                out var conexion))
            {

                Console.WriteLine("[Conexion eliminada del diccionario]");
                Console.WriteLine($"Estado: {conexion.State}");


                if (conexion.State == WebSocketState.Open)
                {
                    await conexion.CloseAsync(
                        WebSocketCloseStatus.NormalClosure,
                        "Desconectado por servidor",
                        CancellationToken.None
                    );
                }


                conexion.Dispose();

                Console.WriteLine("[Dispose ejecutado]");
            }
            else
            {
                Console.WriteLine("[No existia conexion]");
            }

        }



        public async Task<string?> RecibirMensajeAsync(
            string idControlador)
        {

            Console.WriteLine("[RecibirMensajeAsync]");
            Console.WriteLine($"ID: {idControlador}");


            if (!_controladoresConectados.TryGetValue(
                idControlador,
                out var conexion))
            {
                throw new Exception(
                    $"El controlador {idControlador} no está conectado"
                );
            }


            Console.WriteLine($"Estado socket: {conexion.State}");


            var buffer = new byte[4096];


            var resultado =
                await conexion.ReceiveAsync(
                    new ArraySegment<byte>(buffer),
                    CancellationToken.None
                );


            Console.WriteLine($"Bytes recibidos: {resultado.Count}");
            Console.WriteLine($"Tipo: {resultado.MessageType}");


            string mensaje =
                Encoding.UTF8.GetString(
                    buffer,
                    0,
                    resultado.Count
                );


            Console.WriteLine($"Mensaje recibido: {mensaje}");


            return mensaje;

        }
    }
}