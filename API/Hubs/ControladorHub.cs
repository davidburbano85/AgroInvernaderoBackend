using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace invernaderoInteligenteBackend.Api.Hubs
{
    [Authorize]
    public class ControladorHub : Hub
    {
        // ============================================================
        // UNIRSE AL CONTROLADOR
        // ============================================================

        public async Task UnirseControlador(string idControlador)
        {
            if (string.IsNullOrWhiteSpace(idControlador))
            {
                throw new HubException(
                    "El idControlador es obligatorio."
                );
            }

            var grupo = $"Controlador_{idControlador}";

            Console.WriteLine(
                $"[SignalR] Cliente {Context.ConnectionId} uniéndose a {grupo}"
            );

            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                grupo
            );

            Console.WriteLine(
                $"[SignalR] Cliente {Context.ConnectionId} unido a {grupo}"
            );
        }


        // ============================================================
        // SALIR DEL CONTROLADOR
        // ============================================================

        public async Task SalirControlador(string idControlador)
        {
            if (string.IsNullOrWhiteSpace(idControlador))
            {
                return;
            }

            var grupo = $"Controlador_{idControlador}";

            Console.WriteLine(
                $"[SignalR] Cliente {Context.ConnectionId} saliendo de {grupo}"
            );

            await Groups.RemoveFromGroupAsync(
                Context.ConnectionId,
                grupo
            );

            Console.WriteLine(
                $"[SignalR] Cliente {Context.ConnectionId} salió de {grupo}"
            );
        }


        // ============================================================
        // CONEXIÓN
        // ============================================================

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine(
                $"[SignalR] CONEXIÓN: {Context.ConnectionId}"
            );

            await base.OnConnectedAsync();
        }


        // ============================================================
        // DESCONEXIÓN
        // ============================================================

        public override async Task OnDisconnectedAsync(
            Exception? exception)
        {
            Console.WriteLine(
                $"[SignalR] DESCONEXIÓN: {Context.ConnectionId}"
            );

            if (exception != null)
            {
                Console.WriteLine(
                    $"[SignalR] Error: {exception.Message}"
                );
            }

            await base.OnDisconnectedAsync(exception);
        }


        // ============================================================
        // MENSAJE DESDE ANGULAR
        // ============================================================

        public async Task EnviarMensaje(string mensaje)
        {
            Console.WriteLine(
                $"[SignalR] Mensaje recibido desde Angular: {mensaje}"
            );

            // IMPORTANTE:
            // Ya NO hacemos Clients.Caller.SendAsync().
            //
            // El mensaje que debe aparecer en Angular como
            // "RecibirMensaje" será el mensaje REAL que llegue
            // desde el ESP32 mediante WebSocket.
        }
    }
}