using turno_clave_API.Application.DTOs.Service;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Application.Interfaces
{
    public interface IServiceService
    {
        Task<Service> CreateAsync(CreateServiceDTO dto);
        Task<IEnumerable<Service>> GetByUserExternalIdAsync(Guid userExternalId);
        Task<Service?> GetByExternalIdAsync(Guid externalId);
        Task<Service?> UpdateAsync(UpdateServiceDTO dto);
        Task<Service?> DeleteAsync(Guid externalId);
    }
}
