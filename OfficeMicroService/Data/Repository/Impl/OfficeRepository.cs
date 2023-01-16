using System.Linq.Expressions;
using MongoDB.Driver;
using OfficeMicroService.Application.Extensions;
using OfficeMicroService.Data.Models;
using OfficeMicroService.Data.Settings;

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

    public async Task DeleteAsync(Expression<Func<Office, bool>> expression)
    {
        await _collection.FindOneAndDeleteAsync(expression);
    }

    public async Task<List<Office>> FindByCondition(Expression<Func<Office, bool>> expression)
    {
        return (await _collection.FindAsync(expression)).ToList();
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