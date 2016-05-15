using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;

namespace NHibernatorFramework 
{
    public interface ISessionStorage
    {
        Dictionary<string, ISession> Sessions
        {
            get;
        }
        ISession Get(string sessionFactoryKey);
        void Set(string sessionFactoryKey, ISession session);

        void Remove(string sessionFactoryKey);
        bool Exist(string sessionFactoryKey);

        SessionOpenModeType SessionOpenMode
        {
            get;
            set;
        }
    }
}
