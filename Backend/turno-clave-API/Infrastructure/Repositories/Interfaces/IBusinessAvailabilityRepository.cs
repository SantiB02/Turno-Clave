using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Infrastructure.Repositories.Interfaces
{
    public interface IBusinessAvailabilityRepository
    {
        Task<IEnumerable<BusinessAvailability>> GetByBusinessExternalIdAsync(Guid businessExternalId);
        Task<BusinessAvailability?> GetByExternalIdAsync(Guid externalId);
        void Add(BusinessAvailability availability);
        void Update(BusinessAvailability availability);
        Task DeleteAsync(Guid externalId);
        Task SaveAsync();
    }
}
