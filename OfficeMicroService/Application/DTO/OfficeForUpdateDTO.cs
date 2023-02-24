using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OfficeMicroService.Data.Enum;

namespace OfficeMicroService.Application.DTO;

public class OfficeForUpdateDto
{
    private const string NumberRegex = "[+]{1}[0-9]{12}";

    [BsonRepresentation(BsonType.ObjectId)]
    public Guid PhotoId { get; set; }

    [BsonRequired] public OfficeStatus Status { get; set; }

    [BsonRequired] public string City { get; set; }

    [BsonRequired] public string Street { get; set; }

    [BsonRequired] public string HouseNumber { get; set; }
    public List<double> Location { get; set; }

    public string OfficeNumber { get; set; }

    [BsonRequired]
    [RegularExpression(NumberRegex, ErrorMessage = "Incorrect phone number")]
    public string RegistryPhoneNumber { get; set; }
}