using turno_clave_API.Application.DTOs.Appointment;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Application.Interfaces
{
    public interface IAppointmentService
    {
        public Task<Appointment?> CreateAsync(CreateAppointmentDTO dto);
        public Task<Appointment?> GetByExternalId (Guid externalId);
    }
}
