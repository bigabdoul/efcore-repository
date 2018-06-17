using System;

namespace CoreRepository.Test.Data
{
    /// <summary>
    /// Represents a simple repository that uses an instance of a class that 
    /// implements the <see cref="ISqliteDbAccessOptions"/> interface in its constructor.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class SqliteRepository<TEntity> : Repository<TEntity>, IDisposable where TEntity : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqliteRepository{TEntity}"/> class.
        /// </summary>
        /// <param name="options">An object used to create an in-memory Sqlite database context.</param>
        public SqliteRepository(ISqliteDbAccessOptions options) : base(options)
        {
        }

        /// <summary>
        /// Releases the allocated resources for the underlying context.
        /// </summary>
        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}
