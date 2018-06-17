using System;
using CoreRepository.Test.Data;
using CoreRepository.Test.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CoreRepository.Test.DependencyInjection
{
    internal static class Extensions
    {
        public static DependencyInjectionProvider AddRepositories(this DependencyInjectionProvider provider)
        {
            provider.Services.AddRepositories();
            return provider;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddScoped<IOptions<DbAccessOptions>, InMemoryDbAccessOptions>()
                .AddScoped<ISqliteDbAccessOptions, InMemorySqliteDbAccessOptions>()
                .AddScoped<IRepository<Blog>, Repository<Blog>>()
                .AddScoped<IRepository<Category>, SqliteRepository<Category>>()
                .AddScoped<IRepository<Product>, SqliteRepository<Product>>()
                //.AddDbContext<TestDbContext>(options => options.UseSqlite(CreateInMemorySqliteConnection()))
                ;
        }
        
        internal static SqliteConnection CreateInMemorySqliteConnection()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            return connection;
        }
        
    }
}
