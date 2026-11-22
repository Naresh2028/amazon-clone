using Amazon.Infrastructure.Persistance;
using AmazonClone.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmazonClone.Infrastructure.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(AppDbContext dbContext,
            IDbContextTransaction? transaction) 
        {
            _dbContext = dbContext;
            _transaction = transaction;
        }
        public async Task BeginTransactionAsync()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_transaction != null) 
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollBackAsync()
        {
            if (_transaction != null) 
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }
}
