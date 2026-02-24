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

        public Task<List<Availability>> GetAvailabilitiesAsync()
        {
            return _context.Availabilities.ToListAsync();
        }

        public Task<Availability?> GetAvailabilityByExternalIdAsync(Guid externalId)
        {
            return _context.Availabilities.Include(av => av.Professional).FirstOrDefaultAsync(a => a.ExternalId == externalId);
        }

        public void AddAvailability(Availability availability)
        {
            _context.Availabilities.Add(availability);
        }

        public void UpdateAvailability(Availability availability)
        {
            _context.Availabilities.Update(availability);
        }

        public async Task DeleteAvailabilityAsync(Guid availabilityId)
        {
            Availability? availability = await _context.Availabilities.FirstOrDefaultAsync(a => a.ExternalId == availabilityId) ?? throw new KeyNotFoundException($"Availability with ExternalId {availabilityId} not found.");
            _context.Availabilities.Remove(availability);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
