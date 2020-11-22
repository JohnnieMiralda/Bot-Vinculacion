using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HoursTracker.Domain.Contracts
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity, IAggregateRoot
    {
        /// <summary>
        ///     When is override in a derived type, returns all the elements in the entity set.
        /// </summary>
        /// <returns>
        ///     An <see cref="IQueryable" /> for all the elements in the entity set
        /// </returns>
        public IQueryable<TEntity> All();

        /// <summary>
        ///     Filters an entity set of values based on a predicate.
        /// </summary>
        /// <param name="predicate">
        ///     An expression to test each element for a condition.
        /// </param>
        /// <returns>
        ///     An <see cref="IQueryable" /> that contains elements from the input sequence that satisfy the condition specified by
        ///     predicate.
        /// </returns>
        public IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        ///     Returns the first element of a sequence.
        /// </summary>
        /// <param name="predicate">
        ///     An expression to return the first element of a condition.
        /// </param>
        /// <returns>
        ///     The first element that satisfies the condition.
        /// </returns>
        public Task<TEntity> First(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        ///     /// Search entities by id.
        /// </summary>
        /// <param name="id">Entity identifier.</param>
        /// <returns>A task of the entity.</returns>
        public Task<TEntity> FindById(int id);


        /// <summary>
        ///     Returns the first element of the entity set with the specified condition, or a default value if the entity set
        ///     contains no elements.
        /// </summary>
        /// <param name="predicate">
        ///     A function to test each element for a condition.
        /// </param>
        /// <returns>
        ///     default value, if the source is empty; otherwise, the first element in source.
        /// </returns>
        public Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        ///     Marks the entity as added
        /// </summary>
        /// <param name="entity">
        ///     The entity.
        /// </param>
        /// <returns>
        ///     The entity with its state marked as added.
        /// </returns>
        public Task<TEntity> Add(TEntity entity);

        /// <summary>
        ///     Marks the entity as modified.
        /// </summary>
        /// <param name="entity">
        ///     The entity.
        /// </param>
        /// <returns>
        ///     The entity with its state marked as modified.
        /// </returns>
        public Task Update(TEntity entity);

        public Task Update<T, TKey>(IEnumerable<T> currentItems, IEnumerable<T> newItems, Func<T, TKey> getKey) where T : class;

        /// <summary>
        ///     Marks the entity as deleted.
        /// </summary>
        /// <param name="entity">
        ///     The entity.
        /// </param>
        /// <returns>
        ///     The entity with its state marked as deleted.
        /// </returns>
        public Task Delete(TEntity entity);

        public Task<TEntity> Disable(TEntity entity);

        public int Count();
    }
}