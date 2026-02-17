using turno_clave_API.Application.DTOs.Business;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Application.Interfaces
{
    public interface IBusinessService
    {
        Task<Business> CreateAsync(CreateBusinessDto dto);
        Task<Business?> GetByExternalId(Guid externalId);
        Task<Business?> UpdateAsync(Guid externalId, UpdateBusinessDto dto);
        Task<Business?> DeleteAsync(Guid externalId);
    }
}
