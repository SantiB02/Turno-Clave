using turno_clave_API.Application.DTOs.Availability;
using turno_clave_API.Application.DTOs.ProfessionalAvailability;
using turno_clave_API.Common;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Application.Interfaces
{
    public interface IProfessionalAvailabilityService
    {
        Task<Result<ProfessionalAvailability>> CreateAsync(CreateProfessionalAvailabilityDTO dto);
        Task<ProfessionalAvailability?> GetByExternalIdAsync(Guid externalId);
        Task<Result<ProfessionalAvailability>> UpdateAsync(Guid externalId, UpdateProfessionalAvailabilityDTO dto);
        Task<List<ProfessionalAvailabilityDTO>?> UpdateAvailabilitiesAsync(Guid professionalExternalId, UpdateProfessionalAvailabilitiesDTO dto);
        Task<Result<ProfessionalAvailability>> DeleteAsync(Guid externalId);
        Task<bool> IsAvailabilityValidAsync(Professional professional, DayOfWeek dayOfWeek, TimeOnly startTime, TimeOnly endTime);
        Task<bool> IsDayWorkDayAsync(Professional professional, DayOfWeek dayOfWeek);
    }
}
