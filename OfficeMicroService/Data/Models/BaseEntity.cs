using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace OfficeMicroService.Data.Models;

public class BaseEntity : IBaseEntity
{
    [BsonId(IdGenerator = typeof(GuidGenerator))]
    public Guid Id { get; set; }
}