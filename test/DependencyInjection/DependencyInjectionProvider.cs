using CoreRepository.Test.Data;
using Microsoft.Extensions.DependencyInjection;

namespace CoreRepository.Test.DependencyInjection
{
    public class DependencyInjectionProvider
    {
        ServiceProvider serviceProvider;

        private DependencyInjectionProvider()
        {
            Services = new ServiceCollection();
        }

        public ServiceCollection Services { get; }

        public DependencyInjectionProvider Build()
        {
            serviceProvider = Services.BuildServiceProvider();
            return this;
        }

        public T GetService<T>() => serviceProvider.GetService<T>();

        public IRepository<T> GetRepository<T>() where T : class  => serviceProvider.GetService<IRepository<T>>();

        public SqliteRepository<T> GetSqliteRepository<T>() where T : class 
            => (SqliteRepository<T>) serviceProvider.GetService<IRepository<T>>();

        public static DependencyInjectionProvider Create() => new DependencyInjectionProvider();
    }
}
