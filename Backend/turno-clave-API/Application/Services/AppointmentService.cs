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

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IBusinessService businessService,
            IProfessionalService professionalService,
            IClientService clientService,
            IServiceService serviceService)
        {
            _appointmentRepository = appointmentRepository;
            _businessService = businessService;
            _professionalService = professionalService;
            _clientService = clientService;
            _serviceService = serviceService;
        }

        public async Task<Result<Appointment>> CreateAsync(CreateAppointmentDTO dto)
        {
            Business? business = await _businessService.GetByExternalIdAsync(dto.BusinessExternalId);
            if (business == null)
                return Result<Appointment>.Failure($"Business with ExternalId {dto.BusinessExternalId} not found.");

            Professional? professional = await _professionalService.GetByExternalIdAsync(dto.ProfessionalExternalId);
            if (professional == null)
                return Result<Appointment>.Failure($"Professional with ExternalId {dto.ProfessionalExternalId} not found.");

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

        public async Task<Result<Appointment>> DeleteAsync(Guid externalId)
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
    }
}
