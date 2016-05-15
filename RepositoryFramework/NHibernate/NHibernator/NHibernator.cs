using System;
using System.Reflection;
using System.Web;
using System.IO;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using System.Diagnostics;
using System.Configuration;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Cache;
using NHibernate.Tool.hbm2ddl;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using NHibernatorFramework;
using RepositoryFramework.NHibernate.Config;

namespace NHibernatorFramework
{
    public enum SessionOpenModeType
    {
        Unset,
        HttpRequest,
        AutoTransactionScope,
        Manual
    }

    public class NHibernator
    {
        private static readonly string NHIBERNATE_CFG_FILE_KEY = "NHibernatorConfigFileLocation";
        private static readonly string NHIBERNATE_DEFAULT_FILENAME_KEY = "hibernate.cfg.xml";
        private static NHibernate.Cfg.Configuration configuration;

        public static NHibernate.Cfg.Configuration Configuration
        {
            get { return NHibernator.configuration; }
            set { NHibernator.configuration = value; }
        }
        private static Dictionary<string, ISessionFactory> sessionFactories = new Dictionary<string,ISessionFactory>();
        private static ISessionStorage sessionStorage;
        private static bool httpModuleUndefined = true;

        public static ISessionStorage SessionStorage
        {
            get { return NHibernator.sessionStorage; }
            set { NHibernator.sessionStorage = value; }
        }
        
        public static bool HttpModuleUndefined
        {
            get { return NHibernator.httpModuleUndefined; }
            set { NHibernator.httpModuleUndefined = value; }
        }

        private static void BuildSchema(NHibernate.Cfg.Configuration config)
        {
            configuration = config;
            //if (Config.DevMode)
            //{
            //    new SchemaExport(config)
            //        .Create(false, true);  // WARNING: THIS WILL DROP THE MAPPED TABLES -- DO NOT USE ON A SHARED DB!
            //}
        }

        static NHibernator()
        {
            string configurationFilePath = null;
            string connectionStringName = null;

            string[] configurationFilePathArray = null;
            string nhibernateConfigFile;

            Assembly mappingAssembly = GetAssemblyFromName(Config.MappingAssembly);
            if (mappingAssembly == null)
            {
                throw new NHibernatorException(String.Format("Mapping Assembly Could Not Be Found, check appSettings>{0} in web.config", Config.NHIBERNATE_MAPPING_ASSEMBLY_KEY));
            }

            if (ConfigurationManager.AppSettings != null)
            {
                configurationFilePath = ConfigurationManager.AppSettings[NHIBERNATE_CFG_FILE_KEY];
                connectionStringName = Config.ConnStringName;
            }

            if (connectionStringName != null)
            {
                // Fluent NHibernate Support
                ISessionFactory sf = Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2012
                        .ConnectionString(c =>
                            c.FromConnectionStringWithKey(connectionStringName)))
                    .Cache(c =>
                        c.UseQueryCache()
                        .ProviderClass<HashtableCacheProvider>())
                    //.ShowSql())
                    .Mappings(m =>
                        m.FluentMappings.AddFromAssembly(mappingAssembly))
                    .ExposeConfiguration(BuildSchema)
                    .BuildSessionFactory();
                sessionFactories.Add(connectionStringName, sf);
            }
            else if (configurationFilePath != null)
            {
                configurationFilePathArray = configurationFilePath.Split(';');

                foreach (string configFilePath in configurationFilePathArray)
                {
                    if (configFilePath.StartsWith("~"))
                    {
                        nhibernateConfigFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFilePath.Substring(1));
                    }
                    else
                    {
                        nhibernateConfigFile = configFilePath;
                    }


                    if (File.Exists(nhibernateConfigFile))
                    {
                        string factoryName = null;

                        configuration = new NHibernate.Cfg.Configuration();
                        configuration.Configure(nhibernateConfigFile);
                        configuration.AddAssembly(mappingAssembly);
                        var sf = (ISessionFactoryImplementor)configuration.BuildSessionFactory();
                        if (!String.IsNullOrEmpty(sf.Settings.SessionFactoryName))
                        {
                            factoryName = sf.Settings.SessionFactoryName;
                        }
                        else if (configuration.Properties.ContainsKey("hibernate.session_factory_name"))
                        {
                            factoryName = configuration.Properties["hibernate.session_factory_name"];
                        }
                        else
                        {
                            // default session factory key
                            factoryName = string.Empty;
                        }
                        if (!sessionFactories.ContainsKey(factoryName))
                        {
                            sessionFactories.Add(factoryName, sf);
                        }
                    }
                }
            }

            if (sessionFactories.Count == 0)
            {
                configuration = new NHibernate.Cfg.Configuration();
                nhibernateConfigFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, NHIBERNATE_DEFAULT_FILENAME_KEY);
                if (File.Exists(nhibernateConfigFile))
                {
                    configuration.Configure(nhibernateConfigFile);
                }
                else
                {
                    configuration.Configure();
                }
                ISessionFactory sf = configuration.BuildSessionFactory();
                sessionFactories.Add(string.Empty, sf);
            }

            InitSessionStorge();

        }


        public static ISession GetSession()
        {
            return GetSession(string.Empty);
        }

        public static ISession GetSession(string sessionFactoryName)
        {
            // Here the SessionOpenModetype is set to HttpRequest when running as Web Application
            // Set to Unset when running as Windows or Console Application
            if ((sessionStorage.SessionOpenMode == SessionOpenModeType.HttpRequest)&&
                (!sessionStorage.Exist(sessionFactoryName)))
            {
                OpenSession(sessionFactoryName);
            }
            return sessionStorage.Get(sessionFactoryName);
        }

        public static bool SessionExist(string sessionFactoryName)
        {
            return sessionStorage.Exist(sessionFactoryName);
        }

        private static void InitSessionStorge() 
		{
            if (System.Web.HttpContext.Current != null)
            {
                sessionStorage = new HttpSessionStorage();
            }
            else
            {
                httpModuleUndefined = false;
                sessionStorage = new ThreadSessionStorage();
            }
            //try
            //{
            //    System.Web.ProcessModelInfo.GetCurrentProcessInfo();
            //    sessionStorage = new HttpSessionStorage();
            //} 
            //catch( System.Web.HttpException )
            //{
            //    httpModuleUndefined = false;
            //    sessionStorage = new ThreadSessionStorage();
            //}
		}

        public static void OpenSession()
        {
            OpenSession(string.Empty);
        }

        public static void OpenSession(string sessionFactoryName)
        {
            if (sessionStorage.SessionOpenMode == SessionOpenModeType.Unset)
            {
                sessionStorage.SessionOpenMode = SessionOpenModeType.Manual;
            }

            if (httpModuleUndefined)
            {
                throw new NHibernatorException("HttpModule not defined. Make sure OpenSessionInViewModule is defined in the web.config.");
            }
            
            if (!sessionFactories.ContainsKey(sessionFactoryName))
            {
                throw new NHibernatorException("SessionFactory not found, make sure session is defined in the session factory.");
            }
            ISessionFactory sessionFactory = sessionFactories[sessionFactoryName] as ISessionFactory;

            ISession session = sessionFactory.OpenSession();
            sessionStorage.Set(sessionFactoryName, session);
        }

        public static void CloseSession()
        {
            CloseSession(string.Empty);
        }

        public static void CloseSession(string sessionFactoryName)
        {
            ISession session = sessionStorage.Get(sessionFactoryName);
            if (session != null && session.IsOpen)
            {
                session.Close();
            }
            session = null;
            sessionStorage.Remove(sessionFactoryName);

        }

        internal static void BeginTransaction()
        {
            BeginTransaction(string.Empty);
        }

        internal static void BeginTransaction(string sessionFactoryName)
        {
            if (sessionStorage.SessionOpenMode == SessionOpenModeType.Unset)
            {
                sessionStorage.SessionOpenMode = SessionOpenModeType.AutoTransactionScope;
            }
            
            if (!sessionStorage.Exist(sessionFactoryName))
            {
                OpenSession(sessionFactoryName);
            }
            ISession session = sessionStorage.Get(sessionFactoryName);
            session.BeginTransaction();
        }

        internal static void Commit()
        {
            Commit(string.Empty);
        }

        internal static void Commit(string sessionFactoryName)
        {
            ISession session = sessionStorage.Get(sessionFactoryName);
            session.Transaction.Commit();
            if (sessionStorage.SessionOpenMode == SessionOpenModeType.AutoTransactionScope)
            {
                CloseSession(sessionFactoryName);
            }
        }

        internal static void Rollback()
        {
            Rollback(string.Empty);
        }

        internal static void Rollback(string sessionFactoryName)
        {
            ISession session = sessionStorage.Get(sessionFactoryName);
            session.Transaction.Rollback();
            if (sessionStorage.SessionOpenMode == SessionOpenModeType.AutoTransactionScope)
            {
                CloseSession(sessionFactoryName);
            }
        }

        private static Assembly GetAssemblyFromName(string assemblyName)
        {
            System.Collections.ICollection assemblyObjects;
            if (HttpContext.Current != null)
                assemblyObjects = System.Web.Compilation.BuildManager.GetReferencedAssemblies();
            else
                assemblyObjects = AppDomain.CurrentDomain.GetAssemblies();

            //if (assemblyObjects == null) throw new NullReferenceException("NHibernator could not load assemblies");
            foreach (Assembly assemblyObject in assemblyObjects)
            {
                var assembly = assemblyObject;
                if (assembly.ManifestModule.ToString().ToLower() == assemblyName.ToLower())
                    return assembly;
            }

            return null;
        }
    }
}
