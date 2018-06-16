using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoreRepository
{
    /// <summary>
    /// Specifies the contract required for repositories that provide read-only data access.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity</typeparam>
    public interface IReadOnlyRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Asynchronously retrieve a collection of entities using the specified query shaper.
        /// </summary>
        /// <param name="queryShaper">A function used to filter the query.</param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryShaper);

        /// <summary>
        /// Asynchronously retrieve a collection of entities using the specified query shaper and cancellation token.
        /// </summary>
        /// <param name="queryShaper">A function used to filter the query.</param>
        /// <param name="cancellationToken">The token to observe while waiting for the task to complete.</param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryShaper, CancellationToken cancellationToken);

        /// <summary>
        /// Return the specified type of result using the provided query shaper.
        /// </summary>
        /// <typeparam name="TResult">The type of result to return.</typeparam>
        /// <param name="queryShaper">A function used to filter the query.</param>
        /// <returns></returns>
        Task<TResult> GetAsync<TResult>(Func<IQueryable<TEntity>, TResult> queryShaper);

        /// <summary>
        /// Return the specified type of result using the provided query shaper and cancellation token.
        /// </summary>
        /// <typeparam name="TResult">The type of result to return.</typeparam>
        /// <param name="queryShaper">A function used to filter the query.</param>
        /// <param name="cancellationToken">The token to observe while waiting for the task to complete.</param>
        /// <returns></returns>
        Task<TResult> GetAsync<TResult>(Func<IQueryable<TEntity>, TResult> queryShaper, CancellationToken cancellationToken);

        /// <summary>
        /// Query a data set using the specified filter function.
        /// </summary>
        /// <param name="queryShaper">A function used to filter the query.</param>
        /// <returns></returns>
        IQueryable<TEntity> Query(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryShaper);
    }
}
