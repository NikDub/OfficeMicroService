using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using OfficeMicroService.Application.Extensions;
using OfficeMicroService.Data.Enum;

namespace OfficeMicroService.Data.Models;

[BsonCollection("Office")]
public class Office : BaseEntity
{
    public Guid PhotoId { get; set; }
    [BsonRepresentation(BsonType.String)]
    public OfficeStatus Status { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string HouseNumber { get; set; }
    public string OfficeNumber { get; set; }
    public string RegistryPhoneNumber { get; set; }
}