using turno_clave_API.Application.DTOs.Service;
using turno_clave_API.Common;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Application.Interfaces
{
    public interface IServiceService
    {
        Task<Result<Service>> CreateAsync(CreateServiceDTO dto);
        Task<Result<IEnumerable<Service>>> GetByBusinessExternalIdAsync(Guid businessExternalId);
        Task<Result<IEnumerable<Service>>> GetByUserExternalIdAsync(Guid userExternalId);
        Task<Result<Service?>> GetByExternalIdAsync(Guid externalId);
        Task<Result<List<Service>>> GetByExternalIdsAsync(List<Guid> externalIds);
        Task<Result<Service?>> UpdateAsync(Guid externalId, UpdateServiceDTO dto);
        Task<Result<Service?>> DeleteAsync(Guid externalId);
    }
}
