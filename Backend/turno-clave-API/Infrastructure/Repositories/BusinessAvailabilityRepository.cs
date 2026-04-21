using Microsoft.EntityFrameworkCore;
using turno_clave_API.Domain.Entities;
using turno_clave_API.Infrastructure.Data;
using turno_clave_API.Infrastructure.Repositories.Interfaces;

namespace turno_clave_API.Infrastructure.Repositories
{
    public class BusinessAvailabilityRepository : IBusinessAvailabilityRepository
    {
        private readonly AppDbContext _context;

        public BusinessAvailabilityRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BusinessAvailability>> GetByBusinessExternalIdAsync(Guid businessExternalId)
        {
            return await _context.BusinessAvailabilities
                .Include(b => b.Business)
                .Where(b => b.Business != null && b.Business.ExternalId == businessExternalId)
                .ToListAsync();
        }

        public async Task<BusinessAvailability?> GetByExternalIdAsync(Guid externalId)
        {
            return await _context.BusinessAvailabilities
                .Include(b => b.Business)
                .FirstOrDefaultAsync(b => b.ExternalId == externalId);
        }

        public void Add(BusinessAvailability availability)
        {
            _context.BusinessAvailabilities.Add(availability);
        }

        public void Update(BusinessAvailability availability)
        {
            _context.BusinessAvailabilities.Update(availability);
        }

        public async Task DeleteAsync(Guid externalId)
        {
            var entity = await GetByExternalIdAsync(externalId) ?? throw new KeyNotFoundException($"BusinessAvailability {externalId} not found");
            _context.BusinessAvailabilities.Remove(entity);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
