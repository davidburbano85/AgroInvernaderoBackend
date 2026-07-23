using invernaderoInteligenteBackend.Dominio.Entidades.TipoInstrumentos;

namespace invernaderoInteligenteBackend.Aplicacion.Interfaces.IRepositorios
{
    public interface ITipoInstrumentoRepositorio
    {
        Task<short> CrearTipoInstrumentoAsync(TipoInstrumento tipoInstrumento);

        Task<TipoInstrumento> ActualizarTipoInstrumentoAsync(TipoInstrumento tipoInstrumento);


        Task<TipoInstrumento?> ObtenerTipoInstrumentoPorIdAsync(short id);

        Task<IEnumerable<TipoInstrumento>> ObtenerTodosTipoInstrumentosAsync();

        Task<bool> ExisteTipoInstrumentoAsync(short id);

        Task<bool> ExisteNombreTipoInstrumentoAsync(string nombre);

        Task<bool> ExisteNombreActualizarTipoInstrumentoAsync(short id, string nombre);
    }
}