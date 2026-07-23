using invernaderoInteligenteBackend.Aplicacion.DTO.InstrumentoDto;

namespace invernaderoInteligenteBackend.Aplicacion.Interfaces.IServicios
{
    public interface IInstrumentoServicio
    {
        Task<InstrumentoRespuestaDto> CrearInstrumentoAsync(CrearInstrumentoDto dto);

        Task<InstrumentoRespuestaDto> ActualizarInstrumentoAsync(long id,ActualizarInstrumentoDto dto);

        Task<bool> EliminarInstrumentoAsync(long id);

        Task<bool> ReactivarInstrumentoAsync(long id);

        Task<InstrumentoRespuestaDto> ObtenerInstrumentoPorIdAsync(long id);

        Task<IEnumerable<InstrumentoRespuestaDto>> ObtenerInstrumentoPorControladorAsync();
    }
}