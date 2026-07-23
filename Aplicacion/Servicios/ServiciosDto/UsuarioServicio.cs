using invernaderoInteligenteBackend.Aplicacion.DTO.UsuarioDto;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IRepositorios;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IServicios;
using invernaderoInteligenteBackend.Dominio.Entidades.Usuario;

namespace invernaderoInteligenteBackend.Aplicacion.Servicios.ServiciosDto
{
    public class UsuarioServicio : IUsuarioServicio
    {
        private readonly IUsuarioRepositorioDapper _usuarioRepositorio;

        public UsuarioServicio(IUsuarioRepositorioDapper usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public async Task<UsuarioRespuestaDto?> ObtenerUsuarioPorIdAsync(Guid idUsuario)
        {
            Usuario? usuario =
                await _usuarioRepositorio.ObtenerUsuarioPorIdAsync(idUsuario);

            if (usuario is null)
                throw new KeyNotFoundException("El usuario no existe.");

            return new UsuarioRespuestaDto
            {
                Id = usuario.Id,
                Correo = usuario.Correo,
                Nombre = usuario.Nombre,
                Activo = usuario.Activo,
                CreatedAt = usuario.CreatedAt,
                UpdatedAt = usuario.UpdatedAt
            };
        }

        public async Task<IEnumerable<UsuarioRespuestaDto?>> ListarUsuariosAsync()
        {
            IEnumerable<Usuario?> usuarios =
                await _usuarioRepositorio.ListarUsuariosAsync();

            return usuarios
                .Where(usuario => usuario is not null)
                .Select(usuario => new UsuarioRespuestaDto
                {
                    Id = usuario!.Id,
                    Correo = usuario.Correo,
                    Nombre = usuario.Nombre,
                    Activo = usuario.Activo,
                    CreatedAt = usuario.CreatedAt,
                    UpdatedAt = usuario.UpdatedAt
                });
        }

        public async Task<bool> ActualizarUsuarioAsync(
            Guid idUsuario,
            ActualizarUsuarioDto dto)
        {
            Usuario? usuarioActual =
                await _usuarioRepositorio.ObtenerUsuarioPorIdAsync(idUsuario);

            if (usuarioActual is null)
                throw new KeyNotFoundException("El usuario no existe.");

            var usuario = new Usuario
            {
                Id = idUsuario,
                Correo = usuarioActual.Correo,
                Nombre = dto.Nombre,
                Activo = dto.Activo,
                CreatedAt = usuarioActual.CreatedAt,
                UpdatedAt = DateTime.UtcNow
            };

            return await _usuarioRepositorio.ActualizarUsuarioAsync(usuario);
        }

        public async Task<bool> EliminarUsuarioAsync(Guid idUsuario)
        {
            Usuario? usuario =
                await _usuarioRepositorio.ObtenerUsuarioPorIdAsync(idUsuario);

            if (usuario is null)
                throw new KeyNotFoundException("El usuario no existe.");

            return await _usuarioRepositorio.EliminarUsuarioAsync(idUsuario);
        }


        public async Task<bool> ReactivarUsuarioAsync(Guid idUsuario)
        {
            Usuario? usuario =
                await _usuarioRepositorio.ObtenerUsuarioPorIdAsync(idUsuario);

            if (usuario is null)
                throw new KeyNotFoundException("El usuario no existe.");

            return await _usuarioRepositorio.ReactivarUsuarioAsync(idUsuario);
        }
    }
}