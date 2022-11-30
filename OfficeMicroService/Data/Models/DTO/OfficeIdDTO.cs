using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OfficeMicroService.Data.Models.DTO
{
    public class OfficeIdDTO : OfficeDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

    }
}
