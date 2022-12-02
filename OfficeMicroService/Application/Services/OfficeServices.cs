using AutoMapper;
using MongoDB.Driver;
using OfficeMicroService.Data.Enum;
using OfficeMicroService.Data.Models.DTO;
using OfficeMicroService.Data.Settings;

namespace OfficeMicroService.Application.Services
{
    public class OfficeServices : IOfficeServices
    {
        private readonly IMongoCollection<OfficeIdDTO> _Offices;
        public IMapper _mapper { get; }

        public OfficeServices(IOfficeStoreDatabaseSettings settings, IMapper mapper)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _Offices = database.GetCollection<OfficeIdDTO>(settings.OfficesCollectionName);
            _mapper = mapper;
        }

        public async Task<List<OfficeIdDTO>> GetAsync()
        {
            return (await _Offices.FindAsync(Office => true)).ToList();
        }

        public async Task<OfficeIdDTO> GetAsync(string id)
        {
            try
            {
                return (await _Offices.FindAsync(Office => Office.Id == id)).FirstOrDefault();
            }
            catch(Exception)
            {
                return null;
            }
        }

        public async Task<OfficeIdDTO> CreateAsync(OfficeDTO model)
        {
            var mapModel = _mapper.Map<OfficeIdDTO>(model);
            await _Offices.InsertOneAsync(mapModel);
            return mapModel;
        }

        public async Task<OfficeIdDTO> UpdateAsync(string id, OfficeDTO model)
        {
            var mapModel = _mapper.Map<OfficeIdDTO>(model);
            mapModel.Id = id;

            try
            {
                await _Offices.ReplaceOneAsync(Office => Office.Id == id, mapModel);
                return mapModel;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task RemoveAsync(string id)
        {
            await _Offices.DeleteOneAsync(Office => Office.Id == id);
        }

        public async Task<OfficeIdDTO> ChangeStatus(string id, OfficeStatus status)
        {
            var office = await GetAsync(id);
            if (office != null)
            {
                office.Status = status.ToString();
                office = await UpdateAsync(id, office);
            }
            return office;

        }
    }
}
