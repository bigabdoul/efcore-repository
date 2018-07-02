using System.Threading;
using System.Threading.Tasks;

namespace CoreRepository
{
    /// <summary>
    /// Specifies the code contract required for a unit of work involving a group of repositories.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Return a repository for the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the repository to return.</typeparam>
        /// <returns></returns>
        IRepository<T> Repository<T>() where T : class;

        /// <summary>
        /// Persist all changes made to all repositories so far.
        /// </summary>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete</param>
        /// <returns></returns>
        Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
