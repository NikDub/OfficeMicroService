﻿using IdentityMicroService.Domain.Entities.Enums;
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

        /// <summary>
        /// Get All Offices
        /// </summary>
        /// <response code="200">Returns offices list</response>
        /// <response code="500">Operation wasn't succeeded</response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _officeServices.GetAsync());
        }

        /// <summary>
        /// Get Office Info by id
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Returns office info</response>
        /// <response code="404">Office not found</response>
        /// <response code="500">Operation wasn't succeeded</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var office = await _officeServices.GetAsync(id);
            if (office != null)
            {
                return Ok();
            }
            return NotFound();
        }

        /// <summary>
        /// Create office for Receptionist
        /// </summary>
        /// <param name="model"></param>
        /// <response code="200">Returns created office</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Operation wasn't succeeded</response>
        [HttpGet("{id}")]
        [Authorize(Roles = nameof(UserRole.Receptionist))]
        [HttpPost]
        public async Task<IActionResult> Create(OfficeDTO model)
        {
            var office = await _officeServices.CreateAsync(model);
            return Ok(office);
        }

        /// <summary>
        /// Update office for Receptionist
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <response code="200">Returns created office</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Office not found</response>
        /// <response code="500">Operation wasn't succeeded</response>
        [Authorize(Roles = nameof(UserRole.Receptionist))]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, OfficeDTO model)
        {
            var office = await _officeServices.UpdateAsync(id, model);
            if (office != null)
            {
                return Ok(office);
            }

            return NotFound();
        }

        /// <summary>
        /// Update office status to Inactive for Receptionist
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Returns updated office</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Office not found</response>
        /// <response code="500">Operation wasn't succeeded</response>
        [Authorize(Roles = nameof(UserRole.Receptionist))]
        [HttpPut("{id}")]
        public async Task<IActionResult> MakeInactive(string id)
        {
            return Ok(await _officeServices.ChangeStatus(id, OfficeStatus.Inactive));
        }

        /// <summary>
        /// Update office status to Active for Receptionist
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Returns updated office</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Office not found</response>
        /// <response code="500">Operation wasn't succeeded</response>
        [Authorize(Roles = nameof(UserRole.Receptionist))]
        [HttpPut("{id}")]
        public async Task<IActionResult> MakeActive(string id)
        {
            return Ok(await _officeServices.ChangeStatus(id, OfficeStatus.Active));
        }
    }
}
