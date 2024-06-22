
namespace posQuery.Infrastracture.DataAccess
{
    public class DatabaseContextFactory
    {
        private readonly Action<DbContextOptionsBuilder> _configureDbContex;
        public DatabaseContextFactory(Action<DbContextOptionsBuilder> configureDbContext)
        {
            _configureDbContex = configureDbContext;
        }
        public DatabaseContext CreateDbContext()
        {
            DbContextOptionsBuilder<DatabaseContext> options = new DbContextOptionsBuilder<DatabaseContext>();
            _configureDbContex(options);
            return new DatabaseContext(options.Options);
        }
    }
}
