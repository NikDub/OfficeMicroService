using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace OfficeMicroService.Data.Models
{
    public class BaseEntity : IBaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
