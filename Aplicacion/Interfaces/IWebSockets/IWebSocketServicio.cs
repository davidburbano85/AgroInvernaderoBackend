using System.Net.WebSockets;

namespace invernaderoInteligenteBackend.Aplicacion.Interfaces.IWebSockets
{

    public interface IWebSocketServicio
    {


        // Envía una orden al controlador IoT.
        Task EnviarMensajeAsync(string idControlador,string mensaje);



        // Consulta si un controlador está conectado.
        bool EstaConectado(string idControlador);



        // Recibe información enviada por el controlador IoT.
        //
        // Ejemplo:
        // ESP32 → Backend
        // "Motor finalizado"
        Task<string?> RecibirMensajeAsync(string idControlador);



        // Desconecta un controlador IoT.
        Task DesconectarControladorAsync(string idControlador);
        Task RegistrarConexionAsync(WebSocket conexion);
        void GuardarMensajeRecibido(string idControlador,string mensaje);

        Task<string?> ObtenerMensajeRecibidoAsync(string idControlador);

    }

}