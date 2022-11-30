using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OfficeMicroService.Application.Models;
using OfficeMicroService.Models;
using OfficeMicroService.Services;

namespace OfficeMicroService.Controllers
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
            return Ok(await _officeServices.CreateAsync(model));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, OfficeDTO model)
        {
            await _officeServices.UpdateAsync(id, model);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _officeServices.RemoveAsync(id);
            return Ok();
        }

    }
}
