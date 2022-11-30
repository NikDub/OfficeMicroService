﻿using OfficeMicroService.Data.Models.DTO;
using OfficeMicroService.Data.Models.Enum;

namespace OfficeMicroService.Application.Services
{
    public interface IOfficeServices
    {
        Task<List<OfficeIdDTO>> GetAsync();
        Task<OfficeIdDTO> GetAsync(string id);

        Task<OfficeIdDTO> CreateAsync(OfficeDTO Office);

        Task<OfficeIdDTO> UpdateAsync(string id, OfficeDTO OfficeIn);

        Task RemoveAsync(string id);
        Task<OfficeIdDTO> ChangeStatus(string id, OfficeStatus status);
    }
}
