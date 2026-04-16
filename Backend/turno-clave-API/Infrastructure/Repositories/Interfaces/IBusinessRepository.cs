using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Infrastructure.Repositories.Interfaces
{
    public interface IBusinessRepository
    {
        public Task<List<Business>> GetBusinessesAsync();
        public Task<Business?> GetBusinessByExternalIdAsync(Guid externalId);
        public Task<IEnumerable<Business>> GetBusinessesByUserExternalIdAsync(Guid userExternalId);
        public Task<bool> SlugExistsAsync(string slug);
        public void AddBusiness(Business business);
        public void UpdateBusiness(Business business);
        public Task DeleteBusinessAsync(Guid externalId);
        public Task SaveAsync();
    }
}
