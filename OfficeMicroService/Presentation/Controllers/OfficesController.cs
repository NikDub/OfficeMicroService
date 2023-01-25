using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeMicroService.Application.DTO;
using OfficeMicroService.Application.Services;
using OfficeMicroService.Data.Enum;
using Serilog;

namespace OfficeMicroService.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OfficesController : Controller
{
    private readonly IOfficeServices _officeServices;

    public OfficesController(IOfficeServices officeServices)
    {
        _officeServices = officeServices;
    }

    /// <summary>
    ///     Get All Offices
    /// </summary>
    /// <response code="200">Returns offices list</response>
    /// <response code="500">Operation wasn't succeeded</response>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        Log.Information("Method {0}", nameof(GetAll));
        return Ok(await _officeServices.GetAllAsync());
    }

    /// <summary>
    ///     Get Office Info by id
    /// </summary>
    /// <param name="id"></param>
    /// <response code="200">Returns office info</response>
    /// <response code="404">Office not found</response>
    /// <response code="500">Operation wasn't succeeded</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        Log.Information("Method {0} {1}", nameof(GetById), id);
        var office = await _officeServices.GetAsync(id);
        if (office == null)
        {
            Log.Information("Method {0}, Office {1} NotFound", nameof(GetById), id);
            return NotFound($"Can't find office by {id}");
        }

        return Ok(office);
    }

    /// <summary>
    ///     Create office for Receptionist
    /// </summary>
    /// <param name="model"></param>
    /// <response code="201">Returns created office</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="422">Incorrect model</response>
    /// <response code="500">Operation wasn't succeeded</response>
    [Authorize(Roles = nameof(UserRole.Receptionist))]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OfficeForCreateDto model)
    {
        Log.Information("Method {0} {1} {2} {3} {4} {5} {6} {7}", nameof(Create), model.City, model.Street,
            model.HouseNumber, model.RegistryPhoneNumber, model.Status, model.PhotoId, model.OfficeNumber);
        if (!ModelState.IsValid)
        {
            Log.Error("Method {0}, Model UnprocessableEntity with {1} {2} {3} {4} {5} {6} {7}", nameof(Create),
                model.City, model.Street, model.HouseNumber, model.RegistryPhoneNumber, model.Status, model.PhotoId,
                model.OfficeNumber);
            return UnprocessableEntity(model);
        }

        var office = await _officeServices.CreateAsync(model);
        if (office == null)
        {
            Log.Error("Method {0} can't created entity {1} {2} {3} {4} {5} {6} {7}", nameof(Create), model.City,
                model.Street, model.HouseNumber, model.RegistryPhoneNumber, model.Status, model.PhotoId,
                model.OfficeNumber);
            return BadRequest("Can't created entity");
        }

        return Created("", office);
    }

    /// <summary>
    ///     Update office for Receptionist
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <response code="204">Returns if office was updated</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Office not found</response>
    /// <response code="422">Incorrect model</response>
    /// <response code="500">Operation wasn't succeeded</response>
    [Authorize(Roles = nameof(UserRole.Receptionist))]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] OfficeForUpdateDto model)
    {
        Log.Information("Method {0} {1}", nameof(Update), id);
        if (!ModelState.IsValid)
        {
            Log.Error("Method {0}, Model UnprocessableEntity with {1} {2} {3} {4} {5} {6} {7}", nameof(Update),
                model.City, model.Street, model.HouseNumber, model.RegistryPhoneNumber, model.Status, model.PhotoId,
                model.OfficeNumber);
            return UnprocessableEntity(model);
        }

        var office = await _officeServices.UpdateAsync(id, model);
        if (office == null)
        {
            Log.Information("Method {0}, Office {1}  NotFound", nameof(Update), id);
            return NotFound($"Can't find office by {id}");
        }

        return NoContent();
    }

    /// <summary>
    ///     Update office status for Receptionist
    /// </summary>
    /// <param name="id"></param>
    /// <response code="204">Returns if office status was updated</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Office not found</response>
    /// <response code="500">Operation wasn't succeeded</response>
    [Authorize(Roles = nameof(UserRole.Receptionist))]
    [HttpPut("{id}/status")]
    public async Task<IActionResult> ChangeStatus(Guid id)
    {
        Log.Information("Method {0} {1}", nameof(ChangeStatus), id);
        await _officeServices.ChangeStatusAsync(id);
        return NoContent();
    }
}