using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Utilities.Core.Implementation.Database.Queries;
using Utilities.Core.Interfaces.Database.Queries;
using Utilities.Core.Interfaces.Database.Repositories;
using Utilities.Core.Interfaces.Database.UnitOfWorks;

namespace Utilities.Core.Implementation.Database.Repositories
{
    public class Repository<TEntity> : IRepositoryAsync<TEntity> where TEntity : class
    {
        #region Private Fields

        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;
        private readonly IUnitOfWorkAsync _unitOfWork;

        #endregion Private Fields

        public Repository(DbContext context, IUnitOfWorkAsync unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            if (_context != null)
            {
                _dbSet = _context.Set<TEntity>();
            }
        }

        public virtual TEntity Find(params object[] keyValues) => _dbSet.Find(keyValues);

        public TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            var query = ApplyDefaultFilters(_dbSet);
            return query.FirstOrDefault(predicate);
        }

        public Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var query = ApplyDefaultFilters(_dbSet);
            return query.FirstOrDefaultAsync(predicate);
        }

        public TEntity Find<TKey>(Expression<Func<TEntity, TKey>> sortExpression, bool isDesc, Expression<Func<TEntity, bool>> predicate) => isDesc ? _dbSet.OrderBy(sortExpression).FirstOrDefault(predicate) : _dbSet.OrderByDescending(sortExpression).FirstOrDefault(predicate);
        public IQueryable<TEntity> SelectQuery(string query, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Added;
            var entityDb = await _dbSet.AddAsync(entity);
            _unitOfWork.SyncObjectState(entity);
            return entityDb.Entity;
        }

        public virtual void Insert(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Added;
            _dbSet.Add(entity);
            _unitOfWork.SyncObjectState(entity);
        }

        public virtual void InsertRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Insert(entity);
            }
        }

        public virtual Task<TEntity> UpdateAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _unitOfWork.SyncObjectState(entity);
            return Task.FromResult(entity);
        }

        public virtual void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _unitOfWork.SyncObjectState(entity);
        }

        public IQueryable<I> ExecuteSqlCommand<I>(string query, params object[] parameters) where I : class
        {
            throw new NotImplementedException();
        }

        public virtual Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public virtual void Delete(object id)
        {
            var entity = _dbSet.Find(id);
            Delete(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
            _unitOfWork.SyncObjectState(entity);
        }

        public virtual IQueryable<TEntity> GetAll(bool withoutDefaultFilters = false)
        {
            var query2 = ApplyDefaultFilters(_dbSet);
            return query2;
        }

        public virtual DbSet<TEntity> Get() => _dbSet;
        public IQueryFluent<TEntity> Query(IQueryObject<TEntity> queryObject)
        {
            throw new NotImplementedException();
        }

        public IQueryFluent<TEntity> Query(Expression<Func<TEntity, bool>> query)
        {
            throw new NotImplementedException();
        }

        public IQueryFluent<TEntity> Query()
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> Queryable() => _dbSet;

        public IRepository<T> GetRepository<T>() where T : class => _unitOfWork.Repository<T>();
        public IQueryable<TEntity> SelectQueryInterpolated(FormattableString query, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public int RunProcedure(string name, object parameters = null)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<TEntity> FindAsync(params object[] keyValues) => await _dbSet.FindAsync(keyValues);

        public virtual async Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues) => await _dbSet.FindAsync(keyValues);

        public virtual async Task<bool> DeleteAsync(params object[] keyValues) => await DeleteAsync(CancellationToken.None, keyValues);

        public virtual async Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            var entity = await FindAsync(cancellationToken, keyValues);

            if (entity == null)
            {
                return false;
            }

            _context.Entry(entity).State = EntityState.Deleted;
            //_context.Set<TEntity>().Attach(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public int ExecuteSqlCommand(FormattableString query, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        internal IQueryable<TEntity> Select(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (predicate != null)
            {
                query = query.AsExpandable().Where(predicate);
            }
            if (page != null && pageSize != null)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }
            return query;
        }

        internal async Task<IEnumerable<TEntity>> SelectAsync(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null)
        {
            return await Select(predicate, orderBy, includes, page, pageSize).ToListAsync();
        }

        public virtual IQueryable<TEntity> Filter(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (predicate != null)
            {
                query = query.AsExpandable().Where(predicate);
            }
            if (page != null && pageSize != null)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }
            return query;
        }

        public virtual async Task<IEnumerable<TEntity>> FilterAsync(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null)
        {
            return await Select(predicate, orderBy, includes, page, pageSize).ToListAsync();
        }

        public Task<int> Commit()
        {
            return _unitOfWork.SaveChangesAsync();
        }

        public Task<List<TOutput>> RunProcedure<TOutput>(string name, object parameters = null) where TOutput : new()
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> ApplyDefaultFilters(IQueryable<TEntity> query)
        {
            return query;
        }

    }
}
