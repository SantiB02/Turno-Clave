using turno_clave_API.Application.DTOs.ProfessionalAvailability;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Infrastructure.Repositories.Interfaces
{
    public interface IProfessionalAvailabilityRepository
    {
        public Task<List<ProfessionalAvailability>> GetAvailabilitiesAsync();
        public Task<ProfessionalAvailability?> GetAvailabilityByExternalIdAsync(Guid externalId);
        public void AddAvailability(ProfessionalAvailability availability);
        public void UpdateAvailability(ProfessionalAvailability availability);
        public Task<List<ProfessionalAvailabilityDTO>> UpdateAvailabilitiesAsync(Professional professional, UpdateProfessionalAvailabilitiesDTO availabilities);
        public Task DeleteAvailabilityAsync(Guid availabilityId);
        public Task DeleteAvailabilityAsync(ProfessionalAvailability availability);
        public Task<bool> IsAvailabilityTakenAsync(Professional professional, DayOfWeek dayOfWeek, TimeOnly startTime, TimeOnly endTime);
        public Task<bool> IsDayWorkDayAsync(Professional professional, DayOfWeek dayOfWeek);
        public Task SaveAsync();
    }
}
