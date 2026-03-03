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
        public Task<bool> IsAvailabilityTaken(Professional professional, DayOfWeek dayOfWeek, TimeOnly startTime, TimeOnly endTime);
        public Task SaveAsync();
    }
}
