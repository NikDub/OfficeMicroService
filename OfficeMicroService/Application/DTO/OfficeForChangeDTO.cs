using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OfficeMicroService.Application.DTO;

public class OfficeForChangeDto
{
    private const string NumberRegex = "[+]{1}[0-9]{12}";

    [BsonRepresentation(BsonType.ObjectId)]
    public string PhotoId { get; set; }

    [Required] [BsonRequired] public string Status { get; set; }

    [Required] [BsonRequired] public string City { get; set; }

    [Required] [BsonRequired] public string Street { get; set; }

    [Required] [BsonRequired] public string HouseNumber { get; set; }

    public string OfficeNumber { get; set; }

    [Required]
    [BsonRequired]
    [RegularExpression(NumberRegex, ErrorMessage = "Incorrect phone number")]
    public string RegistryPhoneNumber { get; set; }
}