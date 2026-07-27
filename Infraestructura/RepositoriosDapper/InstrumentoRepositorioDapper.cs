using Dapper;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IRepositorios;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IunitOfWork;
using invernaderoInteligenteBackend.Dominio.Entidades.Instrumentos;
using invernaderoInteligenteBackend.Dominio.Entidades.TipoInstrumentos;
using invernaderoInteligenteBackend.Infraestructura.Mapper.MapperDominio;

namespace invernaderoInteligenteBackend.Infraestructura.RepositoriosDapper
{
    public class InstrumentoRepositorioDapper : IInstrumentoRepositorio
    {
        private readonly IUnitOfWork _unitOfWork;

        public InstrumentoRepositorioDapper(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<long> CrearInstrumentoAsync(Instrumento instrumento)
        {
            const string sql = """
        INSERT INTO public.instrumentos
        (
            controlador_id,
            tipo_instrumento_id,
            referencia,
            estado
        )
        VALUES
        (
            @ControladorId,
            @TipoInstrumentoId,
            @Referencia,
            @Estado
        )
        RETURNING id;
        """;

            return await _unitOfWork.Connection.ExecuteScalarAsync<long>(
                sql,
                new
                {
                    instrumento.ControladorId,
                    instrumento.TipoInstrumentoId,
                    instrumento.Referencia,
                    Estado = instrumento.Estado
                },
                _unitOfWork.Transaction);
        }
       
        
        
        public async Task<Instrumento> ActualizarInstrumentoAsync(Instrumento instrumento)
        {
            const string sql = """
                UPDATE public.instrumentos
                SET 
                    tipo_instrumento_id = @TipoInstrumentoId,
                    referencia = @Referencia,
                    estado = @Estado,
                    updated_at = @UpdatedAt
                WHERE id = @Id
                RETURNING
                    id,
                    controlador_id,
                    tipo_instrumento_id,
                    referencia,
                    estado,
                    activo,
                    created_at,
                    updated_at;
                
                """;

            return await _unitOfWork.Connection.QueryFirstAsync<Instrumento>(
                sql, instrumento, _unitOfWork.Transaction);


        }

        public async Task<bool> EliminarInstrumentoLogicoAsync(long id)
        {
            const string sql = """
                    UPDATE public.instrumentos
                    SET
                        activo = FALSE,
                        updated_at = @UpdatedAt
                    WHERE id = @Id;
                    """;

            return await _unitOfWork.Connection.ExecuteAsync(sql,new
                {Id = id,UpdatedAt = DateTime.UtcNow},_unitOfWork.Transaction) > 0;
        }

        public async Task<Instrumento?> ObtenerInstrumentoPorIdAsync(long id, Guid usuarioId)
        {
            const string sql = """
                    SELECT
                        i.id,
                        i.controlador_id,
                        i.tipo_instrumento_id,
                        i.referencia,
                        i.estado,
                        i.activo,
                        i.created_at,
                        i.updated_at,

                        ti.id,
                        ti.nombre,
                        ti.descripcion,
                        ti.unidad_medida,
                        ti.activo

                    FROM public.instrumentos i
                    INNER JOIN public.controlador_iot c
                        ON c.id = i.controlador_id
                    INNER JOIN public.invernaderos inv
                        ON inv.id = c.invernadero_id
                    INNER JOIN public.tipo_instrumento ti
                        ON ti.id = i.tipo_instrumento_id
                    WHERE
                        i.id = @Id
                        AND inv.usuario_id = @UsuarioId
                        AND i.activo = TRUE;
                    """;

            var resultado = await _unitOfWork.Connection.QueryAsync<
                Instrumento,
                TipoInstrumento,
                Instrumento>(
                sql,(instrumento, tipo) =>
                {instrumento.TipoInstrumento = tipo;return instrumento;},
                new{Id = id,UsuarioId = usuarioId},_unitOfWork.Transaction,splitOn: "id");

            return resultado.FirstOrDefault();
        }
        public async Task<IEnumerable<Instrumento>> ObtenerInstrumentoPorControladorAsync(long controladorId,Guid usuarioId)
                {
                    const string sql = """
                            SELECT
                                i.id,
                                i.controlador_id,
                                i.tipo_instrumento_id,
                                i.referencia,
                                i.estado,
                                i.activo,
                                i.created_at,
                                i.updated_at,

                                ti.id,
                                ti.nombre,
                                ti.descripcion,
                                ti.unidad_medida,
                                ti.activo

                            FROM public.instrumentos i
                            INNER JOIN public.controlador_iot c
                                ON c.id = i.controlador_id
                            INNER JOIN public.invernaderos inv
                                ON inv.id = c.invernadero_id
                            INNER JOIN public.tipo_instrumento ti
                                ON ti.id = i.tipo_instrumento_id
                            WHERE
                                i.controlador_id = @ControladorId
                                AND inv.usuario_id = @UsuarioId
                                AND i.activo = TRUE;
                            """;

                    return await _unitOfWork.Connection.QueryAsync<
                        Instrumento,
                        TipoInstrumento,
                        Instrumento>(
                        sql,
                        (instrumento, tipo) =>
                        {instrumento.TipoInstrumento = tipo;return instrumento;},
                        new
                        {
                            ControladorId = controladorId,
                            UsuarioId = usuarioId
                        },_unitOfWork.Transaction,splitOn: "id");
        }
        public async Task<bool> ExisteInstrumentoAsync(long id)
        {
            const string sql = """
                    SELECT EXISTS
                    (
                        SELECT 1
                        FROM public.instrumentos
                        WHERE id = @Id
                    );
                    """;

            return await _unitOfWork.Connection.ExecuteScalarAsync<bool>(
                sql,new { Id = id },_unitOfWork.Transaction);
        }

        public async Task<bool> ExisteReferenciaInstrumentoAsync(long controladorId, string referencia)
        {
            const string sql = """
                SELECT EXISTS
                (
                    SELECT 1
                    FROM public.instrumentos
                    WHERE controlador_id = @ControladorId
                      AND LOWER(referencia) = LOWER(@Referencia)
                );
                """;

            return await _unitOfWork.Connection.ExecuteScalarAsync<bool>(
                sql,new{ControladorId = controladorId,Referencia = referencia},
                _unitOfWork.Transaction
            );
        }

        public async Task<bool> ExisteReferenciaActualizarInstrumentoAsync(long id,long controladorId,string referencia)
        {
            const string sql = """
                    SELECT EXISTS
                    (
                        SELECT 1
                        FROM public.instrumentos
                        WHERE controlador_id = @ControladorId
                          AND LOWER(referencia) = LOWER(@Referencia)
                          AND id <> @Id
                    );
                    """;

            return await _unitOfWork.Connection.ExecuteScalarAsync<bool>(
                sql,new{Id = id,ControladorId = controladorId,Referencia = referencia},
                _unitOfWork.Transaction
            );
        }

        public async Task<bool> ReactivarInstrumentoLogicoAsync(long id)
        {
            const string sql = """
                    UPDATE public.instrumentos
                    SET
                        activo = true,
                        updated_at = @UpdatedAt
                    WHERE id = @Id;
                    """;

            return await _unitOfWork.Connection.ExecuteAsync(sql, new
            { Id = id, UpdatedAt = DateTime.UtcNow }, _unitOfWork.Transaction) > 0;
        }


    }
}