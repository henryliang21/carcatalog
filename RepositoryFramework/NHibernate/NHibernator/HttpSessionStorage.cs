using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using NHibernate;

namespace NHibernatorFramework
{
    public class HttpSessionStorage : ISessionStorage
    {
        private static readonly string NHIBERNATE_SESSION_KEY = "NHibernator.Session";
        private static readonly string NHIBERNATE_OPENMODE_KEY = "NHibernator.OpenMode";

        public Dictionary<string, ISession> Sessions
        {
            get 
            {
                return HttpContext.Current.Items[NHIBERNATE_SESSION_KEY] as Dictionary<string, ISession>; 
            }
        }

        public ISession Get(string sessionFactoryKey)
        {
            ISession session = null;
            Dictionary<string, ISession> sessionMap =
                HttpContext.Current.Items[NHIBERNATE_SESSION_KEY] as Dictionary<string, ISession>;

            if (sessionMap != null)
            {
                session = sessionMap[sessionFactoryKey] as ISession;
            }

            if (session == null)
            {
                throw new NHibernatorException("Unable to find session. SessionFactory not initialized.");
            }
            return session;
        }

        public void Set(string sessionFactoryKey, ISession session)
        {
            Dictionary<string, ISession> sessionMap =
                HttpContext.Current.Items[NHIBERNATE_SESSION_KEY] as Dictionary<string, ISession>;

            if (sessionMap == null)
            {
                sessionMap = new Dictionary<string, ISession>();
                HttpContext.Current.Items[NHIBERNATE_SESSION_KEY] = sessionMap;
            }
            sessionMap[sessionFactoryKey] = session;
        }

        public void Remove(string sessionFactoryKey)
        {
            Dictionary<string, ISession> sessionMap =
                HttpContext.Current.Items[NHIBERNATE_SESSION_KEY] as Dictionary<string, ISession>;

            if (sessionMap != null)
            {
                sessionMap[sessionFactoryKey] = null;
            }

        }

        public bool Exist(string sessionFactoryKey)
        {
            if (Sessions != null)
            {
                return Sessions.ContainsKey(sessionFactoryKey);
            }
            else
            {
                return false;
            }
            
        }

        public SessionOpenModeType SessionOpenMode
        {
            get
            {
                object openMode = HttpContext.Current.Items[NHIBERNATE_OPENMODE_KEY];
                if (openMode != null)
                {
                    return (SessionOpenModeType)openMode;    
                }
                else
                {
                    return SessionOpenModeType.Unset;
                }
            }
            set
            {
                HttpContext.Current.Items[NHIBERNATE_OPENMODE_KEY] = value;
            }
        }

    }
}
