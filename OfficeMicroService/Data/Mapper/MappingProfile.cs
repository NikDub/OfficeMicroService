using AutoMapper;
using OfficeMicroService.Application.DTO;
using OfficeMicroService.Data.Models;

namespace OfficeMicroService.Data.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<OfficeDto, Office>().ReverseMap();
        CreateMap<OfficeForCreateDto, Office>().ReverseMap();
        CreateMap<OfficeForUpdateDto, Office>().ReverseMap();
        CreateMap<OfficeForUpdateDto, OfficeDto>().ReverseMap();
    }
}