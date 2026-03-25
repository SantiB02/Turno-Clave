using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Infrastructure.Repositories.Interfaces
{
    public interface IAvailabilityRepository
    {
        public Task<List<Availability>> GetAvailabilitiesAsync();
        public Task<Availability?> GetAvailabilityByExternalIdAsync(Guid externalId);
        public void AddAvailability(Availability availability);
        public void UpdateAvailability(Availability availability);
        public Task DeleteAvailabilityAsync(Guid availabilityId);
        public Task DeleteAvailabilityAsync(Availability availability);
        public Task<bool> IsAvailabilityTakenAsync(Professional professional, DayOfWeek dayOfWeek, TimeOnly startTime, TimeOnly endTime);
        public Task<bool> IsDayWorkDayAsync(Professional professional, DayOfWeek dayOfWeek);
        public Task SaveAsync();
    }
}
