using invernaderoInteligenteBackend.Dominio.Entidades.Instrumentos;

namespace invernaderoInteligenteBackend.Aplicacion.Interfaces.IRepositorios
{
    public interface IInstrumentoRepositorio
    {
        Task<long> CrearInstrumentoAsync(Instrumento instrumento);

        Task<Instrumento> ActualizarInstrumentoAsync(Instrumento instrumento);

        Task<bool> EliminarInstrumentoLogicoAsync(long id);

        Task<Instrumento?> ObtenerInstrumentoPorIdAsync(long id, Guid usuarioId);

        Task<IEnumerable<Instrumento>> ObtenerInstrumentoPorControladorAsync(long controladorId, Guid usuarioId);

        Task<bool> ExisteInstrumentoAsync(long id);

        Task<bool> ExisteReferenciaInstrumentoAsync(long controladorId, string referencia);

        Task<bool> ExisteReferenciaActualizarInstrumentoAsync(long id, long controladorId,string referencia);
        Task<bool> ReactivarInstrumentoLogicoAsync(long id);
        Task<bool> ExisteInstrumentoPorControladorAsync(long instrumentoId, long controladorId);



    }
}