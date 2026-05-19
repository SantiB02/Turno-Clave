using turno_clave_API.Application.DTOs.Appointment;
using turno_clave_API.Application.DTOs.Client;
using turno_clave_API.Application.Interfaces;
using turno_clave_API.Domain.Entities;
using turno_clave_API.Common;
using turno_clave_API.Infrastructure.Repositories.Interfaces;
using turno_clave_API.Domain.Enums;

namespace turno_clave_API.Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IBusinessRepository _businessRepository;
        private readonly IProfessionalService _professionalService;
        private readonly IClientService _clientService;
        private readonly IServiceService _serviceService;
        private readonly IProfessionalAvailabilityService _professionalAvailabilityService;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IBusinessRepository businessRepository,
            IProfessionalService professionalService,
            IClientService clientService,
            IServiceService serviceService,
            IProfessionalAvailabilityService professionalAvailabilityService)
        {
            _appointmentRepository = appointmentRepository;
            _businessRepository = businessRepository;
            _professionalService = professionalService;
            _clientService = clientService;
            _serviceService = serviceService;
            _professionalAvailabilityService = professionalAvailabilityService;
        }

        public async Task<Result<Appointment>> CreateAsync(CreateAppointmentDTO dto)
        {
            if (dto.StartDateTime >= dto.EndDateTime)
                return Result<Appointment>.Failure("StartDateTime must be earlier than EndDateTime.");

            if (dto.Items == null || dto.Items.Count == 0)
                return Result<Appointment>.Failure("Appointment must have at least one service item.");

            // Validate business
            Business? business = await _businessRepository.GetBusinessByExternalIdAsync(dto.BusinessExternalId);
            if (business == null)
                return Result<Appointment>.Failure($"Business with ExternalId {dto.BusinessExternalId} not found.");

            // Get or create client (deduplicación por email)
            Client? client = await _clientService.GetByEmailAsync(business.Id, dto.Client.Email);
            if (client == null)
            {
                // Cliente no existe, crear uno nuevo
                client = await _clientService.CreateAsync(new CreateClientDTO
                {
                    BusinessExternalId = dto.BusinessExternalId,
                    Name = dto.Client.Name,
                    Email = dto.Client.Email,
                    Phone = dto.Client.Phone,
                    Notes = dto.Client.Notes
                });
            }

            TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(business.TimeZone);
            DateTimeOffset startLocal = TimeZoneInfo.ConvertTime(dto.StartDateTime, tz);
            DateTimeOffset endLocal = TimeZoneInfo.ConvertTime(dto.EndDateTime, tz);
            DayOfWeek day = startLocal.DayOfWeek;

            // Validate and create items
            List<AppointmentItem> appointmentItems = new();

            foreach (var itemDto in dto.Items)
            {
                // Validate professional
                Professional? professional = await _professionalService.GetByExternalIdAsync(itemDto.ProfessionalExternalId);
                if (professional == null)
                    return Result<Appointment>.Failure($"Professional with ExternalId {itemDto.ProfessionalExternalId} not found.");

                // Validate service
                Result<Service?> serviceResult = await _serviceService.GetByExternalIdAsync(itemDto.ServiceExternalId);
                if (!serviceResult.IsSuccess || serviceResult.Value == null)
                    return Result<Appointment>.Failure($"Service with ExternalId {itemDto.ServiceExternalId} not found.");

                Service service = serviceResult.Value;

                // Check professional availability for this time block
                bool isAppointmentTaken = await _appointmentRepository.IsAppointmentTakenAsync(professional.Id, dto.StartDateTime, dto.EndDateTime);
                if (isAppointmentTaken)
                    return Result<Appointment>.Failure($"Professional {professional.Name} is not available at the requested time slot.");

                bool isDayWorkDay = await _professionalAvailabilityService.IsDayWorkDayAsync(professional, day);
                if (!isDayWorkDay)
                    return Result<Appointment>.Failure($"Professional {professional.Name} does not work on {day}.");

                // Create appointment item
                appointmentItems.Add(new AppointmentItem
                {
                    ServiceId = service.Id,
                    Service = service,
                    ProfessionalId = professional.Id,
                    Professional = professional,
                    StartDateTime = new DateTime(DateOnly.FromDateTime(dto.StartDateTime.DateTime), itemDto.StartTime).ToUniversalTime(),
                    EndDateTime = new DateTime(DateOnly.FromDateTime(dto.StartDateTime.DateTime), itemDto.EndTime).ToUniversalTime()
                });
            }

            // Create appointment with all items
            Appointment appointment = new()
            {
                ExternalId = Guid.NewGuid(),
                BusinessId = business.Id,
                Business = business,
                ClientId = client.Id,
                Client = client,
                StartDateTime = dto.StartDateTime,
                EndDateTime = dto.EndDateTime,
                Notes = dto.Notes,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow,
                Items = appointmentItems
            };

            _appointmentRepository.AddAppointment(appointment);
            await _appointmentRepository.SaveAsync();

            return Result<Appointment>.Success(appointment);
        }

        public async Task<Appointment?> GetByExternalIdAsync(Guid externalId)
        {
            Appointment? appointment = await _appointmentRepository.GetAppointmentByExternalIdAsync(externalId);
            return appointment;
        }

        // TODO: Implement UpdateAsync method and add validations (e.g. check if the new time slot is valid and not overlapping with other appointments for the same professional)
        //public async Task<Result<Appointment>> UpdateAsync(UpdateAppointmentDTO dto)
        //{
        //    Appointment? appointment = await _appointmentRepository.GetAppointmentByExternalIdAsync(dto.ExternalId);
        //    if (appointment == null)
        //        return Result<Appointment>.Failure($"Appointment with ExternalId {dto.ExternalId} not found.");
        //    appointment.StartDateTime = dto.StartDateTime;
        //    appointment.EndDateTime = dto.EndDateTime;
        //    appointment.Notes = dto.Notes;
        //    appointment.Status = dto.Status;
        //    appointment.UpdatedAt = DateTimeOffset.UtcNow;
        //    _appointmentRepository.UpdateAppointment(appointment);
        //    await _appointmentRepository.SaveAsync();
        //    return Result<Appointment>.Success(appointment);
        //}

        public async Task<Result<Appointment>> CancelAsync(Guid externalId)
        {
            Appointment? appointment = await _appointmentRepository.GetAppointmentByExternalIdAsync(externalId);
            if (appointment == null)
            {
                return Result<Appointment>.Failure($"Appointment with ExternalId {externalId} not found.");
            }
            appointment.Status = AppointmentStatus.Cancelled;
            await _appointmentRepository.SaveAsync();
            return Result<Appointment>.Success(appointment);
        }

        public async Task<Result<Appointment>> CancelAsync(Appointment appointment)
        {
            appointment.Status = AppointmentStatus.Cancelled;
            await _appointmentRepository.SaveAsync();
            return Result<Appointment>.Success(appointment);
        }
    }
}
