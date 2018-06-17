using Microsoft.EntityFrameworkCore;
using static CoreRepository.Test.DependencyInjection.Extensions;

namespace CoreRepository.Test.Data
{
    internal class InMemorySqliteDbAccessOptions : ISqliteDbAccessOptions
    {
        public DbAccessOptions Value => new DbAccessOptions
        {
            CreateContext = () =>
            {
                var context = new TestDbContext(RepositoryTestsBase.GetDbContextOptions());
                context.Database.EnsureCreated();
                return context;
            }
        };
    }
}
