using turno_clave_API.Application.DTOs.Appointment;
using turno_clave_API.Common;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Application.Interfaces
{
    public interface IAppointmentService
    {
        Task<Result<Appointment>> CreateAsync(CreateAppointmentDTO dto);
        Task<Appointment?> GetByExternalIdAsync(Guid externalId);
        // Task<Result<Appointment>> UpdateAsync(UpdateAppointmentDTO dto); // too complex for MVP (?)
        Task<Result<Appointment>> CancelAsync(Guid externalId);
        Task<Result<Appointment>> CancelAsync(Appointment appointment);
    }
}
