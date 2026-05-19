using turno_clave_API.Application.DTOs.Appointment;
using turno_clave_API.Application.DTOs.AppointmentItem;
using turno_clave_API.Application.DTOs.Professional;
using turno_clave_API.Application.DTOs.Service;
using turno_clave_API.Domain.Enums;

namespace turno_clave_API.Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public Guid ExternalId { get; set; } = Guid.NewGuid();

        public int BusinessId { get; set; }
        public required Business Business { get; set; }

        public int ClientId { get; set; }
        public required Client Client { get; set; }

        // Global start and end times for the appointment, stored in UTC
        public DateTimeOffset StartDateTime { get; set; }
        public DateTimeOffset EndDateTime { get; set; }
        public string? Notes { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<AppointmentItem> Items { get; set; } = [];

        public static AppointmentDTO ToDto(Appointment appt)
        {
            return new AppointmentDTO
            {
                ExternalId = appt.ExternalId,
                BusinessExternalId = appt.Business.ExternalId,
                ClientExternalId = appt.Client.ExternalId,
                StartDateTime = appt.StartDateTime,
                EndDateTime = appt.EndDateTime,
                Notes = appt.Notes,
                Status = appt.Status,
                CreatedAt = appt.CreatedAt,
                UpdatedAt = appt.UpdatedAt,
                Items = appt.Items.Select(item => new AppointmentItemDTO
                {
                    Service = MinimalServiceDTO.FromService(item.Service),
                    Professional = MinimalProfessionalDTO.FromProfessional(item.Professional),
                    StartDateTime = item.StartDateTime,
                    EndDateTime = item.EndDateTime,
                }).ToList()
            };
        }
    }
}
