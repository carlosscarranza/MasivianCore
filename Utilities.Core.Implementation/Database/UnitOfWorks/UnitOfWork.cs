﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommonServiceLocator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Utilities.Core.Implementation.Database.Repositories;
using Utilities.Core.Interfaces.Database.Repositories;
using Utilities.Core.Interfaces.Database.UnitOfWorks;

namespace Utilities.Core.Implementation.Database.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWorkAsync
    {
        #region Private Fields
        private bool _disposed;
        private DbContext _context;
        private readonly IDbContextTransaction _transaction;
        private Dictionary<string, dynamic> _repositories;
        
        #endregion Private Fields

        #region Constuctor/Dispose

        public UnitOfWork(DbContext context, IDbContextTransaction transaction)
        {
            _context = context;
            _transaction = transaction;
            _repositories = new Dictionary<string, dynamic>();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                try
                {
                    if (_context != null)
                    {
                        _context.Dispose();
                        _context = null;
                    }
                }
                catch (ObjectDisposedException)
                {
                    // 
                }
            }
            _disposed = true;
        }

        #endregion Constuctor/Dispose

        public int SaveChanges()
        {
            SyncObjectsStatePreCommit();
            var changes = _context.SaveChanges();
            SyncObjectsStatePostCommit();
            return changes;
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (ServiceLocator.IsLocationProviderSet)
            {
                return ServiceLocator.Current.GetInstance<IRepository<TEntity>>();
            }

            return RepositoryAsync<TEntity>();
        }
        
        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            throw new NotImplementedException();
        }

        public async Task<int> SaveChangesAsync() => await this.SaveChangesAsync(CancellationToken.None);

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            SyncObjectsStatePreCommit();
            var changesAsync = await _context.SaveChangesAsync(cancellationToken);
            SyncObjectsStatePostCommit();
            return changesAsync;
        }

        public IRepositoryAsync<TEntity> RepositoryAsync<TEntity>() where TEntity : class
        {
            if (ServiceLocator.IsLocationProviderSet)
            {
                return ServiceLocator.Current.GetInstance<IRepositoryAsync<TEntity>>();
            }

            if (_repositories == null)
            {
                _repositories = new Dictionary<string, dynamic>();
            }

            var type = typeof(TEntity).Name;

            if (_repositories.ContainsKey(type))
            {
                return (IRepositoryAsync<TEntity>)_repositories[type];
            }

            var repositoryType = typeof(Repository<>);

            _repositories.Add(type, Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context, this));

            return _repositories[type];
        }

        #region Unit of Work Transactions

        public bool Commit()
        {
            _transaction.Commit();
            return true;
        }

        public void Rollback()
        {
            _transaction.Rollback();
            SyncObjectsStatePostCommit();
        }

        #endregion

        #region State

        public void SyncObjectsStatePreCommit()
        {
            foreach (var dbEntityEntry in _context.ChangeTracker.Entries())
            {
                dbEntityEntry.State = StateHelper.ConvertState(dbEntityEntry.State);
            }
        }

        public void SyncObjectsStatePostCommit()
        {
            foreach (var dbEntityEntry in _context.ChangeTracker.Entries())
            {
                dbEntityEntry.State = StateHelper.ConvertState(dbEntityEntry.State);
            }
        }

        public void SyncObjectState<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Entry(entity).State = StateHelper.ConvertState(_context.Entry(entity).State);
        }

        #endregion
    }
}
