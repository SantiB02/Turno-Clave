using Microsoft.EntityFrameworkCore;
using turno_clave_API.Domain.Entities;
using turno_clave_API.Infrastructure.Data;
using turno_clave_API.Infrastructure.Repositories.Interfaces;

namespace turno_clave_API.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly AppDbContext _context;

        public ClientRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<Client>> GetClientsAsync()
        {
            return _context.Clients.ToListAsync();
        }

        public Task<Client?> GetClientByExternalIdAsync(Guid externalId)
        {
            return _context.Clients.Include(c => c.Business).FirstOrDefaultAsync(c => c.ExternalId == externalId);
        }

        public Task<Client?> GetClientByEmailAsync(int businessId, string email)
        {
            return _context.Clients
                .Include(c => c.Business)
                .FirstOrDefaultAsync(c => c.BusinessId == businessId && c.Email == email);
        }

        public void AddClient(Client client)
        {
            _context.Clients.Add(client);
        }

        public void UpdateClient(Client client)
        {
            _context.Clients.Update(client);
        }

        public async Task DeleteClientAsync(Guid externalId)
        {
            Client? client = await GetClientByExternalIdAsync(externalId) ?? throw new KeyNotFoundException($"Client with ExternalId {externalId} not found.");
            _context.Clients.Remove(client);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
