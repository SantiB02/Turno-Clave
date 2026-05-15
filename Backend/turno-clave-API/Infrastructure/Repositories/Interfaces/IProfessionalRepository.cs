using turno_clave_API.Application.DTOs.Professional;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Infrastructure.Repositories.Interfaces
{
    public interface IProfessionalRepository
    {
        public Task<List<Professional>> GetProfessionalsAsync();
        public Task<List<Professional>> GetProfessionalsByBusinessExternalIdAsync(Guid businessExternalId);
        public Task<List<ProfessionalDTO>> GetProfessionalDtosByBusinessExternalIdAsync(Guid businessExternalId);
        public Task<Professional?> GetProfessionalByExternalIdAsync(Guid externalId);
        public Task<Professional?> GetProfessionalByExternalIdWithServicesAsync(Guid externalId);
        public Task<List<Professional>> GetProfessionalsByExternalIdsAsync(List<Guid> externalIds);
        public void AddProfessional(Professional professional);
        public void UpdateProfessional(Professional professional);
        public Task DeleteProfessionalAsync(Guid professionalId);
        public Task SaveAsync();
    }
}
