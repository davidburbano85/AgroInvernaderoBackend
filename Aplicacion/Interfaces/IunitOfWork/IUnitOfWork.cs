using System.Data;

namespace invernaderoInteligenteBackend.Aplicacion.Interfaces.IunitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IDbConnection Connection { get; }

        IDbTransaction? Transaction { get; }

        void BeginTransaction();

        void Commit();

        void Rollback();
    }
}