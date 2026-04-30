using turno_clave_API.Application.DTOs.Business;
using turno_clave_API.Common;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Application.Interfaces
{
    public interface IBusinessService
    {
        Task<Result<BusinessDTO>> CreateAsync(CreateBusinessDTO dto, Guid userExternalId);
        Task<BusinessDetailDTO?> GetByExternalIdAsync(Guid externalId);
        Task<IEnumerable<BusinessDetailDTO>> GetByUserExternalIdAsync(Guid userExternalId);
        Task<BusinessDTO?> UpdateAsync(UpdateBusinessDTO dto);
        Task<BusinessDTO?> DeleteAsync(Guid externalId);
        // Business availability methods
        Task<IEnumerable<BusinessAvailabilityDTO>> GetGlobalAvailabilityAsync(Guid businessExternalId);
        Task<BusinessAvailabilityDTO> CreateGlobalAvailabilityAsync(Guid businessExternalId, CreateBusinessAvailabilityDTO dto);
        Task<BusinessAvailabilityDTO?> UpdateGlobalAvailabilityAsync(BusinessAvailabilityDTO dto);
        Task<bool> DeleteGlobalAvailabilityAsync(Guid externalId);
    }
}
