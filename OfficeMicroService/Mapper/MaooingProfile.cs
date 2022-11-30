using AutoMapper;
using OfficeMicroService.Application.Models;
using OfficeMicroService.Models;

namespace OfficeMicroService.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OfficeDTO, OfficeIdDTO>().ReverseMap();
        }
    }
}
