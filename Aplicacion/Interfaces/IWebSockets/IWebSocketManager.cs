using System.Net.WebSockets;

namespace invernaderoInteligenteBackend.Aplicacion.Interfaces.IWebSockets
{

    public interface IWebSocketManager
    {


        // Registra un controlador IoT cuando se conecta.
        Task RegistrarControladorAsync(
            string idControlador,
            WebSocket conexion
        );



        // Desconecta un controlador IoT.
        Task DesconectarControladorAsync(
            string idControlador
        );



        // Envía un mensaje al controlador conectado.
        Task EnviarMensajeAsync(
            string idControlador,
            string mensaje
        );



        // Verifica si un controlador está conectado.
        bool EstaConectado(
            string idControlador
        );



        // NUEVO:
        // Escucha mensajes enviados por el controlador IoT.
        //
        // Ejemplo:
        // ESP32 → Backend
        // "Motor terminado"
        Task<string?> RecibirMensajeAsync(
            string idControlador
        );


    }

}

//Controller.cs
//      ↓
//Servicio IoT
//      ↓
//WebSocket Manager

