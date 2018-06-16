using System;
using Microsoft.EntityFrameworkCore;

namespace CoreRepository
{
    /// <summary>
    /// Exposes a property used to create new instances of the <see cref="DbContext"/> class.
    /// </summary>
    public class DbAccessOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbAccessOptions"/> class.
        /// </summary>
        public DbAccessOptions()
        {
        }

        /// <summary>
        /// Gets or sets the function used to create a new instance of the <see cref="DbContext"/> class.
        /// </summary>
        public Func<DbContext> CreateContext { get; set; }
    }
}
