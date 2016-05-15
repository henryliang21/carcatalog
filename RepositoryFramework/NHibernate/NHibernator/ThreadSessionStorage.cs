using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;

namespace NHibernatorFramework
{
    public class ThreadSessionStorage : ISessionStorage
    {
        [ThreadStatic]
        private static Dictionary<string, ISession> staticSessions = new Dictionary<string, ISession>();
        
        [ThreadStatic]
        private static SessionOpenModeType sessionOpenMode = SessionOpenModeType.Unset;

        public Dictionary<string, ISession> Sessions
        {
            get 
            {
                if (staticSessions == null)
                {
                    staticSessions = new Dictionary<string, ISession>();
                }
                return ThreadSessionStorage.staticSessions; 
            }
        }

        public ISession Get(string sessionFactoryKey)
        {
            ISession session = Sessions[sessionFactoryKey];
            if (session == null)
            {
                throw new NHibernatorException("Unable to find session. SessionFactory not initialized.");
            }
            return session;
        }

        public void Set(string sessionFactoryKey, ISession session)
        {
            Sessions[sessionFactoryKey] = session;
        }

        public void Remove(string sessionFactoryKey)
        {
            Sessions[sessionFactoryKey] = null;
            Sessions.Remove(sessionFactoryKey);
        }

        public bool Exist(string sessionFactoryKey)
        {
            return Sessions.ContainsKey(sessionFactoryKey);
        }

        public SessionOpenModeType SessionOpenMode
        {
            get
            {
                return sessionOpenMode;
            }
            set
            {
                sessionOpenMode = value;
            }
        }
    }
}
