using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Infrastructure.Repositories.Interfaces
{
    public interface IClientRepository
    {
        public Task<List<Client>> GetClientsAsync();
        public Task<Client?> GetClientByExternalIdAsync(Guid externalId);
        public Task<Client?> GetClientByEmailAsync(int businessId, string email);
        public void AddClient(Client client);
        public void UpdateClient(Client client);
        public Task DeleteClientAsync(Guid externalId);
        public Task SaveAsync();
    }
}
