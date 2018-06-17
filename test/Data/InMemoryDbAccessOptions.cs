using System;
using CoreRepository.Test.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CoreRepository.Test.Data
{
    internal class InMemoryDbAccessOptions : IOptions<DbAccessOptions>
    {
        public DbAccessOptions Value => new DbAccessOptions
        {
            CreateContext = () =>
            {
                var options = new DbContextOptionsBuilder<TestDbContext>()
                      .UseInMemoryDatabase(Guid.NewGuid().ToString())
                      .Options;
                return new TestDbContext(options);
            }
        };
    }
}
