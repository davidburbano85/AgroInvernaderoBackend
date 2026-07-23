using Dapper;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IRepositorios;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IunitOfWork;
using invernaderoInteligenteBackend.Dominio.Entidades.TipoInstrumentos;

namespace invernaderoInteligenteBackend.Infraestructura.RepositoriosDapper
{
    public class TipoInstrumentoRepositorioDapper : ITipoInstrumentoRepositorio
    {
        private readonly IUnitOfWork _unitOfWork;

        public TipoInstrumentoRepositorioDapper(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<short> CrearTipoInstrumentoAsync(TipoInstrumento tipoInstrumento)
        {
            const string sql = """
                INSERT INTO public.tipo_instrumento
                (nombre,descripcion,unidad_medida)
                VALUES
                (@Nombre,@Descripcion,@UnidadMedida)
                RETURNING id;
                """;

            return await _unitOfWork.Connection.ExecuteScalarAsync<short>(
                sql,
                tipoInstrumento,
                _unitOfWork.Transaction
            );
        }

        public async Task<TipoInstrumento> ActualizarTipoInstrumentoAsync(TipoInstrumento tipoInstrumento)
        {
            const string sql = """
                UPDATE public.tipo_instrumento
                SET
                    nombre = @Nombre,
                    descripcion = @Descripcion,
                    unidad_medida = @UnidadMedida
                WHERE id = @Id
                RETURNING
                    id,
                    nombre,
                    descripcion,
                    unidad_medida,
                    activo;
                """;

            return await _unitOfWork.Connection.QueryFirstAsync<TipoInstrumento>(
                sql,
                tipoInstrumento,
                _unitOfWork.Transaction
            );
        }

       
        public async Task<TipoInstrumento?> ObtenerTipoInstrumentoPorIdAsync(short id)
        {
            const string sql = """
                SELECT
                    id,
                    nombre,
                    descripcion,
                    unidad_medida,
                    activo
                FROM public.tipo_instrumento
                WHERE id = @Id;
                """;

            return await _unitOfWork.Connection.QueryFirstOrDefaultAsync<TipoInstrumento>(
                sql,
                new { Id = id },
                _unitOfWork.Transaction
            );
        }

        public async Task<IEnumerable<TipoInstrumento>> ObtenerTodosTipoInstrumentosAsync()
        {
            const string sql = """
                SELECT
                    id,
                    nombre,
                    descripcion,
                    unidad_medida,
                    activo
                FROM public.tipo_instrumento
                ORDER BY nombre;
                """;

            return await _unitOfWork.Connection.QueryAsync<TipoInstrumento>(
                sql,
                transaction: _unitOfWork.Transaction
            );
        }

        public async Task<bool> ExisteTipoInstrumentoAsync(short id)
        {
            const string sql = """
                SELECT EXISTS
                (
                    SELECT 1
                    FROM public.tipo_instrumento
                    WHERE id = @Id
                );
                """;

            return await _unitOfWork.Connection.ExecuteScalarAsync<bool>(
                sql,
                new { Id = id },
                _unitOfWork.Transaction
            );
        }

        public async Task<bool> ExisteNombreTipoInstrumentoAsync(string nombre)
        {
            const string sql = """
                SELECT EXISTS
                (
                    SELECT 1
                    FROM public.tipo_instrumento
                    WHERE LOWER(nombre) = LOWER(@Nombre)
                );
                """;

            return await _unitOfWork.Connection.ExecuteScalarAsync<bool>(
                sql,
                new { Nombre = nombre },
                _unitOfWork.Transaction
            );
        }

        public async Task<bool> ExisteNombreActualizarTipoInstrumentoAsync(short id, string nombre)
        {
            const string sql = """
                SELECT EXISTS
                (
                    SELECT 1
                    FROM public.tipo_instrumento
                    WHERE LOWER(nombre) = LOWER(@Nombre)
                      AND id <> @Id
                );
                """;

            return await _unitOfWork.Connection.ExecuteScalarAsync<bool>(
                sql,
                new
                {
                    Id = id,
                    Nombre = nombre
                },
                _unitOfWork.Transaction
            );
        }
    }
}