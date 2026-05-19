using turno_clave_API.Application.DTOs.Client;
using turno_clave_API.Application.Interfaces;
using turno_clave_API.Domain.Entities;
using turno_clave_API.Infrastructure.Repositories.Interfaces;

namespace turno_clave_API.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly ILogger _logger;
        private readonly IClientRepository _clientRepository;
        private readonly IBusinessRepository _businessRepository;
        private readonly ICurrentUserService _currentUserService;

        public ClientService(ILogger<ClientService> logger, IClientRepository clientRepository, IBusinessRepository businessRepository, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _clientRepository = clientRepository;
            _businessRepository = businessRepository;
            _currentUserService = currentUserService;
        }

        public async Task<Client> CreateAsync(CreateClientDTO dto)
        {
            Business? business = await _businessRepository.GetBusinessByExternalIdAsync(dto.BusinessExternalId);
            if (business == null)
                throw new KeyNotFoundException($"Business with ExternalId {dto.BusinessExternalId} not found.");

            Client client = new()
            {
                ExternalId = Guid.NewGuid(),
                BusinessId = business.Id,
                Business = business,
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Notes = dto.Notes ?? null
            };

            _clientRepository.AddClient(client);
            await _clientRepository.SaveAsync();
            return client;
        }

        public async Task<Client?> GetByExternalIdAsync(Guid externalId)
        {
            Client? client = await _clientRepository.GetClientByExternalIdAsync(externalId);
            return client;
        }

        public async Task<Client?> GetByEmailAsync(int businessId, string email)
        {
            Client? client = await _clientRepository.GetClientByEmailAsync(businessId, email);
            return client;
        }

        // THIS SHOULD NOT BE INCLUDED IN MVP. A CLIENT'S PERSONAL DATA IS ONLY ASKED ONCE WHEN THEY BOOK AN APPOINTMENT

        //public async Task<Client?> UpdateAsync(UpdateClientDTO dto)
        //{
        //    Client? client = await _clientRepository.GetClientByExternalIdAsync(dto.ExternalId);
        //    if (client == null)
        //        throw new KeyNotFoundException($"Client with ExternalId {dto.ExternalId} not found.");
        //    client.Name = dto.Name;
        //    client.Email = dto.Email;
        //    client.Phone = dto.Phone;
        //    client.Notes = dto.Notes ?? null;
        //    _clientRepository.UpdateClient(client);
        //    await _clientRepository.SaveAsync();
        //    return client;
        //}

        public async Task<Client?> DeleteAsync(Guid externalId)
        {
            Client? client = await _clientRepository.GetClientByExternalIdAsync(externalId);
            if (client == null)
                throw new KeyNotFoundException($"Client with ExternalId {externalId} not found.");
            await _clientRepository.DeleteClientAsync(externalId);
            await _clientRepository.SaveAsync();
            return client;
        }
    }
}
