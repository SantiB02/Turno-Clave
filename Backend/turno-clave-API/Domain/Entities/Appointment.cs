using turno_clave_API.Application.DTOs.Appointment;
using turno_clave_API.Domain.Enums;

namespace turno_clave_API.Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public Guid ExternalId { get; set; } = Guid.NewGuid();

        public int BusinessId { get; set; }
        public required Business Business { get; set; }

        public int ProfessionalId { get; set; }
        public required Professional Professional { get; set; }

        public int ClientId { get; set; }
        public required Client Client { get; set; }

        public int ServiceId { get; set; }
        public required Service Service { get; set; }

        public DateTimeOffset StartDateTime { get; set; }
        public DateTimeOffset EndDateTime { get; set; }
        public string? Notes { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        public static AppointmentDTO ToDto(Appointment appt)
        {
            return new AppointmentDTO
            {
                ExternalId = appt.ExternalId,
                BusinessExternalId = appt.Business.ExternalId,
                ProfessionalExternalId = appt.Professional.ExternalId,
                ClientExternalId = appt.Client.ExternalId,
                ServiceExternalId = appt.Service.ExternalId,
                StartDateTime = appt.StartDateTime,
                EndDateTime = appt.EndDateTime,
                Notes = appt.Notes,
                Status = appt.Status,
                CreatedAt = appt.CreatedAt,
                UpdatedAt = appt.UpdatedAt
            };
        }
    }
}
