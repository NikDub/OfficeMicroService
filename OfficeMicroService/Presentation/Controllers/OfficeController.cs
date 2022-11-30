using Microsoft.AspNetCore.Mvc;
using OfficeMicroService.Application.Services;
using OfficeMicroService.Data.Models.DTO;
using OfficeMicroService.Data.Models.Enum;
using Serilog;

namespace OfficeMicroService.Presentation.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class OfficeController : Controller
    {
        public IOfficeServices _officeServices { get; }

        public OfficeController(IOfficeServices officeServices)
        {
            _officeServices = officeServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _officeServices.GetAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await _officeServices.GetAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(OfficeDTO model)
        {
            var office = await _officeServices.CreateAsync(model);
            Log.Information("Create new Office {0} {1}", office.Id, office.Address, office.RegistryPhoneNumber);
            return Ok(office);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, OfficeDTO model)
        {
            var office = await _officeServices.UpdateAsync(id, model);
            Log.Information("Update Office {0} {1} {2}", office.Id, office.Address, office.RegistryPhoneNumber);
            return Ok(office);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _officeServices.RemoveAsync(id);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> MakeInactive(string id)
        {
            return Ok(await _officeServices.ChangeStatus(id, OfficeStatus.Inactive));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> MakeActive(string id)
        {
            return Ok(await _officeServices.ChangeStatus(id, OfficeStatus.Active));
        }
    }
}
