using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace OfficeMicroService.Application.DTO
{
    public class OfficeDTO
    {
        public string Id { get; set; }
        public string PhotoId { get; set; }
        public string Status { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string OfficeNumber { get; set; }
        public string RegistryPhoneNumber { get; set; }
    }
}
