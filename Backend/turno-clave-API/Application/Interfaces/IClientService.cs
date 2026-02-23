using turno_clave_API.Application.DTOs.Client;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Application.Interfaces
{
    public interface IClientService
    {
        Task<Client> CreateAsync(CreateClientDTO dto);
        Task<Client?> GetByExternalIdAsync(Guid externalId);
        // Task<Client?> UpdateAsync(UpdateClientDTO dto); // NOT SUITABLE FOR MVP. CLIENT ALWAYS ENTERS THEIR DATA WHEN BOOKING AN APPOINTMENT
        Task<Client?> DeleteAsync(Guid externalId);
    }
}
