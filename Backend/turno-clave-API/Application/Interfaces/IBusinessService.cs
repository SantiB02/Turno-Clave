using turno_clave_API.Application.DTOs.Business;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Application.Interfaces
{
    public interface IBusinessService
    {
        Task<Business> CreateAsync(CreateBusinessDTO dto);
        Task<Business?> GetByExternalIdAsync(Guid externalId);
        Task<Business?> UpdateAsync(UpdateBusinessDTO dto);
        Task<Business?> DeleteAsync(Guid externalId);
    }
}
