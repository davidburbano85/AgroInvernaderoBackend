using invernaderoInteligenteBackend.Aplicacion.DTO.Medicion;

namespace invernaderoInteligenteBackend.Aplicacion.Interfaces.IServicios
{
    public interface IMedicionServicio
    {
        Task<long> CrearMedicionAsync(CrearMedicionDto dto);

        Task<MedicionRespuestaDto> ActualizarMedicionAsync(
            long id,
            ActualizarMedicionDto dto);

        Task<bool> EliminarMedicionLogicoAsync(long id);

        Task<MedicionRespuestaDto?> ObtenerMedicionPorIdAsync(long id);

        Task<IEnumerable<MedicionRespuestaDto>> ObtenerMedicionPorInstrumentoAsync(
            long instrumentoId);

        Task<IEnumerable<MedicionRespuestaDto>> ObtenerMedicionPorRangoFechasAsync(
            long instrumentoId,
            DateTime fechaInicio,
            DateTime fechaFin);

        Task<IEnumerable<MedicionRespuestaDto>> ObtenerTodasLasMedicionesAsync();
    }

}