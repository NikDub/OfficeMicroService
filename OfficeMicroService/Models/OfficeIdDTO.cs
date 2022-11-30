using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using OfficeMicroService.Models;

namespace OfficeMicroService.Application.Models
{
    public class OfficeIdDTO : OfficeDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

    }
}
