using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CoreRepository
{
    /// <summary>
    /// Specifies the contract required for repositories that provide read-write data access.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Set the item's entity state to <see cref="EntityState.Added"/>.
        /// </summary>
        /// <param name="item">The item to add.</param>
        void Add(TEntity item);

        /// <summary>
        /// Set the item's entity state to <see cref="EntityState.Modified"/>.
        /// </summary>
        /// <param name="item">The item that was modified.</param>
        void Update(TEntity item);

        /// <summary>
        /// Set the item's entity state to <see cref="EntityState.Deleted"/>.
        /// </summary>
        /// <param name="item">The item to delete.</param>
        void Remove(TEntity item);

        /// <summary>
        /// Asynchronously save all changes made in the underlying context to the database.
        /// </summary>
        /// <returns></returns>
        Task SaveChangesAsync();

        /// <summary>
        /// Asynchronously save all changes made in the underlying context to the database.
        /// </summary>
        /// <param name="cancellationToken">The token to observe while waiting for the task to complete.</param>
        /// <returns></returns>
        Task SaveChangesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Checks if any new, deleted, or changed entities are being tracked such that 
        /// these changes will be sent to the database if <see cref="SaveChangesAsync()"/> 
        /// or  <see cref="SaveChangesAsync(CancellationToken)"/> is called.
        /// </summary>
        bool HasPendingChanges { get; }

        /// <summary>
        /// Ignore all changes made so far to the underlying data context.
        /// </summary>
        void DiscardChanges();
    }
}
