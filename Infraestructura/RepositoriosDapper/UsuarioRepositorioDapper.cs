using Dapper;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IRepositorios;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IunitOfWork;
using invernaderoInteligenteBackend.Dominio.Entidades.Usuario;

namespace invernaderoInteligenteBackend.Infraestructura.RepositoriosDapper
{
    public class UsuarioRepositorioDapper : IUsuarioRepositorioDapper
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsuarioRepositorioDapper(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> CrearUsuarioAsync(Usuario usuario)
        {
            const string checkSql = """
                SELECT EXISTS
                (
                    SELECT 1
                    FROM public.usuarios
                    WHERE id = @Id
                );
                """;

            bool existe = await _unitOfWork.Connection.ExecuteScalarAsync<bool>(
                checkSql,
                new { usuario.Id },
                _unitOfWork.Transaction);

            if (existe)
                return 0;

            const string insertSql = """
                INSERT INTO public.usuarios
                (
                    id,
                    nombre,
                    activo,
                    created_at
                )
                VALUES
                (
                    @Id,
                    @Nombre,
                    @Activo,
                    @CreatedAt
                );
                """;

            return await _unitOfWork.Connection.ExecuteAsync(
                insertSql,
                usuario,
                _unitOfWork.Transaction);
        }

        public async Task<Usuario?> ObtenerUsuarioPorIdAsync(Guid idUsuario)
        {
            const string sql = """
                SELECT
                    id,
                    correo,
                    nombre,
                    activo,
                    created_at,
                    updated_at
                FROM public.usuarios
                WHERE id = @Id;
                """;

            return await _unitOfWork.Connection.QueryFirstOrDefaultAsync<Usuario>(
                sql,
                new { Id = idUsuario },
                _unitOfWork.Transaction);
        }

        public async Task<IEnumerable<Usuario?>> ListarUsuariosAsync()
        {
            const string sql = """
                SELECT
                    id,
                    correo,
                    nombre,
                    activo,
                    created_at
                    updated_at
                FROM public.usuarios
                ORDER BY nombre;
                """;

            return await _unitOfWork.Connection.QueryAsync<Usuario>(
                sql,
                transaction: _unitOfWork.Transaction);
        }

        public async Task<bool> ActualizarUsuarioAsync(Usuario usuario)
        {
            const string sql = """
                UPDATE public.usuarios
                SET
                    nombre = @Nombre,
                    activo = @Activo
                WHERE id = @Id;
                """;

            return await _unitOfWork.Connection.ExecuteAsync(
                sql,
                usuario,
                _unitOfWork.Transaction) > 0;
        }

        public async Task<bool> EliminarUsuarioAsync(Guid idUsuario)
        {
            const string sql = """
                UPDATE public.usuarios
                SET
                    activo = FALSE,
                    updated_at = NOW()
                WHERE id = @Id;
                """;

            return await _unitOfWork.Connection.ExecuteAsync(
                sql,
                new { Id = idUsuario },
                _unitOfWork.Transaction) > 0;
        }

        public async Task<bool> ReactivarUsuarioAsync(Guid idUsuario)
        {
            const string sql = """
                UPDATE public.usuarios
                SET
                    activo = TRUE,
                    updated_at = NOW()
                WHERE id = @Id;
                """;

            return await _unitOfWork.Connection.ExecuteAsync(
                sql,
                new { Id = idUsuario },
                _unitOfWork.Transaction) > 0;
        }

        public async Task<Usuario?> ObtenerPorAuthUsuarioAsync(Guid authIdUsuario)
        {
            const string sql = """
                SELECT
                    id,
                    correo,
                    nombre,
                    activo,
                    created_at,
                    updated_at
                FROM public.usuarios
                WHERE id = @AuthId;
                """;

            return await _unitOfWork.Connection.QueryFirstOrDefaultAsync<Usuario>(
                sql,
                new { AuthId = authIdUsuario },
                _unitOfWork.Transaction);
        }

        public async Task<bool> ActualizarPorAuthUsuarioAsync(Guid authId, Usuario usuario)
        {
            const string sql = """
                UPDATE public.usuarios
                SET
                    nombre = @Nombre,
                    activo = @Activo
                WHERE id = @AuthId;
                """;

            return await _unitOfWork.Connection.ExecuteAsync(
                sql,
                new
                {
                    AuthId = authId,
                    usuario.Nombre,
                    usuario.Activo
                },
                _unitOfWork.Transaction) > 0;
        }
    }
}