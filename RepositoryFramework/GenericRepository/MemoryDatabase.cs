using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryFramework.GenericRepository
{
    public class MemoryDatabase
    {
        const string KeyPropertyName = "Id";

        private Dictionary<Type, object> _tables = new Dictionary<Type, object>();

        private void EnsureTable<T>()
        {
            if (_tables.ContainsKey(typeof(T)) == false)
                _tables.Add(typeof(T), new List<T>());
        }

        private List<T> GetTable<T>()
        {
            return (List<T>)_tables[typeof(T)];
        }


        private void SetIdentityValue<T>(T entityToInsert)
        {
            var count = GetTable<T>().Count + 1;
            typeof(T).GetProperty(KeyPropertyName).SetValue(entityToInsert, count, null);
        }

        public IQueryable<T> Select<T>()
        {
            EnsureTable<T>();
            return ((List<T>)_tables[typeof(T)]).AsQueryable();
        }

        public void Insert<T>(T entityToInsert)
        {
            EnsureTable<T>();
            SetIdentityValue<T>(entityToInsert);
            GetTable<T>().Add(entityToInsert);
        }


        public void Update<T>(T originalEntity, T modifiedEntity)
        {
            var properties = typeof(T).GetProperties();
            foreach (var prop in properties)
            {
                if (prop.CanWrite && prop.Name != "Id")
                {
                    var value = prop.GetValue(modifiedEntity, null);
                    prop.SetValue(originalEntity, value, null);
                }
            }
        }


        public void Delete<T>(T entityToDelete)
        {
            EnsureTable<T>();
            GetTable<T>().Remove(entityToDelete);
        }

    }
}
