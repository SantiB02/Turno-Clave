using turno_clave_API.Application.DTOs.Availability;
using turno_clave_API.Application.Interfaces;
using turno_clave_API.Common;
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

        public async Task<Result<Availability>> CreateAsync(CreateAvailabilityDTO dto)
        {
            Professional? professional = await _professionalService.GetByExternalIdAsync(dto.ProfessionalExternalId);
            if (professional == null)
                return Result<Availability>.Failure($"Professional with ExternalId {dto.ProfessionalExternalId} not found.");

            // Validate that the new availability does not overlap with existing availabilities for the same professional.
            bool isValid = await IsAvailabilityValidAsync(professional, dto.DayOfWeek, dto.StartTime, dto.EndTime);
            if (!isValid)
                return Result<Availability>.Failure("Time slot already taken or invalid start and end time"); // TODO: Improve error message

            Availability availability = new()
            {
                ProfessionalId = professional.Id,
                Professional = professional,
                DayOfWeek = dto.DayOfWeek,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime
            };
            _availabilityRepository.AddAvailability(availability);
            await _availabilityRepository.SaveAsync();

            return Result<Availability>.Success(availability);
        }

        public async Task<Availability?> GetByExternalIdAsync(Guid externalId)
        {
            Availability? availability = await _availabilityRepository.GetAvailabilityByExternalIdAsync(externalId);
            return availability;
        }

        public async Task<Result<Availability>> UpdateAsync(UpdateAvailabilityDTO dto)
        {
            Availability? availability = await _availabilityRepository.GetAvailabilityByExternalIdAsync(dto.ExternalId);
            if (availability == null)
                return Result<Availability>.Failure($"Availability with ExternalId {dto.ExternalId} not found.");

            availability.DayOfWeek = dto.DayOfWeek;
            availability.StartTime = dto.StartTime;
            availability.EndTime = dto.EndTime;

            _availabilityRepository.UpdateAvailability(availability);
            await _availabilityRepository.SaveAsync();
            return Result<Availability>.Success(availability);
        }

        public async Task<Result<Availability>> DeleteAsync(Guid externalId)
        {
            Availability? availability = await _availabilityRepository.GetAvailabilityByExternalIdAsync(externalId);
            if (availability == null)
            {
                return Result<Availability>.Failure($"Availability with ExternalId {externalId} not found.");
            }

            await _availabilityRepository.DeleteAvailabilityAsync(availability.ExternalId);
            await _availabilityRepository.SaveAsync();
            return Result<Availability>.Success(availability);
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
