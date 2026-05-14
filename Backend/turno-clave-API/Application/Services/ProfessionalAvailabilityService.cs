using turno_clave_API.Application.DTOs.Availability;
using turno_clave_API.Application.DTOs.ProfessionalAvailability;
using turno_clave_API.Application.Interfaces;
using turno_clave_API.Application.Validators;
using turno_clave_API.Common;
using turno_clave_API.Domain.Entities;
using turno_clave_API.Infrastructure.Repositories.Interfaces;

namespace turno_clave_API.Application.Services
{
    // TODO: Implement validations
    public class ProfessionalAvailabilityService : IProfessionalAvailabilityService
    {
        private readonly ILogger _logger;
        private readonly IProfessionalAvailabilityRepository _professionalAvailabilityRepository;
        private readonly IProfessionalService _professionalService;
        private readonly IBusinessService _businessService;

        public ProfessionalAvailabilityService(
            ILogger<ProfessionalAvailabilityService> logger, 
            IProfessionalAvailabilityRepository professionalAvailabilityRepository, 
            IProfessionalService professionalService, 
            IBusinessService businessService)
        {
            _logger = logger;
            _professionalAvailabilityRepository = professionalAvailabilityRepository;
            _professionalService = professionalService;
            _businessService = businessService;
        }

        public async Task<Result<ProfessionalAvailability>> CreateAsync(CreateProfessionalAvailabilityDTO dto)
        {
            Professional? professional = await _professionalService.GetByExternalIdAsync(dto.ProfessionalExternalId);
            if (professional == null)
                return Result<ProfessionalAvailability>.Failure($"Professional with ExternalId {dto.ProfessionalExternalId} not found.");

            // Validate that the new availability does not overlap with existing availabilities for the same professional.
            bool isValid = await IsAvailabilityValidAsync(professional, dto.DayOfWeek, dto.StartTime, dto.EndTime);
            if (!isValid)
                return Result<ProfessionalAvailability>.Failure("Time slot already taken or invalid start and end time"); // TODO: Improve error message

            ProfessionalAvailability availability = new()
            {
                ProfessionalId = professional.Id,
                Professional = professional,
                DayOfWeek = dto.DayOfWeek,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime
            };
            _professionalAvailabilityRepository.AddAvailability(availability);
            await _professionalAvailabilityRepository.SaveAsync();

            return Result<ProfessionalAvailability>.Success(availability);
        }

        public async Task<ProfessionalAvailability?> GetByExternalIdAsync(Guid externalId)
        {
            ProfessionalAvailability? availability = await _professionalAvailabilityRepository.GetAvailabilityByExternalIdAsync(externalId);
            return availability;
        }

        public async Task<Result<ProfessionalAvailability>> UpdateAsync(Guid externalId, UpdateProfessionalAvailabilityDTO dto)
        {
            ProfessionalAvailability? availability = await _professionalAvailabilityRepository.GetAvailabilityByExternalIdAsync(externalId);
            if (availability == null)
                return Result<ProfessionalAvailability>.Failure($"Availability with ExternalId {externalId} not found.");

            availability.DayOfWeek = dto.DayOfWeek;
            availability.StartTime = dto.StartTime;
            availability.EndTime = dto.EndTime;

            _professionalAvailabilityRepository.UpdateAvailability(availability);
            await _professionalAvailabilityRepository.SaveAsync();
            return Result<ProfessionalAvailability>.Success(availability);
        }

        public async Task<List<ProfessionalAvailabilityDTO>?> UpdateAvailabilitiesAsync(Guid professionalExternalId, UpdateProfessionalAvailabilitiesDTO dto)
        {
            Professional? professional = await _professionalService.GetByExternalIdAsync(professionalExternalId);

            if (professional == null)
                return null;

            List<AvailabilityRange> professionalAvailabilities = dto.Availabilities
                .Select(a => new AvailabilityRange
                {
                    DayOfWeek = a.DayOfWeek,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime
                })
                .ToList();

            if (AvailabilityValidator.HasOverlappingAvailabilities(
                professionalAvailabilities))
            {
                throw new BusinessException(
                    "Los horarios de un mismo día no pueden superponerse."
                );
            }

            List<AvailabilityRange> businessAvailabilities =
                professional.Business.BusinessAvailabilities
                    .Select(a => new AvailabilityRange
                    {
                        DayOfWeek = a.DayOfWeek,
                        StartTime = a.StartTime,
                        EndTime = a.EndTime
                    })
                    .ToList();

            bool valid = professionalAvailabilities.All(availability => 
                _businessService.IsAvailabilityWithinBusinessHours(availability, businessAvailabilities)
            );

            if (!valid)
            {
                throw new BusinessException(
                    "Los horarios del profesional deben estar dentro de los horarios del negocio."
                );
            }

            return await _professionalAvailabilityRepository.UpdateAvailabilitiesAsync(professional, dto);
        }

        public async Task<Result<ProfessionalAvailability>> DeleteAsync(Guid externalId)
        {
            ProfessionalAvailability? availability = await _professionalAvailabilityRepository.GetAvailabilityByExternalIdAsync(externalId);
            if (availability == null)
            {
                return Result<ProfessionalAvailability>.Failure($"Availability with ExternalId {externalId} not found.");
            }

            await _professionalAvailabilityRepository.DeleteAvailabilityAsync(availability.ExternalId);
            await _professionalAvailabilityRepository.SaveAsync();
            return Result<ProfessionalAvailability>.Success(availability);
        }

        public async Task<bool> IsAvailabilityValidAsync(Professional professional, DayOfWeek dayOfWeek, TimeOnly startTime, TimeOnly endTime)
        {
            bool isStartTimeCorrect = startTime < endTime;
            bool isAvailabilityTaken = await _professionalAvailabilityRepository.IsAvailabilityTakenAsync(professional, dayOfWeek, startTime, endTime);

            return isStartTimeCorrect && !isAvailabilityTaken;
        }

        public async Task<bool> IsDayWorkDayAsync(Professional professional, DayOfWeek dayOfWeek)
        {
            return await _professionalAvailabilityRepository.IsDayWorkDayAsync(professional, dayOfWeek);
        }
    }
}
