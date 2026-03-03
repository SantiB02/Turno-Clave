using turno_clave_API.Application.DTOs.Availability;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Application.Interfaces
{
    public interface IAvailabilityService
    {
        Task<turno_clave_API.Common.Result<Availability>> CreateAsync(CreateAvailabilityDTO dto);
        Task<Availability?> GetByExternalIdAsync(Guid externalId);
        Task<turno_clave_API.Common.Result<Availability>> UpdateAsync(UpdateAvailabilityDTO dto);
        Task<turno_clave_API.Common.Result<Availability>> DeleteAsync(Guid externalId);
        Task<bool> CheckIfAvailabilityIsTaken(Professional professional, CreateAvailabilityDTO dto);
    }
}
