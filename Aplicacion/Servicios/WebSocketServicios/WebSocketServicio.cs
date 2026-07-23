using invernaderoInteligenteBackend.Aplicacion.Interfaces.IWebSockets;

namespace invernaderoInteligenteBackend.Aplicacion.Servicios.WebSocketServicio
{

    public class WebSocketServicio : IWebSocketServicio
    {


        // Referencia al administrador de conexiones.
        private readonly IWebSocketManager _webSocketManager;



        // Constructor.
        public WebSocketServicio(IWebSocketManager webSocketManager)
        {// Guardamos la referencia del repositorio.
            _webSocketManager = webSocketManager;
        }



        // Envía mensajes hacia el controlador IoT.
        public async Task EnviarMensajeAsync(string idControlador,string mensaje)
        {
            await _webSocketManager.EnviarMensajeAsync(idControlador,mensaje);
        }



        // Verifica si el controlador está conectado.
        public bool EstaConectado(string idControlador)
        {
            return _webSocketManager.EstaConectado(idControlador);
        }



        // Recibe mensajes enviados por el controlador IoT.
        public async Task<string?> RecibirMensajeAsync(string idControlador)
        {
            return await _webSocketManager.RecibirMensajeAsync(idControlador);
        }



        // Desconecta el controlador IoT.
        public async Task DesconectarControladorAsync(string idControlador)
        {
            await _webSocketManager.DesconectarControladorAsync(idControlador);
        }


    }

}