using System;
using System.Collections.Generic;
using System.Linq;

namespace RepositoryFramework.GenericRepository
{
    public interface IGenericRepository<T, ID>
    {
        IQueryable<T> List<T>();
        T Get<T>(ID id);
        T Get<T>(Dictionary<string, object> compositeIds);
        void Create<T>(T entityToCreate);
        void Edit<T>(T entityToEdit);
        void Delete<T>(T entityToDelete);

        void CreateFast<T>(T entityToCreate);
        void CreateBulk<T>(IEnumerable<T> entityListToCreate);
        void EditFast<T>(T entityToEdit);
        void EditBulk<T>(IEnumerable<T> entityListToEdit);

        IDisposable BeginTransaction();
        void Commit(IDisposable transaction);
    }
}