using turno_clave_API.Application.DTOs.Availability;
using turno_clave_API.Application.Interfaces;
using turno_clave_API.Common;
using turno_clave_API.Domain.Entities;
using turno_clave_API.Infrastructure.Repositories.Interfaces;

namespace turno_clave_API.Application.Services
{
    // TODO: Implement validations
    public class ProfessionalAvailabilityService : IProfessionalAvailabilityService
    {
        private readonly ILogger _logger;
        private readonly IAvailabilityRepository _availabilityRepository;
        private readonly IProfessionalService _professionalService;

        public ProfessionalAvailabilityService(ILogger<ProfessionalAvailabilityService> logger, IAvailabilityRepository availabilityRepository, IProfessionalService professionalService)
        {
            _logger = logger;
            _availabilityRepository = availabilityRepository;
            _professionalService = professionalService;
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
            _availabilityRepository.AddAvailability(availability);
            await _availabilityRepository.SaveAsync();

            return Result<ProfessionalAvailability>.Success(availability);
        }

        public async Task<ProfessionalAvailability?> GetByExternalIdAsync(Guid externalId)
        {
            ProfessionalAvailability? availability = await _availabilityRepository.GetAvailabilityByExternalIdAsync(externalId);
            return availability;
        }

        public async Task<Result<ProfessionalAvailability>> UpdateAsync(UpdateProfessionalAvailabilityDTO dto)
        {
            ProfessionalAvailability? availability = await _availabilityRepository.GetAvailabilityByExternalIdAsync(dto.ExternalId);
            if (availability == null)
                return Result<ProfessionalAvailability>.Failure($"Availability with ExternalId {dto.ExternalId} not found.");

            availability.DayOfWeek = dto.DayOfWeek;
            availability.StartTime = dto.StartTime;
            availability.EndTime = dto.EndTime;

            _availabilityRepository.UpdateAvailability(availability);
            await _availabilityRepository.SaveAsync();
            return Result<ProfessionalAvailability>.Success(availability);
        }

        public async Task<Result<ProfessionalAvailability>> DeleteAsync(Guid externalId)
        {
            ProfessionalAvailability? availability = await _availabilityRepository.GetAvailabilityByExternalIdAsync(externalId);
            if (availability == null)
            {
                return Result<ProfessionalAvailability>.Failure($"Availability with ExternalId {externalId} not found.");
            }

            await _availabilityRepository.DeleteAvailabilityAsync(availability.ExternalId);
            await _availabilityRepository.SaveAsync();
            return Result<ProfessionalAvailability>.Success(availability);
        }

        public async Task<bool> IsAvailabilityValidAsync(Professional professional, DayOfWeek dayOfWeek, TimeOnly startTime, TimeOnly endTime)
        {
            bool isStartTimeCorrect = startTime < endTime;
            bool isAvailabilityTaken = await _availabilityRepository.IsAvailabilityTakenAsync(professional, dayOfWeek, startTime, endTime);

            return isStartTimeCorrect && !isAvailabilityTaken;
        }

        public async Task<bool> IsDayWorkDayAsync(Professional professional, DayOfWeek dayOfWeek)
        {
            return await _availabilityRepository.IsDayWorkDayAsync(professional, dayOfWeek);
        }
    }
}
