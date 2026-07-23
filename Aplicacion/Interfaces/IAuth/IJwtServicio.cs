namespace invernaderoInteligenteBackend.Aplicacion.Interfaces.IAuth
{
    public interface IJwtServicio
    {
        string GenerarToken(Guid userId, Guid invernaderoId);
    }
}
