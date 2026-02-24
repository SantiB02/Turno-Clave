using turno_clave_API.Application.DTOs.Availability;
using turno_clave_API.Application.Interfaces;
using turno_clave_API.Domain.Entities;
using turno_clave_API.Infrastructure.Repositories.Interfaces;

namespace turno_clave_API.Application.Services
{
    // TODO: Implement validations
    public class AvailabilityService : IAvailabilityService
    {
        private readonly ILogger _logger;
        private readonly IAvailabilityRepository _availabilityRepository;
        private readonly IProfessionalService _professionalService;

        public AvailabilityService(ILogger<AvailabilityService> logger, IAvailabilityRepository availabilityRepository, IProfessionalService professionalService)
        {
            _logger = logger;
            _availabilityRepository = availabilityRepository;
            _professionalService = professionalService;
        }

        public async Task<Availability> CreateAsync(CreateAvailabilityDTO dto)
        {
            Professional? professional = await _professionalService.GetByExternalIdAsync(dto.ProfessionalExternalId);
            if (professional == null)
                throw new KeyNotFoundException($"Professional with ExternalId {dto.ProfessionalExternalId} not found.");

            // TODO: Validate that the new availability does not overlap with existing availabilities for the same professional.

            Availability availability = new()
            {
                ProfessionalId = professional.Id,
                Professional = professional,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime
            };
            _availabilityRepository.AddAvailability(availability);
            await _availabilityRepository.SaveAsync();
            return availability;
        }

        public async Task<Availability?> GetByExternalIdAsync(Guid externalId)
        {
            Availability? availability = await _availabilityRepository.GetAvailabilityByExternalIdAsync(externalId);
            return availability;
        }

        public async Task<Availability?> UpdateAsync(UpdateAvailabilityDTO dto)
        {
            Availability? availability = await _availabilityRepository.GetAvailabilityByExternalIdAsync(dto.ExternalId);
            if (availability == null)
                throw new KeyNotFoundException($"Availability with ExternalId {dto.ExternalId} not found.");

            availability.DayOfWeek = dto.DayOfWeek;
            availability.StartTime = dto.StartTime;
            availability.EndTime = dto.EndTime;

            _availabilityRepository.UpdateAvailability(availability);
            await _availabilityRepository.SaveAsync();
            return availability;
        }

        public async Task<Availability?> DeleteAsync(Guid externalId)
        {
            Availability? availability = await _availabilityRepository.GetAvailabilityByExternalIdAsync(externalId);
            if (availability != null)
            {
                availability.IsActive = false;
                await _availabilityRepository.SaveAsync();
            } else
            {
                throw new KeyNotFoundException($"Availability with ExternalId {externalId} not found.");
            }

            return availability;
        }
    }
}
