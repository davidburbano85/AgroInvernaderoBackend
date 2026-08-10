using invernaderoInteligenteBackend.Api.Hubs;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IServicios.ISignalR;
using Microsoft.AspNetCore.SignalR;

namespace invernaderoInteligenteBackend.Aplicacion.Servicios.SignalR
{
    public class SignalRServicio : ISignalRServicio
    {
        private readonly IHubContext<ControladorHub> _hubContext;

        public SignalRServicio(
            IHubContext<ControladorHub> hubContext)
        {
            _hubContext = hubContext;
        }

        // ============================================================
        // ENVIAR MENSAJE AL CONTROLADOR
        // ============================================================

        public async Task EnviarMensajeAsync(
            string idControlador,
            string mensaje)
        {
            Console.WriteLine("======================================");
            Console.WriteLine("[SignalRServicio] Enviando mensaje");
            Console.WriteLine($"[SignalRServicio] Controlador: {idControlador}");
            Console.WriteLine($"[SignalRServicio] Mensaje: {mensaje}");
            Console.WriteLine(
                $"[SignalRServicio] Grupo: Controlador_{idControlador}"
            );
            Console.WriteLine("======================================");

            await _hubContext.Clients
                .Group($"Controlador_{idControlador}")
                .SendAsync(
                    "RecibirMensaje",
                    mensaje
                );

            Console.WriteLine(
                "[SignalRServicio] Mensaje enviado correctamente"
            );
        }

        // ============================================================
        // ENVIAR MENSAJE A USUARIO
        // ============================================================

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

        // ============================================================
        // ESTADO DEL CONTROLADOR
        // ============================================================

        public async Task EnviarEstadoControladorAsync(
            string idControlador,
            string estado)
        {
            await _hubContext.Clients
                .Group($"Controlador_{idControlador}")
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