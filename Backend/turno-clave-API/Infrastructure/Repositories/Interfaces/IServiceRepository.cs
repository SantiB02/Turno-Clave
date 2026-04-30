using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Infrastructure.Repositories.Interfaces
{
    public interface IServiceRepository
    {
            public Task<List<Service>> GetServicesAsync();
            public Task<IEnumerable<Service>> GetServicesByUserExternalIdAsync(Guid userExternalId);
            public Task<Service?> GetServiceByExternalIdAsync(Guid externalId);
            public void AddService(Service service);
            public void UpdateService(Service service);
            public Task DeleteServiceAsync(Guid externalId);
            public Task SaveAsync();
    }
}
