using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace RepositoryFramework.GenericRepository
{
    public class FakeGenericRepository<T, ID> : GenericRepositoryBase<T, ID>
    {

        private MemoryDatabase _db = new MemoryDatabase();


        #region IGenericRepository Members

        public override IQueryable<T> List<T>()
        {
            return _db.Select<T>();
        }

        public override T Get<T>(ID id)
        {
            return List<T>().FirstOrDefault(CreateGetExpression<T>(id));
        }

        public override T Get<T>(Dictionary<string, object> compositeIds)
        {
            return List<T>().FirstOrDefault(CreateGetExpressionFromCompositeIds<T>(compositeIds));
        }

        public override void Create<T>(T entityToCreate)
        {
            _db.Insert<T>(entityToCreate);
        }

        public override void Edit<T>(T originalEntity)
        {
            _db.Update<T>(originalEntity, originalEntity);
        }


        public override void Delete<T>(T entityToDelete)
        {
            _db.Delete<T>(entityToDelete);
        }

        public override void CreateFast<T>(T entityToCreate)
        {
            _db.Insert<T>(entityToCreate);
        }

        public override void CreateBulk<T>(IEnumerable<T> entityListToCreate)
        {
            foreach (var el in entityListToCreate)
                _db.Insert<T>(el);
        }

        public override void EditFast<T>(T entityToEdit)
        {
            _db.Update<T>(entityToEdit, entityToEdit);
        }

        public override void EditBulk<T>(IEnumerable<T> entityListToEdit)
        {
            foreach (var el in entityListToEdit)
                _db.Update<T>(el, el);
        }

        public override IDisposable BeginTransaction()
        {
            return new FakeDisposable();
        }

        public override void Commit(IDisposable transaction)
        {
        }

        public override void Rollback(IDisposable transaction)
        {
        }

        #endregion
    }

    public class FakeDisposable : IDisposable
    {
        public void Dispose()
        {
        }
    }
}
