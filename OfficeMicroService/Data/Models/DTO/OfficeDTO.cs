using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace OfficeMicroService.Data.Models.DTO
{
    public class OfficeDTO
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string PhotoId { get; set; }
        [BsonRequired]
        public string Status { get; set; }
        [BsonRequired]
        public string City { get; set; }
        [BsonRequired]
        public string Street { get; set; }
        [BsonRequired]
        public string HouseNumber { get; set; }
        public string OfficeNumber { get; set; }
        [BsonRequired]
        public string RegistryPhoneNumber { get; set; }
    }
}
