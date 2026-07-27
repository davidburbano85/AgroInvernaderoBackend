using invernaderoInteligenteBackend.Dominio.Entidades.ControladorIot;

namespace invernaderoInteligenteBackend.Aplicacion.Interfaces.IRepositorios
{
    public interface IControladorIotRepositorio
    {
        Task<long> CrearControladorIotAsync(ControladorIot controladorIot);
        Task<ControladorIot> ActualizarControladorIotAsync(ControladorIot controladorIot);
        Task<bool> EliminarLogicoControladorIotAsync(long id);
        Task<IEnumerable<ControladorIot>> ObtenerControladorIotPorInvernaderoAsync(long invernaderoId);
        Task<bool> ExisteControladorIotAsync(long id);
        Task<bool> ReactivarLogicoControladorIotAsync(long id);
        Task<bool> ExisteControladorPorInvernaderoAsync(long invernaderoId);
        Task<ControladorIot> ObtenerControladorPorToken(Guid token);


    }
}
