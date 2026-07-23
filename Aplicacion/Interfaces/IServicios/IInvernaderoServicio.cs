using invernaderoInteligenteBackend.Aplicacion.DTO.InvernaderoDto;

namespace invernaderoInteligenteBackend.Aplicacion.Interfaces.IServicios
{
    public interface IInvernaderoServicio
    {
        Task<InvernaderoRespuestaDto> CrearAsync(CrearInvernaderoDto dto);

        Task<InvernaderoRespuestaDto> ActualizarAsync(long id, ActualizarInvernaderoDto dto);

        Task<bool> EliminarAsync(long id);

        Task<InvernaderoRespuestaDto?> ObtenerPorIdAsync(long id);

        Task<InvernaderoRespuestaDto> ObtenerPorUsuarioAsync();
        Task<bool> ReactivarAsync(long id);
    }
}