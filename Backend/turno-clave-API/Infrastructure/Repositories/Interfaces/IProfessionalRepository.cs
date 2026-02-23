using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Infrastructure.Repositories.Interfaces
{
    public interface IProfessionalRepository
    {
        public Task<List<Professional>> GetProfessionalsAsync();
        public Task<Professional?> GetProfessionalByExternalIdAsync(Guid externalId);
        public void AddProfessional(Professional professional);
        public void UpdateProfessional(Professional professional);
        public Task DeleteProfessionalAsync(Guid professionalId);
        public Task SaveAsync();
    }
}
