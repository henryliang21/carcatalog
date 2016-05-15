using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace RepositoryFramework.GenericRepository
{
    public abstract class GenericRepositoryBase<T, ID> : IGenericRepository<T, ID>
    {
        const string KeyPropertyName = "Id";

        protected Expression<Func<T, bool>> CreateGetExpression<T>(ID id)
        {
            ParameterExpression e = Expression.Parameter(typeof(T), "e");
            PropertyInfo propInfo = typeof(T).GetProperty(KeyPropertyName);
            MemberExpression m = Expression.MakeMemberAccess(e, propInfo);
            ConstantExpression c = Expression.Constant(id, typeof(ID));
            BinaryExpression b = Expression.Equal(m, c);
            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(b, e);
            return lambda;
        }
        
        protected Expression<Func<T, bool>> CreateGetExpressionFromCompositeIds<T>(Dictionary<string, object> idValues)
        {
            Expression<Func<T, bool>> lambda
                = Expression.Lambda<Func<T, bool>>(Expression.Equal(Expression.Constant(1), Expression.Constant(1)),
                Expression.Parameter(typeof(T), "e"));
            foreach (var item in idValues)
            {
                ParameterExpression e = Expression.Parameter(typeof(T), "e");
                PropertyInfo propInfo = typeof(T).GetProperty(item.Key);
                MemberExpression m = Expression.MakeMemberAccess(e, propInfo);
                ConstantExpression c = Expression.Constant(item.Value, item.Value.GetType());
                BinaryExpression b = Expression.Equal(m, c);
                Expression<Func<T, bool>> itemLambda = Expression.Lambda<Func<T, bool>>(b, e);
                lambda = Expression.Lambda<Func<T, bool>>(Expression.AndAlso(lambda.Body, itemLambda.Body), e);
            }
            return lambda;
        }

        protected int GetKeyPropertyValue<T>(object entity)
        {
            return (int)typeof(T).GetProperty(KeyPropertyName).GetValue(entity, null);
        }


        #region IGenericRepository Members

        public abstract IQueryable<T> List<T>();

        public abstract T Get<T>(ID id);

        public abstract T Get<T>(Dictionary<string, object> compositeIds);

        public abstract void Create<T>(T entityToCreate);

        public abstract void Edit<T>(T entityToEdit);

        public abstract void Delete<T>(T entityToDelete);

        public abstract void CreateFast<T>(T entityToCreate);

        public abstract void CreateBulk<T>(IEnumerable<T> entityListToCreate);

        public abstract void EditFast<T>(T entityToEdit);

        public abstract void EditBulk<T>(IEnumerable<T> entityListToEdit);

        public abstract IDisposable BeginTransaction();

        public abstract void Commit(IDisposable transaction);

        public abstract void Rollback(IDisposable transaction);

        #endregion
    }
}
