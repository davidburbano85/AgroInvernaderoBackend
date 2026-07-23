using invernaderoInteligenteBackend.Api.Hubs;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IServicios.ISignalR;
using Microsoft.AspNetCore.SignalR;

namespace invernaderoInteligenteBackend.Aplicacion.Servicios.SignalR
{

    public class SignalRServicio : ISignalRServicio
    {

        // Contexto del Hub.
        // Permite enviar mensajes a conexiones SignalR.
        private readonly IHubContext<ControladorHub> _hubContext;



        // Constructor.
        public SignalRServicio(
            IHubContext<ControladorHub> hubContext)
        {

            // Guardamos la referencia del Hub.
            _hubContext = hubContext;

        }



        // Envía un mensaje solamente al invernadero indicado.
        public async Task EnviarMensajeAsync(
            string idInvernadero,
            string mensaje)
        {

            await _hubContext.Clients
                .Group($"Invernadero_{idInvernadero}")
                .SendAsync(
                    "RecibirMensaje",
                    mensaje
                );

        }



        // Envía un mensaje solamente a un usuario específico.
        public async Task EnviarMensajeUsuarioAsync(
            string idUsuario,
            string mensaje)
        {

            await _hubContext.Clients
                .User(idUsuario)
                .SendAsync(
                    "RecibirMensaje",
                    mensaje
                );

        }



        // Envía información del controlador
        // únicamente al invernadero dueño.
        public async Task EnviarEstadoControladorAsync(
            string idInvernadero,
            string idControlador,
            string estado)
        {

            await _hubContext.Clients
                .Group($"Invernadero_{idInvernadero}")
                .SendAsync(
                    "EstadoControlador",
                    new
                    {
                        idControlador,
                        estado
                    }
                );

        }


    }

}