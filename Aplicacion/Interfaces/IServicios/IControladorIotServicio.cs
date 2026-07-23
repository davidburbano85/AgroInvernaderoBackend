using invernaderoInteligenteBackend.Aplicacion.DTO.ControladorDto;

namespace invernaderoInteligenteBackend.Aplicacion.Interfaces.IServicios
{
    public interface IControladorIotServicio
    {
        Task<ControladorIotRespuestaDto> CrearControladorIotAsync(CrearControladorIotDto dto);

        Task<ControladorIotRespuestaDto> ActualizarControladorIotAsync(long id, ActualizarControladorIotDto dto);

        Task<bool> EliminarControladorIotAsync(long id);


        // El usuario autenticado solo tiene un invernadero y este solo un controlador
        Task<ControladorIotRespuestaDto?> ObtenerControladorIotPorInvernaderoAsync();

        Task<bool> ReactivarControladorIotAsync(long id);
    }
}