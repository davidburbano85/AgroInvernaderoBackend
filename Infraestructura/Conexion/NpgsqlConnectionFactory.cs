using invernaderoInteligenteBackend.Infraestructura.AccesoADatos;
using Npgsql;
using System.Data;

namespace invernaderoInteligenteBackend.Infraestructura.Conexion
{
    public class NpgsqlConnectionFactory : IDBConnectionFactory
    {
        private readonly NpgsqlDataSource _dataSource;

        public NpgsqlConnectionFactory(NpgsqlDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public IDbConnection CrearConexion()
        {
            return _dataSource.CreateConnection();
        }
    }
}