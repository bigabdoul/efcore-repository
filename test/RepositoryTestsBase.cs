using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CoreRepository.Test.Data;
using CoreRepository.Test.DependencyInjection;
using CoreRepository.Test.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoreRepository.Test
{
    public class RepositoryTestsBase
    {
        protected DependencyInjectionProvider DIProvider;

        [TestInitialize]
        public void Init() => DIProvider = DependencyInjectionProvider.Create().AddRepositories().Build();

        [TestCleanup]
        public void CloseConnection()
        {
            if (_sqliteConnection != null)
            {
                _sqliteConnection.Close();
                _sqliteConnection.Dispose();
            }
        }

        #region helpers

        protected Blog[] CreateBlogs()
        {
            return new Blog[]
            {
                new Blog { Url = "http://sample.com/cats" },
                new Blog { Url = "http://sample.com/catfish" },
                new Blog { Url = "http://sample.com/dogs" }
            };
        }

        protected async Task<Blog[]> InsertBlogsAsync()
        {
            var repo = DIProvider.GetRepository<Blog>();
            var blogs = CreateBlogs();

            foreach (var blog in blogs)
            {
                repo.Add(blog);
            }

            await repo.SaveChangesAsync();
            return blogs;
        }

        protected async Task InsertCategoriesAsync()
        {
            var categories = DIProvider.GetSqliteRepository<Category>();
            AddCategories(categories);
            await categories.SaveChangesAsync();
        }

        protected void AddCategories(IRepository<Category> categories)
        {
            var cars = new Category { Id = 1, Name = "Cars" };
            var computers = new Category { Id = 2, Name = "Computers" };

            categories.Add(cars);
            categories.Add(computers);
        }

        protected virtual async Task InsertProductsAsync()
        {
            var categories = DIProvider.GetSqliteRepository<Category>();
            var products = DIProvider.GetSqliteRepository<Product>();

            var cars = await categories.GetAsync(q => q.Where(c => c.Name == "Cars").Single());
            var computers = await categories.GetAsync(q => q.Where(c => c.Name == "Computers").Single());

            AddCars(products, cars);
            AddComputers(products, computers);

            await products.SaveChangesAsync();
        }

        protected async Task InsertCategoriesAndProductsAsync()
        {
            await InsertCategoriesAsync();
            await InsertProductsAsync();
        }

        static readonly object syncLock = new object();
        static SqliteConnection _sqliteConnection;
        static DbContextOptions<TestDbContext> _testDbContextOptions;

        protected internal static DbContextOptions<TestDbContext> GetDbContextOptions()
        {
            if (_testDbContextOptions == null)
            {
                lock (syncLock)
                {
                    if (_testDbContextOptions == null)
                    {
                        _sqliteConnection = new SqliteConnection("DataSource=:memory:");
                        _sqliteConnection.Open();

                        var options = new DbContextOptionsBuilder<TestDbContext>()
                                .UseSqlite(_sqliteConnection)
                                .Options;

                        using (var context = new TestDbContext(options))
                        {
                            context.Database.EnsureCreated();
                        }

                        _testDbContextOptions = options;
                    }
                }
            }
            return _testDbContextOptions;
        }

        protected void AddCars(IRepository<Product> products, Category cars)
        {
            products.Add(new Product { Id = 1, Name = "Audi", Category = cars });
            products.Add(new Product { Id = 2, Name = "Porsche", Category = cars });
            products.Add(new Product { Id = 3, Name = "Volkswagen", Category = cars });
            products.Add(new Product { Id = 4, Name = "BMW", Category = cars });
            products.Add(new Product { Id = 5, Name = "Mercedes Benz", Category = cars });
            products.Add(new Product { Id = 6, Name = "Peugeot", Category = cars });
            products.Add(new Product { Id = 7, Name = "Citroën", Category = cars });
            products.Add(new Product { Id = 8, Name = "Toyota", Category = cars });
            products.Add(new Product { Id = 9, Name = "Suzuki", Category = cars });
            products.Add(new Product { Id = 10, Name = "Lamborghini", Category = cars });
            products.Add(new Product { Id = 11, Name = "Ferrari", Category = cars });
            products.Add(new Product { Id = 12, Name = "GMC", Category = cars });
        }

        protected void AddComputers(IRepository<Product> products, Category computers)
        {
            products.Add(new Product { Id = 13, Name = "HP", Category = computers });
            products.Add(new Product { Id = 14, Name = "Dell", Category = computers });
            products.Add(new Product { Id = 15, Name = "Toshiba", Category = computers });
            products.Add(new Product { Id = 16, Name = "Compaq", Category = computers });
            products.Add(new Product { Id = 17, Name = "MacBook Pro", Category = computers });
            products.Add(new Product { Id = 18, Name = "Google Chromebook", Category = computers });
            products.Add(new Product { Id = 19, Name = "Sony", Category = computers });
            products.Add(new Product { Id = 20, Name = "Asus", Category = computers });
            products.Add(new Product { Id = 21, Name = "Acer", Category = computers });
            products.Add(new Product { Id = 22, Name = "Surface Pro", Category = computers });
            products.Add(new Product { Id = 23, Name = "Samsung", Category = computers });
            products.Add(new Product { Id = 24, Name = "Lenovo", Category = computers });
        }

        #endregion
    }
}
