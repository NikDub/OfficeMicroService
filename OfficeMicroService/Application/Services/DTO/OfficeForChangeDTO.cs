using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace OfficeMicroService.Application.Services.DTO
{
    public class OfficeForChangeDTO
    {
        private const string NumberRegex = "[+]{1}[0-9]{12}";

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
        [RegularExpression(NumberRegex, ErrorMessage = "Incorrect phone number")]
        public string RegistryPhoneNumber { get; set; }
    }
}
