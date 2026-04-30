using turno_clave_API.Application.DTOs.Service;
using turno_clave_API.Application.Interfaces;
using turno_clave_API.Domain.Entities;
using turno_clave_API.Infrastructure.Repositories.Interfaces;

namespace turno_clave_API.Application.Services
{
    public class ServiceService : IServiceService
    {
        private readonly ILogger _logger;
        private IServiceRepository _serviceRepository;
        private IBusinessRepository _businessRepository;

        public ServiceService(ILogger<ServiceService> logger, IServiceRepository serviceRepository, IBusinessRepository businessRepository)
        {
            _logger = logger;
            _serviceRepository = serviceRepository;
            _businessRepository = businessRepository;
        }

        public async Task<Service> CreateAsync(CreateServiceDTO dto)
        {
            Business? business = await _businessRepository.GetBusinessByExternalIdAsync(dto.BusinessExternalId);
            if (business == null)
                throw new KeyNotFoundException($"Business with ExternalId {dto.BusinessExternalId} not found.");

            Service service = new()
            {
                BusinessId = business.Id,
                Business = business,
                Name = dto.Name,
                Description = dto.Description ?? null,
                Price = dto.Price,
                DurationMinutes = dto.DurationMinutes
            };
            _serviceRepository.AddService(service);
            await _serviceRepository.SaveAsync();
            return service;
        }

        public async Task<IEnumerable<Service>> GetByUserExternalIdAsync(Guid userExternalId)
        {
            IEnumerable<Service> services = await _serviceRepository.GetServicesByUserExternalIdAsync(userExternalId);
            return services;
        }

        public async Task<Service?> GetByExternalIdAsync(Guid externalId)
        {
            Service? service = await _serviceRepository.GetServiceByExternalIdAsync(externalId);
            return service;
        }

        public async Task<Service?> UpdateAsync(UpdateServiceDTO dto)
        {
            Service? service = await _serviceRepository.GetServiceByExternalIdAsync(dto.ExternalId);
            if (service == null)
                throw new KeyNotFoundException($"Service with ExternalId {dto.ExternalId} not found.");
            service.Name = dto.Name ?? service.Name;
            service.Description = dto.Description ?? null;
            service.Price = dto.Price ?? service.Price;
            service.DurationMinutes = dto.DurationMinutes ?? service.DurationMinutes;

            _serviceRepository.UpdateService(service);
            await _serviceRepository.SaveAsync();
            return service;
        }

        public async Task<Service?> DeleteAsync(Guid externalId)
        {
            Service? service = await _serviceRepository.GetServiceByExternalIdAsync(externalId);
            if (service != null)
            {
                service.IsActive = false;
                await _serviceRepository.SaveAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Service with ExternalId {externalId} not found.");
            }

            return service;
        }
    }
}
