namespace invernaderoInteligenteBackend.Aplicacion.Interfaces.IServicios.ISignalR
{

    public interface ISignalRServicio
    {
        // Envía mensaje a un grupo de invernadero.
        Task EnviarMensajeAsync(string idInvernadero,string mensaje);

        // Envía mensaje a un usuario específico.
        Task EnviarMensajeUsuarioAsync(string idUsuario,string mensaje);

        // Envía estado del controlador al invernadero correspondiente.
        Task EnviarEstadoControladorAsync(
     string idControlador,
     string estado
 );
    }

}