using AutoMapper;
using MongoDB.Driver;
using OfficeMicroService.Data.Enum;
using OfficeMicroService.Data.Models;
using OfficeMicroService.Data.Models.DTO;
using OfficeMicroService.Data.Settings;
using System.Data.Common;
using System.Runtime.InteropServices;

namespace OfficeMicroService.Application.Services
{
    public class OfficeServices : IOfficeServices
    {
        private readonly IMongoCollection<Office> _offices;
        private readonly IMapper _mapper;

        public OfficeServices(IOfficeStoreDatabaseSettings settings, IMapper mapper)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _offices = database.GetCollection<Office>(settings.OfficesCollectionName);
            _mapper = mapper;
        }

        public async Task<List<OfficeList>> GetAllAsync()
        {
            var officeList = (await _offices.FindAsync(office => true)).ToList();
            var mapModel = _mapper.Map<List<OfficeList>>(officeList);
            if (mapModel == null)
                return null;

            return mapModel;
        }

        public async Task<Office> GetAsync(string id)
        {
            return (await _offices.FindAsync(office => office.Id == id)).FirstOrDefault();
        }

        public async Task<Office> CreateAsync(OfficeDTO model)
        {
            var mapModel = _mapper.Map<Office>(model);
            if (mapModel == null)
                return null;

            await _offices.InsertOneAsync(mapModel);
            return mapModel;
        }

        public async Task<Office> UpdateAsync(string id, OfficeDTO model)
        {
            var mapModel = _mapper.Map<Office>(model);
            mapModel.Id = id;

            try
            {
                await _offices.ReplaceOneAsync(Office => Office.Id == id, mapModel);
                return mapModel;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task RemoveAsync(string id)
        {
            await _offices.DeleteOneAsync(office => office.Id == id);
        }

        public async Task<Office> ChangeStatus(string id)
        {

            var office = await GetAsync(id);
            if (office == null)
                return null;

            OfficeStatus status;
            if (Enum.TryParse(office.Status, out status))
            {
                status++;
                office.Status = status.ToString();
                office = await UpdateAsync(id, office);
                return office;
            }

            return null;
        }
    }
}
