using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Infrastructure.Repositories.Interfaces
{
    public interface IBusinessRepository
    {
        public Task<List<Business>> GetBusinessesAsync();
        public Task<Business?> GetBusinessByExternalIdAsync(Guid externalId);
        public Task AddBusinessAsync(Business business);
        public Task UpdateBusinessAsync(Business business);
        public Task DeleteBusinessAsync(Business business);
        public Task SaveAsync();
    }
}
