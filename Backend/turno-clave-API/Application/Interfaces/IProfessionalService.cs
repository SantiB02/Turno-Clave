using turno_clave_API.Application.DTOs.Professional;
using turno_clave_API.Common;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Application.Interfaces
{
    public interface IProfessionalService
    {
        Task<Professional> CreateAsync(CreateProfessionalDTO dto);
        Task<Result<IEnumerable<Professional>>> GetByBusinessExternalIdAsync(Guid businessExternalId);
        Task<Professional?> GetByExternalIdAsync(Guid externalId);
        Task<Professional?> UpdateAsync(UpdateProfessionalDTO dto);
        Task<Professional?> DeleteAsync(Guid externalId);
    }
}
