using OfficeMicroService.Data.Enum;

namespace OfficeMicroService.Application.DTO;

public class OfficeDto
{
    public Guid Id { get; set; }
    public Guid PhotoId { get; set; }
    public OfficeStatus Status { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string HouseNumber { get; set; }
    public string OfficeNumber { get; set; }
    public string RegistryPhoneNumber { get; set; }
    public List<double> Location { get; set; }
}