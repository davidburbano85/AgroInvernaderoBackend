using invernaderoInteligenteBackend.Aplicacion.DTO.UsuarioDto;

namespace invernaderoInteligenteBackend.Aplicacion.Interfaces.IServicios
{
    public interface IUsuarioServicio
    {
        Task<UsuarioRespuestaDto?> ObtenerUsuarioPorIdAsync(Guid idUsuario);

        Task<IEnumerable<UsuarioRespuestaDto?>> ListarUsuariosAsync();

        Task<bool> ActualizarUsuarioAsync(
            Guid idUsuario,
            ActualizarUsuarioDto dto);

        Task<bool> EliminarUsuarioAsync(Guid idUsuario);

         Task<bool> ReactivarUsuarioAsync(Guid idUsuario);
    }
}