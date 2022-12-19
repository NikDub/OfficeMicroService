using AutoMapper;
using OfficeMicroService.Application.DTO;
using OfficeMicroService.Data.Models;

namespace OfficeMicroService.Data.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OfficeDTO, Office>().ReverseMap();
            CreateMap<OfficeForChangeDTO, Office>().ReverseMap();
            CreateMap<OfficeForUpdateDTO, Office>().ReverseMap();
            CreateMap<OfficeForUpdateDTO, OfficeDTO>().ReverseMap();
        }
    }
}
