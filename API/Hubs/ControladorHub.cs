using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace invernaderoInteligenteBackend.Api.Hubs
{

    [Authorize]
    public class ControladorHub : Hub
    {

        // Angular llama este método al abrir un invernadero.
        // Se agrega la conexión del usuario al grupo correspondiente.
        public async Task UnirseInvernadero(string idInvernadero)
        {

            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                $"Invernadero_{idInvernadero}"
            );

        }



        // Angular llama este método al cambiar de invernadero.
        // Se elimina la conexión del grupo anterior.
        public async Task SalirInvernadero(string idInvernadero)
        {

            await Groups.RemoveFromGroupAsync(
                Context.ConnectionId,
                $"Invernadero_{idInvernadero}"
            );

        }



        // Se ejecuta cuando Angular se conecta.
        public override async Task OnConnectedAsync()
        {

            await base.OnConnectedAsync();

        }



        // Se ejecuta cuando Angular se desconecta.
        public override async Task OnDisconnectedAsync(Exception? exception)
        {

            await base.OnDisconnectedAsync(exception);

        }

    }

}