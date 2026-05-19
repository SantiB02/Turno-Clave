using turno_clave_API.Application.DTOs.Client;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Application.Interfaces
{
    public interface IClientService
    {
        Task<Client> CreateAsync(CreateClientDTO dto);
        Task<Client?> GetByExternalIdAsync(Guid externalId);

        /// <summary>
        /// Busca un cliente existente por email dentro de un negocio.
        /// Si no existe, retorna null.
        /// Usado para deduplicación en el flujo sin login.
        /// </summary>
        Task<Client?> GetByEmailAsync(int businessId, string email);

        // Task<Client?> UpdateAsync(UpdateClientDTO dto); // NOT SUITABLE FOR MVP. CLIENT ALWAYS ENTERS THEIR DATA WHEN BOOKING AN APPOINTMENT
        Task<Client?> DeleteAsync(Guid externalId);
    }
}
