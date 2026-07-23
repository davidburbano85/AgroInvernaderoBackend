using Dapper;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IRepositorios;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IunitOfWork;
using invernaderoInteligenteBackend.Dominio.Entidades.Mediciones;

namespace invernaderoInteligenteBackend.Infraestructura.RepositoriosDapper
{
    public class MedicionRepositorioDapper : IMedicionRepositorio
    {
        private readonly IUnitOfWork _unitOfWork;

        public MedicionRepositorioDapper(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<long> CrearMedicionAsync(Medicion medicion)
        {
            const string sql = """
                INSERT INTO public.mediciones
                (
                    instrumento_id,
                    coordenada_x,
                    coordenada_y,
                    cantidad,
                    fecha_hora,
                    estado
                )
                VALUES
                (
                    @InstrumentoId,
                    @CoordenadaX,
                    @CoordenadaY,
                    @Cantidad,
                    @FechaHora,
                    @Estado
                )
                RETURNING id, coordenada_x, coordenada_y, cantidad, fecha_hora,estado;
                """;

            return await _unitOfWork.Connection.ExecuteScalarAsync<long>(
                sql,
                medicion,
                _unitOfWork.Transaction
            );
        }

        public async Task<Medicion> ActualizarMedicionAsync(Medicion medicion)
        {
            const string sql = """
                UPDATE public.mediciones
                SET
                   fecha_hora = @FechaHora,
                    estado = @Estado,
                    activo = @Activo
                WHERE id = @Id
                RETURNING
                    id,
                    instrumento_id,
                    coordenada_x,
                    coordenada_y,
                    cantidad,
                    fecha_hora,
                    estado,
                    activo;
                """;

            return await _unitOfWork.Connection.QueryFirstAsync<Medicion>(
                sql,
                medicion,
                _unitOfWork.Transaction
            );
        }

        public async Task<bool> EliminarMedicionLogicoAsync(long id)
        {
            const string sql = """
                UPDATE public.mediciones
                SET activo = FALSE
                WHERE id = @Id;
                """;

            return await _unitOfWork.Connection.ExecuteAsync(
                sql,
                new { Id = id },
                _unitOfWork.Transaction
            ) > 0;
        }

        public async Task<Medicion?> ObtenerMedicionPorIdAsync(long id, Guid usuarioId)
        {
            const string sql = """
                SELECT
                    m.id,
                    m.instrumento_id,
                    m.coordenada_x,
                    m.coordenada_y,
                    m.cantidad,
                    m.fecha_hora,
                    m.estado,
                    m.activo
                FROM public.mediciones m
                INNER JOIN public.instrumentos i
                    ON i.id = m.instrumento_id
                INNER JOIN public.controlador_iot c
                    ON c.id = i.controlador_id
                INNER JOIN public.invernaderos inv
                    ON inv.id = c.invernadero_id
                WHERE m.id = @Id
                  AND inv.usuario_id = @UsuarioId;
                """;

            return await _unitOfWork.Connection.QueryFirstOrDefaultAsync<Medicion>(
                sql,
                new
                {
                    Id = id,
                    UsuarioId = usuarioId
                },
                _unitOfWork.Transaction
            );
        }

        public async Task<IEnumerable<Medicion>> ObtenerMedicionPorInstrumentoAsync(long instrumentoId, Guid usuarioId)
        {
            const string sql = """
                SELECT
                    m.id,
                    m.instrumento_id,
                    m.coordenada_x,
                    m.coordenada_y,
                    m.cantidad,
                    m.fecha_hora,
                    m.estado,
                    m.activo
                FROM public.mediciones m
                INNER JOIN public.instrumentos i
                    ON i.id = m.instrumento_id
                INNER JOIN public.controlador_iot c
                    ON c.id = i.controlador_id
                INNER JOIN public.invernaderos inv
                    ON inv.id = c.invernadero_id
                WHERE m.instrumento_id = @InstrumentoId
                  AND inv.usuario_id = @UsuarioId
                ORDER BY m.fecha_hora DESC;
                """;

            return await _unitOfWork.Connection.QueryAsync<Medicion>(
                sql,
                new
                {
                    InstrumentoId = instrumentoId,
                    UsuarioId = usuarioId
                },
                _unitOfWork.Transaction
            );
        }

        public async Task<IEnumerable<Medicion>> ObtenerMedicionPorRangoFechasAsync(
            long instrumentoId,
            DateTime fechaInicio,
            DateTime fechaFin,
            Guid usuarioId)
        {
            const string sql = """
                SELECT
                    m.id,
                    m.instrumento_id,
                    m.coordenada_x,
                    m.coordenada_y,
                    m.cantidad,
                    m.fecha_hora,
                    m.estado,
                    m.activo
                FROM public.mediciones m
                INNER JOIN public.instrumentos i
                    ON i.id = m.instrumento_id
                INNER JOIN public.controlador_iot c
                    ON c.id = i.controlador_id
                INNER JOIN public.invernaderos inv
                    ON inv.id = c.invernadero_id
                WHERE m.instrumento_id = @InstrumentoId
                  AND m.fecha_hora BETWEEN @FechaInicio AND @FechaFin
                  AND inv.usuario_id = @UsuarioId
                ORDER BY m.fecha_hora DESC;
                """;

            return await _unitOfWork.Connection.QueryAsync<Medicion>(
                sql,
                new
                {
                    InstrumentoId = instrumentoId,
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin,
                    UsuarioId = usuarioId
                },
                _unitOfWork.Transaction
            );
        }

        public async Task<bool> ExisteMedicionAsync(long id)
        {
            const string sql = """
                SELECT EXISTS
                (
                    SELECT 1
                    FROM public.mediciones
                    WHERE id = @Id
                );
                """;

            return await _unitOfWork.Connection.ExecuteScalarAsync<bool>(
                sql,
                new { Id = id },
                _unitOfWork.Transaction
            );
        }
    }
}