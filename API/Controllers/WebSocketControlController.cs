using invernaderoInteligenteBackend.Aplicacion.Interfaces.IWebSockets;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace invernaderoInteligenteBackend.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class WebSocketControlController : ControllerBase
    {


        private readonly IWebSocketManager _webSocketManager;



        public WebSocketControlController(
            IWebSocketManager webSocketManager)
        {

            _webSocketManager = webSocketManager;

        }



        [HttpGet("conectar/{idControlador}")]
        public async Task Conectar(
            string idControlador)
        {


            if (HttpContext.WebSockets.IsWebSocketRequest)
            {

                WebSocket conexion =
                    await HttpContext.WebSockets.AcceptWebSocketAsync();



                await _webSocketManager.RegistrarControladorAsync(
                    idControlador,
                    conexion
                );



                while (conexion.State == WebSocketState.Open)
                {

                    var buffer = new byte[4096];


                    WebSocketReceiveResult resultado =
                        await conexion.ReceiveAsync(
                            new ArraySegment<byte>(buffer),
                            CancellationToken.None
                        );


                    string mensaje =
                        System.Text.Encoding.UTF8.GetString(
                            buffer,
                            0,
                            resultado.Count
                        );


                    Console.WriteLine(
                        $"ESP32 {idControlador}: {mensaje}"
                    );

                }

            }
            else
            {

                HttpContext.Response.StatusCode = 400;

            }

        }


    }

}