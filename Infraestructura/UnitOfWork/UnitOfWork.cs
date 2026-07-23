using invernaderoInteligenteBackend.Aplicacion.Interfaces.IunitOfWork;
using invernaderoInteligenteBackend.Infraestructura.AccesoADatos;
using System.Data;

namespace invernaderoInteligenteBackend.Infraestructura.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IDbConnection Connection { get; }

        public IDbTransaction? Transaction { get; private set; }

        public UnitOfWork(IDBConnectionFactory factory)
        {
            Connection = factory.CrearConexion();
            Connection.Open();
        }


        public void BeginTransaction()
        {
            Transaction = Connection.BeginTransaction();
        }


        public void Commit()
        {
            Transaction?.Commit();
            Transaction?.Dispose();
            Transaction = null;
        }


        public void Rollback()
        {
            Transaction?.Rollback();
            Transaction?.Dispose();
            Transaction = null;
        }


        public void Dispose()
        {
            Transaction?.Dispose();
            Connection?.Dispose();
        }
    }
}