using AutoMapper;
using MongoDB.Driver;
using OfficeMicroService.Application.Models;
using OfficeMicroService.Models;
using OfficeMicroService.Settings;

namespace OfficeMicroService.Services
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
            return (await _Offices.FindAsync(Office => Office.Id == id)).FirstOrDefault();
        }

        public async Task<OfficeIdDTO> CreateAsync(OfficeDTO model)
        {
            var mapModel = _mapper.Map<OfficeIdDTO>(model);
            await _Offices.InsertOneAsync(mapModel);
            return mapModel;
        }

        public async Task UpdateAsync(string id, OfficeDTO model)
        {
            var mapModel = _mapper.Map<OfficeIdDTO>(model);
            mapModel.Id = id;
            await _Offices.ReplaceOneAsync(Office => Office.Id == id, mapModel);
        }

        public async Task RemoveAsync(string id)
        {
            await _Offices.DeleteOneAsync(Office => Office.Id == id);
        }
    }
}
