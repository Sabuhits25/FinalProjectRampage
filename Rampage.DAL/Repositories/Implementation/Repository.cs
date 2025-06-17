using Microsoft.EntityFrameworkCore;
using Rampage.Core.Entities.Commons;
using Rampage.DAL.Context;
using Rampage.DAL.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Rampage.DAL.Repositories.Implementation
{
    public class Repository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : BaseEntity, IAuditedEntity
    {
        protected readonly AppDbContext Context;
        protected readonly DbSet<TEntity> DbSet;
        private bool _disposed = false;
        protected Repository(AppDbContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        public async Task<ICollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            return await AddIncludes(DbSet.Where(predicate), includes);
        }

        public async Task<TEntity?> GetByIdAsync(Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[]? includes)
        {
            return (await AddIncludes(DbSet, includes)).FirstOrDefault(predicate.Compile());
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
            await Context.SaveChangesAsync();

            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            DbSet.Update(entity);
            await Context.SaveChangesAsync();

            return entity;
        }

        public async Task<TEntity> DeleteAsync(TEntity entity)
        {
            entity.IsDeleted = true;

            DbSet.Update(entity);
            await Context.SaveChangesAsync();

            return entity;
        }

        public async Task<TEntity> RecoverAsync(TEntity entity)
        {
            entity.IsDeleted = false;

            DbSet.Update(entity);
            await Context.SaveChangesAsync();

            return entity;
        }

        public async Task<TEntity> RemoveAsync(TEntity entity)
        {
            DbSet.Remove(entity);
            await Context.SaveChangesAsync();

            return entity;
        }

        public async Task<ICollection<TEntity>> AddIncludes(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includes)
        {
            return await includes?.Aggregate(query, (current, include) => current.Include(include)).ToListAsync() ?? await query.ToListAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
