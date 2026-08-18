using invernaderoInteligenteBackend.Dominio.Entidades.Invernadero;

namespace invernaderoInteligenteBackend.Aplicacion.Interfaces.IRepositorios
{
    public interface IInvernaderoRepositorio
    {
        Task<long> CrearAsync(Invernadero invernadero);

        Task<Invernadero> ActualizarAsync(Invernadero invernadero);

        Task<bool> EliminarLogicoAsync(long id);

        Task<Invernadero?> ObtenerInvernaderoPorIdAsync(long id, Guid usuarioId);

        // Un usuario solo puede tener un invernadero
        Task<IEnumerable<Invernadero?>> ObtenerPorUsuarioAsync(Guid usuarioId);

        Task<bool> ExisteInvernaderoAsync(long id);

        Task<bool> ExisteNombreInvernaderoAsync(Guid usuarioId, string nombre);

        Task<bool> ExisteNombreActualizarInvernaderoAsync(long id, Guid usuarioId, string nombre);

        Task<bool> ReactivarLogicoAsync(long id);
    }
}