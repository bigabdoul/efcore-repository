namespace CoreRepository.Test.Data
{
    public class TestUnitOfWork : UnitOfWork
    {
        public TestUnitOfWork(ISqliteDbAccessOptions options) : base(options)
        {
        }

        protected override IRepository<T> CreateRepository<T>()
        {
            lock (SyncRoot)
            {
                var repo = new SqliteRepository<T>((ISqliteDbAccessOptions)Options);
                if (Context == null)
                {
                    Context = repo.Context;
                }
                return repo;
            }
        }
    }
}
