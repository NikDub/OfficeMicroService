using MongoDB.Driver;
using OfficeMicroService.Application.Extensions;
using OfficeMicroService.Data.Models;
using OfficeMicroService.Data.Settings;
using System.Linq.Expressions;

namespace OfficeMicroService.Data.Repository.Impl;

public class OfficeRepository : IOfficeRepository
{
    private readonly IMongoCollection<Office> _collection;

    public OfficeRepository(IOfficeStoreDatabaseSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);

        _collection = database.GetCollection<Office>(GetCollectionName(typeof(Office)));
    }

    public async Task CreateAsync(Office entity)
    {
        await _collection.InsertOneAsync(entity);
    }

    public async Task<List<Office>> FindByConditionAsync(Expression<Func<Office, bool>> expression)
    {
        return (await _collection.FindAsync(expression)).ToList();
    }

    public async Task<List<Office>> FindAllAsync()
    {
        return (await _collection.FindAsync(r => true)).ToList();
    }

    public async Task UpdateAsync(Office entity)
    {
        await _collection.ReplaceOneAsync(x => x.Id.Equals(entity.Id), entity);
    }

    private protected string GetCollectionName(Type documentType)
    {
        return ((BsonCollectionAttribute)documentType.GetCustomAttributes(typeof(BsonCollectionAttribute), true)
            .FirstOrDefault())?.CollectionName;
    }
}