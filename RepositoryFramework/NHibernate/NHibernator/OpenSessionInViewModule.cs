using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using NHibernate;

namespace NHibernatorFramework
{
    public class OpenSessionInViewModule : IHttpModule
    {
        
        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
            context.EndRequest += new EventHandler(context_EndRequest);
            NHibernator.HttpModuleUndefined = false;
        }

        #endregion


        private void context_BeginRequest(object sender, EventArgs e)
        {
            //connections will be opened in when transaction is opened
            //foreach (KeyValuePair<string, ISessionFactory> sf in sessionFactories)
            //{
            //    OpenSession(sf.Key);
            //}
            NHibernator.SessionStorage.SessionOpenMode = SessionOpenModeType.HttpRequest;
        }


        private void context_EndRequest(object sender, EventArgs e)
        {
            Dictionary<string, ISession> sessions = NHibernator.SessionStorage.Sessions;
            if (sessions != null)
            {
                List<string> sessionsToClose = new List<string>();

                foreach (KeyValuePair<string, ISession> session in sessions)
                {
                    sessionsToClose.Add(session.Key);
                }

                foreach (string sessionKey in sessionsToClose)
                {
                    NHibernator.CloseSession(sessionKey);
                }
            }
        }

    }
}
