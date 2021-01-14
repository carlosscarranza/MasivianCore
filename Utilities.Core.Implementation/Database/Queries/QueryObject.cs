using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using LinqKit;
using Utilities.Core.Interfaces.Database.Queries;

namespace Utilities.Core.Implementation.Database.Queries
{
    public class QueryObject<TEntity> : IQueryObject<TEntity>
    {
        private Expression<Func<TEntity, bool>> _query;

        public virtual Expression<Func<TEntity, bool>> Query() => _query;

        public Expression<Func<TEntity, bool>> And(Expression<Func<TEntity, bool>> query) => _query = _query == null ? query : _query.And(query.Expand());

        public Expression<Func<TEntity, bool>> Or(Expression<Func<TEntity, bool>> query) => _query = _query == null ? query : _query.Or(query.Expand());

        public Expression<Func<TEntity, bool>> And(IQueryObject<TEntity> queryObject) => And(queryObject.Query());

        public Expression<Func<TEntity, bool>> Or(IQueryObject<TEntity> queryObject) => Or(queryObject.Query());
    }
}
