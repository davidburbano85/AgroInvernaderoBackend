using System.Data;

namespace invernaderoInteligenteBackend.Infraestructura.AccesoADatos
{
    public interface IDBConnectionFactory
    {
        IDbConnection CrearConexion();
    }
}
