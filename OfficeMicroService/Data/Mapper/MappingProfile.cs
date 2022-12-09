using AutoMapper;
using OfficeMicroService.Data.Models;
using OfficeMicroService.Data.Models.DTO;

namespace OfficeMicroService.Data.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OfficeDTO, Office>().ReverseMap();
        }
    }
}
