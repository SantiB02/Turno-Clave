using turno_clave_API.Application.DTOs.Availability;
using turno_clave_API.Common;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Application.Interfaces
{
    public interface IAvailabilityService
    {
        Task<Result<Availability>> CreateAsync(CreateAvailabilityDTO dto);
        Task<Availability?> GetByExternalIdAsync(Guid externalId);
        Task<Result<Availability>> UpdateAsync(UpdateAvailabilityDTO dto);
        Task<Result<Availability>> DeleteAsync(Guid externalId);
        Task<bool> IsAvailabilityValidAsync(Professional professional, DayOfWeek dayOfWeek, TimeOnly startTime, TimeOnly endTime);
        Task<bool> IsDayWorkDayAsync(Professional professional, DayOfWeek dayOfWeek);
    }
}
