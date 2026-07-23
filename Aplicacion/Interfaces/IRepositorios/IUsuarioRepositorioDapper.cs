using invernaderoInteligenteBackend.Dominio.Entidades.Usuario;
using System.Data;

namespace invernaderoInteligenteBackend.Aplicacion.Interfaces.IRepositorios
{
    public interface IUsuarioRepositorioDapper
    {
        Task<int> CrearUsuarioAsync( Usuario usuario);
        Task<Usuario?> ObtenerUsuarioPorIdAsync(Guid idUsuario);
        Task<IEnumerable<Usuario?>> ListarUsuariosAsync();
        Task<bool> ActualizarUsuarioAsync(Usuario usuario);
        Task<bool> EliminarUsuarioAsync(Guid idUsuario);
        Task<bool> ReactivarUsuarioAsync(Guid idUsuario);
        Task<Usuario?> ObtenerPorAuthUsuarioAsync(Guid authIdUsuario);
        Task<bool> ActualizarPorAuthUsuarioAsync(Guid authId, Usuario usuario);

    }
}
