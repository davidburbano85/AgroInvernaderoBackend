

using Dapper;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IRepositorios;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IunitOfWork;
using invernaderoInteligenteBackend.Dominio.Entidades.ControladorIot;

namespace invernaderoInteligenteBackend.Infraestructura.RepositoriosDapper
{
    public class ControladorIotRepositorioDapper : IControladorIotRepositorio
    {

        private readonly IUnitOfWork _unitOfWork;
        public ControladorIotRepositorioDapper(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<ControladorIot> ActualizarControladorIotAsync(ControladorIot controladorIot)
        {
            const string sql = """
                    UPDATE public.controlador_iot
                    SET nombre = @Nombre,
                        ubicacion = @Ubicacion,
                        referencia = @Referencia,
                        descripcion = @Descripcion,
                        ultima_conexion = NOW(),
                        updated_at = NOW()
                    WHERE id = @Id
                    RETURNING *;
                    """;

            var resultado = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<ControladorIot>(
                    sql,controladorIot,_unitOfWork.Transaction);

            if (resultado is null)
                throw new KeyNotFoundException("No existe el controlador IoT.");

            return resultado;
        }
        public async Task<long> CrearControladorIotAsync(ControladorIot controladorIot)
        {

            const string sql = """
                INSERT INTO public.controlador_iot (
                    invernadero_id,
                    nombre,
                    token,
                    ubicacion,
                    referencia,
                    descripcion,
                    ultima_conexion,
                    activo
                )
                VALUES (
                    @InvernaderoId,
                    @Nombre,
                    @Token,
                    @Ubicacion,
                    @Referencia,
                    @Descripcion,
                    @UltimaConexion,
                    @Activo
                )
                RETURNING id
                """;

            return await _unitOfWork.Connection.ExecuteScalarAsync<long>(
                sql,
                controladorIot,
                _unitOfWork.Transaction
            );
        }
        public async Task<bool> EliminarLogicoControladorIotAsync(long id)
        {
            const string sql = """
                UPDATE public.controlador_iot
                SET activo = false
                WHERE id = @Id
                """;
            return await _unitOfWork.Connection.ExecuteAsync(sql, new { Id = id }, _unitOfWork.Transaction) > 0;

        }

        public async Task<bool> ExisteControladorIotAsync(long id)
        {
            const string sql = """
                SELECT EXISTS(
                    SELECT 1
                    FROM public.controlador_iot
                    WHERE id = @Id
                );                
                """;
            return await _unitOfWork.Connection.ExecuteScalarAsync<bool>(sql, new { Id = id }, _unitOfWork.Transaction);
        }

        public async Task<IEnumerable<ControladorIot>> ObtenerControladorIotPorInvernaderoAsync(long invernaderoId)
        {
            const string sql = """
                SELECT *
                FROM public.controlador_iot
                WHERE invernadero_id = @InvernaderoId
                """;
            return await _unitOfWork.Connection.QueryAsync<ControladorIot>(sql, new { InvernaderoId = invernaderoId }, _unitOfWork.Transaction);
        }

        public  async Task<bool> ReactivarLogicoControladorIotAsync(long id)
        {
            const string sql = """
                UPDATE public.controlador_iot
                SET activo = true
                WHERE id = @Id
                """;
            return await _unitOfWork.Connection.ExecuteAsync(
                sql, new { Id = id }, _unitOfWork.Transaction) > 0;
        }
        public async Task<bool> ExisteControladorPorInvernaderoAsync(long invernaderoId)
        {
            const string sql = """
            SELECT EXISTS(
                SELECT 1
                FROM public.controlador_iot
                WHERE invernadero_id = @InvernaderoId
            );
            """;

            return await _unitOfWork.Connection.ExecuteScalarAsync<bool>(
                sql,
                new { InvernaderoId = invernaderoId },
                _unitOfWork.Transaction);
        }

        public async Task<ControladorIot> ObtenerControladorPorToken(Guid token)
        {
            const string sql = """
                SELECT
                    id,
                    invernadero_id AS InvernaderoId,
                    nombre,
                    token,
                    ubicacion,
                    referencia,
                    descripcion,
                    ultima_conexion AS UltimaConexion,
                    activo,
                    created_at AS CreatedAt,
                    updated_at AS UpdatedAt
                FROM public.controlador_iot
                WHERE token = @Token
                """;

            var controlador = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<ControladorIot>(
                sql,
                new { Token = token },
                _unitOfWork.Transaction
            );

            if (controlador == null)
                throw new KeyNotFoundException("No existe el controlador IoT con el token proporcionado.");

            return controlador;
        }



    }
}
