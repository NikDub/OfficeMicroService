using IdentityMicroService.Domain.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeMicroService.Application.Services;
using OfficeMicroService.Data.Enum;
using OfficeMicroService.Data.Models.DTO;
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

        [Authorize(Roles = nameof(UserRole.Receptionist))]
        [HttpPost]
        public async Task<IActionResult> Create(OfficeDTO model)
        {
            var office = await _officeServices.CreateAsync(model);
            Log.Information("Create new Office {0} {1}", office.Id, office.Address, office.RegistryPhoneNumber);
            return Ok(office);
        }

        [Authorize(Roles = nameof(UserRole.Receptionist))]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, OfficeDTO model)
        {
            var office = await _officeServices.UpdateAsync(id, model);
            Log.Information("Update Office {0} {1} {2}", office.Id, office.Address, office.RegistryPhoneNumber);
            return Ok(office);
        }

        /*
        [Authorize(Roles = "Receptionist")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _officeServices.RemoveAsync(id);
            return Ok();
        }
        */

        [Authorize(Roles = nameof(UserRole.Receptionist))]
        [HttpPut("{id}")]
        public async Task<IActionResult> MakeInactive(string id)
        {
            return Ok(await _officeServices.ChangeStatus(id, OfficeStatus.Inactive));
        }

        [Authorize(Roles = nameof(UserRole.Receptionist))]
        [HttpPut("{id}")]
        public async Task<IActionResult> MakeActive(string id)
        {
            return Ok(await _officeServices.ChangeStatus(id, OfficeStatus.Active));
        }
    }
}
