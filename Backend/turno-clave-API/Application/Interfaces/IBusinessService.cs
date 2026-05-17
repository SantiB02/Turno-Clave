using turno_clave_API.Application.DTOs.Business;
using turno_clave_API.Application.DTOs.BusinessAvailability;
using turno_clave_API.Common;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Application.Interfaces
{
    public interface IBusinessService
    {
        Task<Result<MinimalBusinessDTO>> CreateAsync(CreateBusinessDTO dto, Guid userExternalId);
        Task<BusinessDetailDTO?> GetByExternalIdAsync(Guid externalId);
        Task<IEnumerable<BusinessDetailDTO>> GetByUserExternalIdAsync(Guid userExternalId);
        Task<Result<MinimalBusinessDTO?>> UpdateAsync(Guid externalId, UpdateBusinessDTO dto);
        Task<Result<bool>> UpdatePublicLinkStatusAsync(Guid externalId, bool PublicLinkStatus);
        Task<MinimalBusinessDTO?> DeleteAsync(Guid externalId);
        // Business availability methods
        Task<IEnumerable<BusinessAvailabilityDTO>> GetGlobalAvailabilityAsync(Guid businessExternalId);
        Task<BusinessAvailabilityDTO> CreateGlobalAvailabilityAsync(Guid businessExternalId, CreateBusinessAvailabilityDTO dto);
        Task<BusinessAvailabilityDTO?> UpdateGlobalAvailabilityAsync(Guid externalId, UpdateBusinessAvailabilityDTO dto);
        Task<List<BusinessAvailabilityDTO>?> UpdateGlobalAvailabilitiesAsync(Guid businessExternalId, UpdateBusinessAvailabilitiesDTO dto);
        Task<bool> DeleteGlobalAvailabilityAsync(Guid externalId);
        bool IsAvailabilityWithinBusinessHours(AvailabilityRange professionalAvailability, IEnumerable<AvailabilityRange> businessAvailabilities);

        // ----- Public methods -----
        Task<PublicBusinessDetailDTO?> GetPublicBySlugAsync(string slug);
    }
}
