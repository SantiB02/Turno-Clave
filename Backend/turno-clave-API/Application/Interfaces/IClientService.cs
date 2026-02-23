using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Application.Interfaces
{
    public interface IClientService
    {
        Task<Client> CreateAsync(CreateClientDTO dto);
        Task<Client?> GetByExternalIdAsync(Guid externalId);
        Task<Client?> UpdateAsync(UpdateClientDTO dto);
        Task<Business?> DeleteAsync(Guid externalId);
    }
}
