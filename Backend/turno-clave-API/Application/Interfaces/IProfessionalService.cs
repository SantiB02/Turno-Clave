using turno_clave_API.Application.DTOs.Professional;
using turno_clave_API.Common;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Application.Interfaces
{
    public interface IProfessionalService
    {
        Task<Professional> CreateAsync(Guid businessExternalId, CreateProfessionalDTO dto);
        Task<Result<List<ProfessionalDTO>>> GetByBusinessExternalIdAsync(Guid businessExternalId);
        Task<Professional?> GetByExternalIdAsync(Guid externalId);
        Task<List<Professional>> GetByExternalIdsAsync(List<Guid> externalIds);
        Task<Professional?> UpdateAsync(Guid externalId, UpdateProfessionalDTO dto);
        Task<Professional?> DeleteAsync(Guid businessExternalId, Guid externalId);
    }
}
