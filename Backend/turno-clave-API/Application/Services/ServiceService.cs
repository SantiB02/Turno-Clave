using turno_clave_API.Application.DTOs.Service;
using turno_clave_API.Application.Interfaces;
using turno_clave_API.Common;
using turno_clave_API.Domain.Entities;
using turno_clave_API.Infrastructure.Repositories.Interfaces;

namespace turno_clave_API.Application.Services
{
    public class ServiceService : IServiceService
    {
        private readonly ILogger _logger;
        private readonly IServiceRepository _serviceRepository;
        private readonly IBusinessRepository _businessRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IProfessionalRepository _professionalRepository;

        public ServiceService(ILogger<ServiceService> logger, 
            IServiceRepository serviceRepository, 
            IBusinessRepository businessRepository, 
            ICurrentUserService currentUserService, 
            IProfessionalRepository professionalRepository)
        {
            _logger = logger;
            _serviceRepository = serviceRepository;
            _businessRepository = businessRepository;
            _currentUserService = currentUserService;
            _professionalRepository = professionalRepository;
        }

        public async Task<Result<Service>> CreateAsync(CreateServiceDTO dto)
        {
            Guid businessExternalId = await _currentUserService.GetActiveBusinessExternalIdAsync();

            Business? business = await _businessRepository.GetBusinessByExternalIdAsync(businessExternalId);
            if (business == null)
                return Result<Service>.Failure($"Business with ExternalId {businessExternalId} not found.");

            List<Professional> professionals = await _professionalRepository.GetProfessionalsByExternalIdsAsync(dto.ProfessionalExternalIds);

            Service service = new()
            {
                BusinessId = business.Id,
                Business = business,
                Name = dto.Name,
                Description = dto.Description ?? null,
                Price = dto.Price,
                DurationMinutes = dto.DurationMinutes,

                ProfessionalServices = professionals.Select(p => new Domain.Entities.ProfessionalService
                {
                    Professional = p,
                }).ToList(),
            };

            _serviceRepository.AddService(service);
            await _serviceRepository.SaveAsync();

            return Result<Service>.Success(service);
        }

        public async Task<Result<IEnumerable<Service>>> GetByBusinessExternalIdAsync(Guid businessExternalId)
        {
            IEnumerable<Service> services = await _serviceRepository.GetServicesByBusinessExternalIdAsync(businessExternalId);
            return Result<IEnumerable<Service>>.Success(services);
        }

        public async Task<Result<IEnumerable<Service>>> GetByUserExternalIdAsync(Guid userExternalId)
        {
            IEnumerable<Service> services = await _serviceRepository.GetServicesByUserExternalIdAsync(userExternalId);
            return Result<IEnumerable<Service>>.Success(services);
        }

        public async Task<Result<Service?>> GetByExternalIdAsync(Guid externalId)
        {
            Service? service = await _serviceRepository.GetServiceByExternalIdAsync(externalId);
            if (service == null)
                return Result<Service?>.Failure($"Service with ExternalId {externalId} not found.");
            return Result<Service?>.Success(service);
        }

        public async Task<Result<Service?>> UpdateAsync(Guid externalId, UpdateServiceDTO dto)
        {
            Service? service = await _serviceRepository.GetServiceByExternalIdAsync(externalId);
            if (service == null)
                return Result<Service?>.Failure($"Service with ExternalId {externalId} not found.");
            service.Name = dto.Name ?? service.Name;
            service.Description = dto.Description ?? null;
            service.Price = dto.Price ?? service.Price;
            service.DurationMinutes = dto.DurationMinutes ?? service.DurationMinutes;

            // Sync professionals
            if (dto.ProfessionalExternalIds != null)
            {

                // Remove old relations
                service.ProfessionalServices = service.ProfessionalServices
                    .Where(ps => dto.ProfessionalExternalIds.Contains(ps.Professional.ExternalId))
                    .ToList();

                // Existing ids after removal
                List<Guid> existingIds = service.ProfessionalServices
                    .Select(ps => ps.Professional.ExternalId)
                    .ToList();

                // Add new relations
                List<Guid> idsToAdd = dto.ProfessionalExternalIds
                    .Except(existingIds)
                    .ToList();

                foreach (Guid professionalExternalId in idsToAdd)
                {
                    Professional? professional =
                        await _professionalRepository.GetProfessionalByExternalIdAsync(professionalExternalId);

                    if (professional == null)
                        continue;

                    service.ProfessionalServices.Add(new Domain.Entities.ProfessionalService
                    {
                        Professional = professional,
                        Service = service
                    });
                }
            }


            _serviceRepository.UpdateService(service);
            await _serviceRepository.SaveAsync();
            return Result<Service?>.Success(service);
        }

        public async Task<Result<Service?>> DeleteAsync(Guid externalId)
        {
            Service? service = await _serviceRepository.GetServiceByExternalIdAsync(externalId);
            if (service != null)
            {
                service.IsActive = false;
                await _serviceRepository.SaveAsync();
            }
            else
            {
                return Result<Service?>.Failure($"Service with ExternalId {externalId} not found.");
            }

            return Result<Service?>.Success(service);
        }
    }
}
