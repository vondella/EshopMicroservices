
using buildingBlock.Config;
using buildingBlock.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace buildingBlock.Repositories
{
    public class ItemRepository<T>: IItemRepository<T> where T:IEntity
    {
        private readonly MongoDB.Driver.IMongoCollection<T> dbCollection;
        private readonly FilterDefinitionBuilder<T> filterDefinitionBuilder = Builders<T>.Filter;

        public ItemRepository(IOptions<MongoConfig> config)
        {
            var mongoClient = new MongoClient(config.Value.ConnectionString);
            var database = mongoClient.GetDatabase(config.Value.Database);
            dbCollection = database.GetCollection<T>(config.Value.Collection);
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbCollection.Find(filterDefinitionBuilder.Empty).ToListAsync();
        }
        public async Task<T> GetAsync(Guid Id)
        {
            FilterDefinition<T> filter = filterDefinitionBuilder.Eq(entity => entity.Id, Id);
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }
        public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }
        public async Task CreateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await dbCollection.InsertOneAsync(entity);
        }
        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            FilterDefinition<T> filter = filterDefinitionBuilder.Eq(existingItem => existingItem.Id, entity.Id);
            await dbCollection.ReplaceOneAsync(filter, entity);
        }
        public async Task RemoveAsync(Guid Id)
        {
            FilterDefinition<T> filter = filterDefinitionBuilder.Eq(entity => entity.Id, Id);
            await dbCollection.DeleteOneAsync(filter);
        }

        public async  Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await dbCollection.Find(filter).ToListAsync();
        }
    }
}
