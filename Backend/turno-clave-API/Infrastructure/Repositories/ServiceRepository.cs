using Microsoft.EntityFrameworkCore;
using turno_clave_API.Domain.Entities;
using turno_clave_API.Infrastructure.Data;
using turno_clave_API.Infrastructure.Repositories.Interfaces;

namespace turno_clave_API.Infrastructure.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly AppDbContext _context;
        public ServiceRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<Service>> GetServicesAsync()
        {
            return _context.Services.Include(s => s.Business).ToListAsync();
        }

        public Task<IEnumerable<Service>> GetServicesByUserExternalIdAsync(Guid userExternalId)
        {
            return _context.Services
                .Include(s => s.Business)
                .Where(s => s.Business.UserBusinesses.Any(ub => ub.User.ExternalId == userExternalId))
                .ToListAsync()
                .ContinueWith(t => t.Result.AsEnumerable());
        }

        public Task<Service?> GetServiceByExternalIdAsync(Guid externalId)
        {
            return _context.Services.Include(s => s.Business).FirstOrDefaultAsync(s => s.ExternalId == externalId);
        }

        public void AddService(Service service)
        {
            _context.Services.Add(service);
        }

        public void UpdateService(Service service)
        {
            _context.Services.Update(service);
        }

        public async Task DeleteServiceAsync(Guid externalId)
        {
            Service? service = await GetServiceByExternalIdAsync(externalId) ?? throw new KeyNotFoundException($"Service with ExternalId {externalId} not found.");
            _context.Services.Remove(service);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
