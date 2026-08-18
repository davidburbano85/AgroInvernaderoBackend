using Dapper;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IRepositorios;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IunitOfWork;
using invernaderoInteligenteBackend.Dominio.Entidades.Invernadero;

namespace invernaderoInteligenteBackend.Infraestructura.RepositoriosDapper
{
    public class InvernaderoRepositorioDapper : IInvernaderoRepositorio
    {

        private readonly IUnitOfWork _unitOfWork;

        public InvernaderoRepositorioDapper(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Invernadero> ActualizarAsync(Invernadero invernadero)
        {
            const string sql = """
                UPDATE public.invernaderos
                SET nombre = @Nombre,
                    ubicacion = @Ubicacion
                WHERE id = @Id
                RETURNING id, usuario_id, nombre, ubicacion, activo, created_at, updated_at;
                
                """;
            return await _unitOfWork.Connection.QueryFirstAsync<Invernadero>(sql, invernadero, _unitOfWork.Transaction);


        }

        public async Task<long> CrearAsync(Invernadero invernadero)
        {
         const string sql= """
                INSERT INTO public.invernaderos 
                (usuario_id, nombre, ubicacion) 
                VALUES (@UsuarioId, @Nombre, @Ubicacion)
                RETURNING id;              
                """;
            return  await _unitOfWork.Connection.ExecuteScalarAsync<long>(sql, invernadero, _unitOfWork.Transaction );

        }

        public async Task<bool> EliminarLogicoAsync(long id)
        {
           const string sql = """
                UPDATE public.invernaderos
                SET activo = false
                WHERE id = @InvernaderoId;
                """;

            return await _unitOfWork.Connection.ExecuteAsync(
                sql,  new { InvernaderoId = id }, _unitOfWork.Transaction)>0;
        }
       
        public async Task<bool> ExisteInvernaderoAsync(long id)
        {
            const string sql = """
                        SELECT EXISTS(
                        SELECT 1 
                        FROM public.invernaderos 
                        WHERE id = @InvernaderoId);           
                        """;
            return await _unitOfWork.Connection.ExecuteScalarAsync<bool>(sql, new { InvernaderoId = id },
            _unitOfWork.Transaction
            );

        }

        public Task<bool> ExisteNombreInvernaderoAsync(Guid usuarioId, string nombre)
        {
            const string sql = """
                        SELECT EXISTS(
                        SELECT 1 
                        FROM public.invernaderos 
                        WHERE usuario_id = @UsuarioId 
                        AND LOWER(nombre) = LOWER(@Nombre)
                        );           
                        """;
            return _unitOfWork.Connection.ExecuteScalarAsync<bool>(sql, new 
                { UsuarioId = usuarioId, Nombre = nombre },
                _unitOfWork.Transaction
            );


        }

        public async Task<Invernadero?> ObtenerInvernaderoPorIdAsync(long id, Guid usuarioId)
        {
          const string sql = """
                SELECT id, usuario_id, nombre, ubicacion, activo, created_at, updated_at
                FROM public.invernaderos
                WHERE id = @InvernaderoId
                AND usuario_id = @UsuarioId;
            """;

            return await _unitOfWork.Connection.QueryFirstOrDefaultAsync<Invernadero>(
                sql, new { InvernaderoId = id, UsuarioId=usuarioId },_unitOfWork.Transaction );
        }

        public async Task<IEnumerable<Invernadero?>> ObtenerPorUsuarioAsync(Guid usuarioId)
        {
            const string sql = """
                SELECT *
                FROM public.invernaderos
                WHERE usuario_id = @UsuarioId
                """;

            return await _unitOfWork.Connection.QueryAsync<Invernadero>(
                sql,
                new { UsuarioId = usuarioId },
                _unitOfWork.Transaction);
        }

        public Task<bool> ExisteNombreActualizarInvernaderoAsync(long id, Guid usuarioId, string nombre)
        {
            const string sql = """
                SELECT EXISTS(
                    SELECT 1
                    FROM public.invernaderos
                    WHERE usuario_id = @UsuarioId
                    AND LOWER(nombre) = LOWER(@Nombre)
                    AND id <> @Id
                    );
                """;

            return _unitOfWork.Connection.ExecuteScalarAsync<bool>(
                sql,
                new
                {
                    Id = id,
                    UsuarioId = usuarioId,
                    Nombre = nombre
                },
                _unitOfWork.Transaction);
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

        public async Task<bool> ReactivarLogicoAsync(long id)
        {
            const string sql = """
                UPDATE public.invernaderos
                SET activo = true
                WHERE id = @InvernaderoId;
                """;

            return await _unitOfWork.Connection.ExecuteAsync(
                sql, new { InvernaderoId = id }, _unitOfWork.Transaction) > 0;
        }

    }
}
