using AutoMapper;
using MongoDB.Driver;
using OfficeMicroService.Application.DTO;
using OfficeMicroService.Application.Exceptions;
using OfficeMicroService.Data.Enum;
using OfficeMicroService.Data.Models;
using OfficeMicroService.Data.Repository;

namespace OfficeMicroService.Application.Services
{
    public class OfficeServices : IOfficeServices
    {
        private readonly IOfficeRepository _repository;
        private readonly IMapper _mapper;

        public OfficeServices(IOfficeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<OfficeDTO>> GetAllAsync()
        {
            var offices = (await _repository.FindByCondition(office => true));
            return _mapper.Map<List<OfficeDTO>>(offices);
        }
        public async Task<OfficeDTO> GetAsync(string id)
        {
            var office = (await _repository.FindByCondition(office => office.Id == id)).FirstOrDefault();
            return _mapper.Map<OfficeDTO>(office);
        }
        public async Task<OfficeDTO> CreateAsync(OfficeForChangeDTO model)
        {
            if (model == null)
                return null;
            var mapModel = _mapper.Map<Office>(model);

            await _repository.CreateAsync(mapModel);

            return _mapper.Map<OfficeDTO>(mapModel);
        }

        public async Task<OfficeDTO> UpdateAsync(string id, OfficeForUpdateDTO model)
        {
            var office = GetAsync(id);
            if (office == null && model == null)
                return null;

            var mapModel = _mapper.Map<Office>(model);
            mapModel.Id = id;

            await _repository.UpdateAsync(mapModel);
            return _mapper.Map<OfficeDTO>(mapModel);
        }

        public async Task RemoveAsync(string id)
        {
            await _repository.DeleteAsync(office => office.Id == id);
        }

        public async Task<OfficeDTO> ChangeStatusAsync(string id)
        {
            var office = await GetAsync(id);
            if (office == null)
                throw new NotFoundException("Office not found.");

            OfficeStatus status;
            if (Enum.TryParse(office.Status, out status))
            {
                office.Status = (status == OfficeStatus.Active ? OfficeStatus.Inactive : OfficeStatus.Active).ToString();

                var mapModel = _mapper.Map<Office>(office);
                await _repository.UpdateAsync(mapModel);
                return _mapper.Map<OfficeDTO>(mapModel);
            }

            return null;
        }
    }
}
