using Microsoft.EntityFrameworkCore;
using turno_clave_API.Domain.Entities;
using turno_clave_API.Infrastructure.Data;
using turno_clave_API.Infrastructure.Repositories.Interfaces;

namespace turno_clave_API.Infrastructure.Repositories
{
    public class BusinessRepository : IBusinessRepository
    {
        private readonly AppDbContext _context;

        public BusinessRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<Business>> GetBusinessesAsync()
        {
            return _context.Businesses.ToListAsync();
        }

        public Task<Business?> GetBusinessByExternalIdAsync(Guid externalId)
        {
            return _context.Businesses
                .Include(b => b.BusinessAvailabilities)
                .FirstOrDefaultAsync(b => b.ExternalId == externalId);
        }

        public async Task<IEnumerable<Business>> GetBusinessesByUserExternalIdAsync(Guid userExternalId)
        {
            return await _context.UserBusinesses
                .Where(ub => ub.User.ExternalId == userExternalId)
                .Select(ub => ub.Business)
                .ToListAsync();
        }

        public async Task<bool> SlugExistsAsync(string slug)
        {
            return await _context.Businesses.AnyAsync(b => b.Slug == slug);
        }

        public void AddBusiness(Business business)
        {
            _context.Businesses.Add(business);
        }

        public void UpdateBusiness(Business business)
        {
            _context.Businesses.Update(business);
        }

        public async Task DeleteBusinessAsync(Guid externalId)
        {
            Business? business = await GetBusinessByExternalIdAsync(externalId) ?? throw new KeyNotFoundException($"Business with ExternalId {externalId} not found.");
            _context.Businesses.Remove(business);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
}
}
