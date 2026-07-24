using invernaderoInteligenteBackend.Dominio.Entidades.Mediciones;

namespace invernaderoInteligenteBackend.Aplicacion.Interfaces.IRepositorios
{
    public interface IMedicionRepositorio
    {
        Task<long> CrearMedicionAsync(Medicion medicion);

        Task<Medicion> ActualizarMedicionAsync(Medicion medicion);

        Task<bool> EliminarMedicionLogicoAsync(long id);

        Task<Medicion?> ObtenerMedicionPorIdAsync(long id, Guid usuarioId);

        Task<IEnumerable<Medicion>> ObtenerMedicionPorInstrumentoAsync(long instrumentoId, Guid usuarioId);

        Task<IEnumerable<Medicion>> ObtenerMedicionPorRangoFechasAsync(
            long instrumentoId,
            DateTime fechaInicio,
            DateTime fechaFin,
            Guid usuarioId);

        Task<bool> ExisteMedicionAsync(long id);
        Task<IEnumerable<Medicion>> ObtenerTodasLasMedicionesAsync(Guid usuarioId);
    }
}