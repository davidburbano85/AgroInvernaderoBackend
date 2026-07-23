using invernaderoInteligenteBackend.Aplicacion.DTO.TipoInstrumentoDto;

namespace invernaderoInteligenteBackend.Aplicacion.Interfaces.IServicios
{
    public interface ITipoInstrumentoServicio
    {
        Task<TipoInstrumentoRespuestaDto> CrearTipoInstrumentoAsync(CrearTipoInstrumentoDto dto);

        Task<TipoInstrumentoRespuestaDto> ActualizarTipoInstrumentoAsync(
            short id,
            ActualizarTipoInstrumentoDto dto);

        Task<TipoInstrumentoRespuestaDto> ObtenerTipoInstrumentoPorIdAsync(short id);

        Task<IEnumerable<TipoInstrumentoRespuestaDto>> ObtenerTodosTipoInstrumentosAsync();
    }
}