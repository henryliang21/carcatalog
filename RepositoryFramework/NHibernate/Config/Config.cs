using System;
using System.Configuration;

namespace RepositoryFramework.NHibernate.Config
{
    public static class Config
    {
        public static readonly string NHIBERNATE_DEV_MODE_KEY = "NHibernateSingleDBTestMode";
        public static readonly string NHIBERNATE_CONN_STR_NAME_KEY = "NHibernateConnStringName";
        public static readonly string NHIBERNATE_MAPPING_ASSEMBLY_KEY = "NHibernateMappingAssembly";

        private static bool _nhibernateSingleDBTestMode = false;
        private static string _nhibernateConnStringName = "";
        private static string _nhibernateMappingAssembly = "";

        public static bool SingleDBTestMode
        {
            get
            {
                return _nhibernateSingleDBTestMode;
            }
        }

        public static string ConnStringName
        {
            get
            {
                return _nhibernateConnStringName;
            }
        }

        public static string MappingAssembly
        {
            get
            {
                return _nhibernateMappingAssembly;
            }
        }

        static Config()
        {
            if (ConfigurationManager.AppSettings != null)
            {
                string nhibernateDevMode = ConfigurationManager.AppSettings[NHIBERNATE_DEV_MODE_KEY];
                if (!String.IsNullOrEmpty(nhibernateDevMode))
                {
                    _nhibernateSingleDBTestMode = Boolean.Parse(nhibernateDevMode);
                }
                _nhibernateConnStringName = ConfigurationManager.AppSettings[NHIBERNATE_CONN_STR_NAME_KEY];
                _nhibernateMappingAssembly = ConfigurationManager.AppSettings[NHIBERNATE_MAPPING_ASSEMBLY_KEY];
            }
        }
    }
}
