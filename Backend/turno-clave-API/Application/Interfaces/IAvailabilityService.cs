using turno_clave_API.Application.DTOs.Availability;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Application.Interfaces
{
    public interface IAvailabilityService
    {
        Task<Availability> CreateAsync(CreateAvailabilityDTO dto);
        Task<Availability?> GetByExternalIdAsync(Guid externalId);
        Task<Availability?> UpdateAsync(UpdateAvailabilityDTO dto);
        Task<Availability?> DeleteAsync(Guid externalId);
    }
}
