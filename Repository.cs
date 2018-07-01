using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CoreRepository
{
    /// <summary>
    /// Provides generic methods for manipulating a single entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class Repository<TEntity> : IRepository<TEntity>, IQueryable<TEntity> where TEntity : class
    {
        #region fields

        readonly DbAccessOptions _dbAccessOptions;
        DbContext _context;
        IQueryable<TEntity> _contextSet;

        #endregion

        #region properties

        /// <summary>
        /// Gets the internal database context.
        /// </summary>
        public virtual DbContext Context { get => _context; }

        #endregion

        #region constructor

        /// <summary>
        /// Creates a new instance of the <see cref="Repository{TEntity}"/> class using the specified options.
        /// </summary>
        /// <param name="options">The database context access options.</param>
        public Repository(IOptions<DbAccessOptions> options)
        {
            if (options == null || options.Value == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            _dbAccessOptions = options.Value;
            InitContext();
        }

        #endregion

        #region IRepository<TEntity>

        /// <summary>
        /// Asynchronously retrieves a collection of entities using the specified query shaper.
        /// </summary>
        /// <param name="queryShaper">A function used to filter the query.</param>
        /// <returns></returns>
        public virtual Task<IEnumerable<TEntity>> GetAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryShaper)
            => GetAsync(queryShaper, default(CancellationToken));

        /// <summary>
        /// Asynchronously retrieves a collection of entities using the specified query shaper and cancellation token.
        /// </summary>
        /// <param name="queryShaper">A function used to filter the query.</param>
        /// <param name="cancellationToken">The token to observe while waiting for the task to complete.</param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntity>> GetAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryShaper, CancellationToken cancellationToken) => await Task.Run(() => queryShaper(_contextSet), cancellationToken);

        /// <summary>
        /// Returns the specified type of result using the provided query shaper.
        /// </summary>
        /// <typeparam name="TResult">The type of result to return.</typeparam>
        /// <param name="queryShaper">A function used to filter the query.</param>
        /// <returns></returns>
        public virtual Task<TResult> GetAsync<TResult>(Func<IQueryable<TEntity>, TResult> queryShaper)
            => GetAsync(queryShaper, default(CancellationToken));

        /// <summary>
        /// Returns the specified type of result using the provided query shaper and cancellation token.
        /// </summary>
        /// <typeparam name="TResult">The type of result to return.</typeparam>
        /// <param name="queryShaper">A function used to filter the query.</param>
        /// <param name="cancellationToken">The token to observe while waiting for the task to complete.</param>
        /// <returns></returns>
        public virtual async Task<TResult> GetAsync<TResult>(Func<IQueryable<TEntity>, TResult> queryShaper, CancellationToken cancellationToken) => await Task.Run(() => queryShaper(_contextSet));

        /// <summary>
        /// Returns the underlying query set for the <typeparamref name="TEntity"/>.
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> Query() => _contextSet;

        /// <summary>
        /// Query a data set using the specified filter function.
        /// </summary>
        /// <param name="queryShaper">A function used to filter the query.</param>
        /// <returns></returns>
        public IQueryable<TEntity> Query(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryShaper)
            => queryShaper(_contextSet);

        /// <summary>
        /// Sets the item's entity state to <see cref="EntityState.Added"/>.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public virtual void Add(TEntity item) => Context.Entry(item).State = EntityState.Added;

        /// <summary>
        /// Sets the item's entity state to <see cref="EntityState.Modified"/>.
        /// </summary>
        /// <param name="item">The item that was modified.</param>
        public virtual void Update(TEntity item) => Context.Entry(item).State = EntityState.Modified;

        /// <summary>
        /// Sets the item's entity state to <see cref="EntityState.Deleted"/>.
        /// </summary>
        /// <param name="item">The item to delete.</param>
        public virtual void Remove(TEntity item) => Context.Entry(item).State = EntityState.Deleted;

        /// <summary>
        /// Asynchronously saves all changes made in the underlying context to the database.
        /// </summary>
        /// <returns></returns>
        public virtual Task SaveChangesAsync() => Context.SaveChangesAsync();

        /// <summary>
        /// Asynchronously saves all changes made in the underlying context to the database.
        /// </summary>
        /// <param name="cancellationToken">The token to observe while waiting for the task to complete.</param>
        /// <returns></returns>
        public virtual Task SaveChangesAsync(CancellationToken cancellationToken) => Context.SaveChangesAsync(cancellationToken);

        /// <summary>
        /// Checks if any new, deleted, or changed entities are being tracked such that 
        /// these changes will be sent to the database if <see cref="SaveChangesAsync()"/> 
        /// or  <see cref="SaveChangesAsync(CancellationToken)"/> is called.
        /// </summary>
        public virtual bool HasPendingChanges => Context.ChangeTracker.HasChanges();

        /// <summary>
        /// Ignores all changes made so far to the underlying data context.
        /// </summary>
        public virtual void DiscardChanges()
        {
            // just recreate the context if we don't care about the changes.
            Context.Dispose();
            InitContext();
        }

        #endregion

        #region IQueryable<TEntity> Members

        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
        {
            return _contextSet.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _contextSet.GetEnumerator();
        }

        Type IQueryable.ElementType
        {
            get { return typeof(TEntity); }
        }

        Expression IQueryable.Expression
        {
            get { return _contextSet.Expression; }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return _contextSet.Provider; }
        }

        #endregion

        #region helpers

        void InitContext()
        {
            var ctx = _dbAccessOptions.CreateContext();
            _context = ctx ?? throw new InvalidOperationException($"{nameof(DbAccessOptions.CreateContext)} should not return a null reference.");
            _contextSet = _context.Set<TEntity>();
        }

        #endregion
    }
}
