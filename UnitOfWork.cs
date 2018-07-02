using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CoreRepository
{
    /// <summary>
    /// Implements the code contract required for a unit of work involving a group of repositories.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        #region fields

        readonly ConcurrentDictionary<Type, object> _repositories = new ConcurrentDictionary<Type, object>();

        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class using the specified parameter.
        /// </summary>
        /// <param name="options">An object used to create an instance of the <see cref="DbContext"/> class.</param>
        public UnitOfWork(IOptions<DbAccessOptions> options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }

        #endregion

        #region protected

        #region properties

        /// <summary>
        /// Gets or sets the database context for the current <see cref="UnitOfWork"/>.
        /// </summary>
        protected DbContext Context { get; set; }

        /// <summary>
        /// Gets the object used for synchronization.
        /// </summary>
        protected object SyncRoot { get; } = new object();

        /// <summary>
        /// Gets the options that initialized the current instance.
        /// </summary>
        protected IOptions<DbAccessOptions> Options { get; }

        #endregion

        #region helpers

        /// <summary>
        /// Creates and returns a new instance of the <see cref="Repository{T}"/> class and defines, if not already done, the <see cref="Context"/>.
        /// </summary>
        /// <typeparam name="T">The type of repository to create.</typeparam>
        /// <returns></returns>
        protected virtual IRepository<T> CreateRepository<T>() where T : class
        {
            lock (SyncRoot)
            {
                var repo = new Repository<T>(Options);
                if (Context == null)
                {
                    Context = repo.Context;
                }
                return repo;
            }
        }

        #endregion

        #endregion

        #region IUnitOfWork

        /// <summary>
        /// Returns or creates a new repository for the specified type.
        /// </summary>
        /// <typeparam name="T">The type of repository to return.</typeparam>
        /// <returns></returns>
        public virtual IRepository<T> Repository<T>() where T : class
        {
            if (_repositories.TryGetValue(typeof(T), out var repo))
            {
                return (IRepository<T>)repo;
            }

            repo = CreateRepository<T>() ??
                throw new InvalidOperationException("Method must create a repository and return a non-null reference.");

            if (!_repositories.TryAdd(typeof(T), repo))
                throw new Exception("Could not add the new repository for the specified type to the dictionary.");

            return (IRepository<T>)repo;
        }

        /// <summary>
        /// Saves the changes made to all repositories so far.
        /// </summary>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns></returns>
        public virtual Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (Context == null)
            {
                throw new InvalidOperationException();
            }
            return Context.SaveChangesAsync(cancellationToken);
        } 

        #endregion
    }
}
