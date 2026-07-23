namespace invernaderoInteligenteBackend.Aplicacion.DTO.AuthDto
{
    public class AuthRespuestaDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string TokenInterno { get; set; }
        public Guid UserId { get; set; }
    }
}
