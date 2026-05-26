using turno_clave_API.Application.DTOs.Professional;
using turno_clave_API.Application.Interfaces;
using turno_clave_API.Common;
using turno_clave_API.Domain.Entities;
using turno_clave_API.Infrastructure.Repositories;
using turno_clave_API.Infrastructure.Repositories.Interfaces;

namespace turno_clave_API.Application.Services
{
    public class ProfessionalService : IProfessionalService
    {
        private readonly ILogger _logger;
        private readonly IProfessionalRepository _professionalRepository;
        private readonly IBusinessRepository _businessRepository;
        private readonly IServiceRepository _serviceRepository;

        public ProfessionalService(ILogger<ProfessionalService> logger, 
            IProfessionalRepository professionalRepository, 
            IBusinessRepository businessRepository, 
            IServiceRepository serviceRepository)
        {
            _logger = logger;
            _professionalRepository = professionalRepository;
            _businessRepository = businessRepository;
            _serviceRepository = serviceRepository;
        }

        public async Task<Professional> CreateAsync(Guid businessExternalId, CreateProfessionalDTO dto)
        {
            Business? business = await _businessRepository.GetBusinessByExternalIdAsync(businessExternalId);
            if (business == null)
                throw new KeyNotFoundException($"Business with ExternalId {businessExternalId} not found.");

            Professional professional = new()
            {
                BusinessId = business.Id,
                Business = business,
                Name = dto.Name,
                Availabilities = business.BusinessAvailabilities.Select(static a => new ProfessionalAvailability
                {
                    DayOfWeek = a.DayOfWeek,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                }).ToList()
            };

            await SyncProfessionalServicesAsync(professional, dto.ServiceExternalIds);

            _professionalRepository.AddProfessional(professional);
            await _professionalRepository.SaveAsync();

            return professional;
        }

        public async Task<Result<List<ProfessionalDTO>>> GetByBusinessExternalIdAsync(Guid businessExternalId)
        {
            List<ProfessionalDTO> professionals = await _professionalRepository.GetProfessionalDtosByBusinessExternalIdAsync(businessExternalId);
            return Result<List<ProfessionalDTO>>.Success(professionals);
        }

        public async Task<Professional?> GetByExternalIdAsync(Guid externalId)
        {
            Professional? professional = await _professionalRepository.GetProfessionalByExternalIdAsync(externalId);
            return professional;
        }

        public async Task<List<Professional>> GetByExternalIdsAsync(List<Guid> externalIds)
        {
            if (externalIds == null || externalIds.Count == 0)
                return new List<Professional>();

            List<Professional> professionals = await _professionalRepository.GetProfessionalsByExternalIdsAsync(externalIds);
            return professionals;
        }

        public async Task<Professional?> UpdateAsync(Guid externalId, UpdateProfessionalDTO dto)
        {
            Professional? professional = await _professionalRepository.GetProfessionalByExternalIdWithServicesAsync(externalId);
            if (professional == null)
                throw new KeyNotFoundException($"Professional with ExternalId {externalId} not found.");

            professional.Name = dto.Name;

            // Add and remove services based on incoming ones from DTO
            await SyncProfessionalServicesAsync(professional,dto.ServiceExternalIds);

            _professionalRepository.UpdateProfessional(professional);
            await _professionalRepository.SaveAsync();
            return professional;
        }

        public async Task<Professional?> DeleteAsync(Guid businessExternalId, Guid externalId)
        {
            Professional? professional =
                await _professionalRepository.GetProfessionalByExternalIdAsync(externalId);

            if (professional == null)
                return null;

            if (professional.Business.ExternalId != businessExternalId)
                throw new UnauthorizedAccessException();

            professional.IsActive = false;
            await _professionalRepository.SaveAsync();

            return professional;
        }

        private async Task SyncProfessionalServicesAsync(Professional professional, List<Guid> newServiceExternalIds
)
        {
            HashSet<Guid> newIds = newServiceExternalIds.ToHashSet();

            HashSet<Guid> currentIds = professional.ProfessionalServices
                .Select(ps => ps.Service.ExternalId)
                .ToHashSet();

            // Remove
            List<Domain.Entities.ProfessionalService> servicesToRemove =
                professional.ProfessionalServices
                    .Where(ps => !newIds.Contains(ps.Service.ExternalId))
                    .ToList();

            foreach (var ps in servicesToRemove)
            {
                professional.ProfessionalServices.Remove(ps);
            }

            // Add
            List<Guid> idsToAdd = newIds
                .Except(currentIds)
                .ToList();

            if (idsToAdd.Count == 0)
                return;

            List<Service> servicesToAdd = await _serviceRepository
                .GetServicesByExternalIdsAsync(idsToAdd);

            if (servicesToAdd.Count != idsToAdd.Count)
            {
                throw new Exception("One or more services do not exist.");
            }

            foreach (var service in servicesToAdd)
            {
                professional.ProfessionalServices.Add(new Domain.Entities.ProfessionalService
                {
                    ProfessionalId = professional.Id,
                    ServiceId = service.Id,
                    Service = service
                });
            }
        }
    }
}
