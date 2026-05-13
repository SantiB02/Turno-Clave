using Microsoft.EntityFrameworkCore;
using turno_clave_API.Domain.Entities;
using turno_clave_API.Infrastructure.Data;
using turno_clave_API.Infrastructure.Repositories.Interfaces;

namespace turno_clave_API.Infrastructure.Repositories
{
    public class AvailabilityRepository : IAvailabilityRepository
    {
        private readonly AppDbContext _context;

        public AvailabilityRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<ProfessionalAvailability>> GetAvailabilitiesAsync()
        {
            return _context.ProfessionalAvailabilities.ToListAsync();
        }

        public Task<ProfessionalAvailability?> GetAvailabilityByExternalIdAsync(Guid externalId)
        {
            return _context.ProfessionalAvailabilities.Include(av => av.Professional).FirstOrDefaultAsync(a => a.ExternalId == externalId);
        }

        public void AddAvailability(ProfessionalAvailability availability)
        {
            _context.ProfessionalAvailabilities.Add(availability);
        }

        public void UpdateAvailability(ProfessionalAvailability availability)
        {
            _context.ProfessionalAvailabilities.Update(availability);
        }

        public async Task DeleteAvailabilityAsync(Guid availabilityId)
        {
            ProfessionalAvailability? availability = await _context.ProfessionalAvailabilities.FirstOrDefaultAsync(a => a.ExternalId == availabilityId) ?? throw new KeyNotFoundException($"Availability with ExternalId {availabilityId} not found.");
            _context.ProfessionalAvailabilities.Remove(availability);
        }

        public async Task DeleteAvailabilityAsync(ProfessionalAvailability availability)
        {
            _context.ProfessionalAvailabilities.Remove(availability);
        }

        public async Task<bool> IsAvailabilityTakenAsync(Professional professional, DayOfWeek dayOfWeek, TimeOnly startTime, TimeOnly endTime)
        {
            return await _context.ProfessionalAvailabilities
                .Where(av => av.ProfessionalId == professional.Id)
                .AnyAsync(a =>
                    a.DayOfWeek == dayOfWeek &&
                    startTime < a.EndTime &&
                    endTime > a.StartTime
                );
        }

        public async Task<bool> IsDayWorkDayAsync(Professional professional, DayOfWeek dayOfWeek)
        {
            return await _context.ProfessionalAvailabilities
                .Where(av => av.ProfessionalId == professional.Id)
                .AnyAsync(a => a.DayOfWeek == dayOfWeek);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
