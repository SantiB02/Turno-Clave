using turno_clave_API.Application.DTOs.Appointment;
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
        private readonly IBusinessService _businessService;
        private readonly IProfessionalService _professionalService;
        private readonly IClientService _clientService;
        private readonly IServiceService _serviceService;
        private readonly IAvailabilityService _availabilityService;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IBusinessService businessService,
            IProfessionalService professionalService,
            IClientService clientService,
            IServiceService serviceService,
            IAvailabilityService availabilityService)
        {
            _appointmentRepository = appointmentRepository;
            _businessService = businessService;
            _professionalService = professionalService;
            _clientService = clientService;
            _serviceService = serviceService;
            _availabilityService = availabilityService;
        }

        public async Task<Result<Appointment>> CreateAsync(CreateAppointmentDTO dto)
        {
            if (dto.StartDateTime >= dto.EndDateTime)
                return Result<Appointment>.Failure("StartDateTime must be earlier than EndDateTime.");

            Business? business = await _businessService.GetByExternalIdAsync(dto.BusinessExternalId);
            if (business == null)
                return Result<Appointment>.Failure($"Business with ExternalId {dto.BusinessExternalId} not found.");

            Professional? professional = await _professionalService.GetByExternalIdAsync(dto.ProfessionalExternalId);
            if (professional == null)
                return Result<Appointment>.Failure($"Professional with ExternalId {dto.ProfessionalExternalId} not found.");

            // TODO: Check availability of the professional for the given time slot (e.g. check if there are no overlapping appointments and if the time slot is within the professional's working hours)
            // dto.StartDateTime.Deconstruct(out _, out TimeOnly startTime, out _);
            // dto.EndDateTime.Deconstruct(out _, out TimeOnly endTime, out _);

            TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(business.TimeZone);
            DateTimeOffset startLocal = TimeZoneInfo.ConvertTime(dto.StartDateTime, tz);
            TimeOnly startTime = TimeOnly.FromTimeSpan(startLocal.TimeOfDay);

            DateTimeOffset endLocal = TimeZoneInfo.ConvertTime(dto.EndDateTime, tz);
            TimeOnly endTime = TimeOnly.FromTimeSpan(endLocal.TimeOfDay);

            DayOfWeek day = startLocal.DayOfWeek;

            bool isAppointmentTaken = await _appointmentRepository.IsAppointmentTakenAsync(professional.Id, dto.StartDateTime, dto.EndDateTime);

            // TODO: fix bug down below
            // BUG: If the professional isn't available at the appointment's time range, the appointment will still be created.
            if (isAppointmentTaken)
                return Result<Appointment>.Failure($"The professional is not available on {day} from {startTime} to {endTime}."); // TODO: improve error message to specify if the issue is with the day, time range, or both

            bool isDayWorkDay = await _availabilityService.IsDayWorkDayAsync(professional, day);
            if (!isDayWorkDay)
                return Result<Appointment>.Failure($"The professional does not work on {day}.");

            Client? client = await _clientService.GetByExternalIdAsync(dto.ClientExternalId);
            if (client == null)
                return Result<Appointment>.Failure($"Client with ExternalId {dto.ClientExternalId} not found.");

            Service? service = await _serviceService.GetByExternalIdAsync(dto.ServiceExternalId);
            if (service == null)
                return Result<Appointment>.Failure($"Service with ExternalId {dto.ServiceExternalId} not found.");

            Appointment appointment = new()
            {
                ExternalId = Guid.NewGuid(),
                BusinessId = business.Id,
                Business = business,
                ProfessionalId = professional.Id,
                Professional = professional,
                ClientId = client.Id,
                Client = client,
                ServiceId = service.Id,
                Service = service,
                StartDateTime = dto.StartDateTime,
                EndDateTime = dto.EndDateTime,
                Notes = dto.Notes,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
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
