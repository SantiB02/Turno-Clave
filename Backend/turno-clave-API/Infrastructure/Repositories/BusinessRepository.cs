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
            return Task.FromResult(_context.Businesses.FirstOrDefault(b => b.ExternalId == externalId));
        }

        public Task AddBusinessAsync(Business business)
        {
            _context.Businesses.Add(business);
            return Task.CompletedTask;
        }

        public Task UpdateBusinessAsync(Business business)
        {
            _context.Businesses.Update(business);
            return Task.CompletedTask;
        }

        public Task DeleteBusinessAsync(Business business)
        {
            _context.Businesses.Remove(business);
            return Task.CompletedTask;
        }

        public Task SaveAsync()
        {
            return _context.SaveChangesAsync();
        }
}
}
