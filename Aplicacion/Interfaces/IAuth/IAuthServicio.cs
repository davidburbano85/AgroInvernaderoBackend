using invernaderoInteligenteBackend.Aplicacion.DTO.AuthDto;

namespace invernaderoInteligenteBackend.Aplicacion.Interfaces.IAuth
{
    public interface IAuthServicio
    {
        Task<AuthRespuestaDto>LoginAsync(string email, string password);
        Task<AuthRespuestaDto> SignUpAsync(string email, string password);
        Task<AuthRespuestaDto> RefreshTokenAsync(string refreshToken);
    }
}
