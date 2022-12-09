using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OfficeMicroService.Data.Models.DTO;

namespace OfficeMicroService.Data.Models
{
    public class Office : OfficeDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

    }
}
