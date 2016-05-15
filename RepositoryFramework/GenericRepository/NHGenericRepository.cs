#define NHIBERNATOR_DEPENDENT
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using NHibernatorFramework;

#if NHIBERNATOR_DEPENDENT
using RepositoryFramework.NHibernate.Config;
#endif

namespace RepositoryFramework.GenericRepository
{
    public abstract class NHGenericRepository<T, ID> : GenericRepositoryBase<T, ID>
    {
        protected ISession _session = null;

        //protected NHibernatorTransaction _tran = null;

#if NHIBERNATOR_DEPENDENT
        protected string _sessionName = null;
        public NHGenericRepository()
            : this(Config.ConnStringName)
        {
        }

        public NHGenericRepository(string sessionName)
        {
            _sessionName = sessionName;
            _session = NHibernator.GetSession(_sessionName);
        }
#endif

        public NHGenericRepository(ISession session)
        {
            _session = session;
        }

        public override IQueryable<T> List<T>()
        {
            return _session.Query<T>();
        }

        public override T Get<T>(ID id)
        {
            return _session.Get<T>(id);
        }

        public override T Get<T>(Dictionary<string, object> compositeIds)
        {
            return List<T>().FirstOrDefault(CreateGetExpressionFromCompositeIds<T>(compositeIds));
        }

        public override void Create<T>(T entityToCreate)
        {
            using (var transaction = new NHibernatorTransaction(_sessionName))
            {
                _session.Save(entityToCreate);
                transaction.Commit();
            }
        }

        public override void CreateBulk<T>(IEnumerable<T> entityListToCreate)
        {
            using (var transaction = new NHibernatorTransaction(_sessionName))
            {
                foreach (var item in entityListToCreate)
                {
                    _session.Save(item);
                }
                transaction.Commit();
            }
        }

        public override void CreateFast<T>(T entityToCreate)
        {
            _session.Save(entityToCreate);
        }

        public override void Edit<T>(T entityToEdit)
        {
            using (var transaction = new NHibernatorTransaction(_sessionName))
            {
                _session.SaveOrUpdate(entityToEdit);
                transaction.Commit();
            }
        }

        public override void EditFast<T>(T entityToEdit)
        {
            _session.SaveOrUpdate(entityToEdit);
        }


        public override void EditBulk<T>(IEnumerable<T> entityListToEdit)
        {
            using (var transaction = new NHibernatorTransaction(_sessionName))
            {
                foreach (var item in entityListToEdit)
                {
                    _session.Save(item);
                }
                transaction.Commit();
            }
        }

        public override void Delete<T>(T entityToDelete)
        {
            using (var transaction = new NHibernatorTransaction(_sessionName))
            {
                _session.Evict(entityToDelete);
                _session.Delete(entityToDelete);
                transaction.Commit();
            }
        }

        public override IDisposable BeginTransaction()
        {
            return new NHibernatorTransaction(_sessionName);
        }

        public override void Commit(IDisposable transaction)
        {
            if (transaction != null)
                ((NHibernatorTransaction)transaction).Commit();
        }

        public override void Rollback(IDisposable transaction)
        {
            if (transaction != null)
                ((NHibernatorTransaction)transaction).RollBack();
        }
    }
}
