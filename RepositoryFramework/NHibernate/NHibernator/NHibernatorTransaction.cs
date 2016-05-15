using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;

namespace NHibernatorFramework
{
    public class NHibernatorTransaction : IDisposable
    {
        string sessionFactoryName = string.Empty;
        ITransaction transaction = null;

        public NHibernatorTransaction() : this(string.Empty)
        {

        }

        public NHibernatorTransaction(string sessionFactoryName)
        {
            this.sessionFactoryName = sessionFactoryName;
            NHibernator.BeginTransaction(sessionFactoryName);
            transaction = NHibernator.GetSession(sessionFactoryName).Transaction;

        }

        public void Dispose()
        {
            if (NHibernator.SessionExist(sessionFactoryName))
	        {
                ISession session = NHibernator.GetSession(sessionFactoryName);
                if ((transaction.WasCommitted == false) && (transaction.WasRolledBack == false))
                {
                    NHibernator.Rollback(sessionFactoryName);
                } 
	        }
        }

        public void Commit()
        {
            try
            {
                NHibernator.Commit(sessionFactoryName);
            }
            catch
            {
                NHibernator.Rollback(sessionFactoryName);
                throw;
            }
        }

        public void RollBack()
        {
            NHibernator.Rollback(sessionFactoryName);
        }
    }
}
