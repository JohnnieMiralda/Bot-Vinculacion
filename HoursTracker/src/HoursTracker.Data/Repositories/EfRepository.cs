using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HoursTracker.Domain;
using HoursTracker.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace HoursTracker.Data.Repositories
{
    public class EfRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity, IAggregateRoot
    {
        protected EfRepository(DbContext context)
        {
            Context = context;
        }

        private DbContext Context { get; }

        public IQueryable<TEntity> All()
        {
            return Context.Set<TEntity>();
        }

        public IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate)
        {
            return All().Where(predicate);
        }

        public async Task<TEntity> First(Expression<Func<TEntity, bool>> predicate)
        {
            return await Filter(predicate).FirstAsync();
        }

        public async Task<TEntity> FindById(int id)
        {
            return await FirstOrDefault(x => x.Id == id);
        }

        public async Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return await All().FirstOrDefaultAsync(predicate);
        }

        public async Task<TEntity> Add(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
            await Context.SaveChangesAsync();

            return entity;
        }

        public async Task Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }

        public async Task Delete(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
            await Context.SaveChangesAsync();
        }

        public async Task<TEntity> Disable(TEntity entity)
        {
            entity.Disabled = true;
            await Update(entity);
            return entity;
        }

        public int Count()
        {
            return Context.Set<TEntity>().Count();
        }

        public async Task Update<T, TKey>(IEnumerable<T> currentItems, IEnumerable<T> newItems, Func<T, TKey> getKey) where T : class
        {
            Context.TryUpdateManyToMany(currentItems, newItems, getKey);

            await Context.SaveChangesAsync();
        }
    }
}