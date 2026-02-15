using turno_clave_API.Domain.Enums;

namespace turno_clave_API.Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }

        public int BusinessId { get; set; }
        public required Business Business { get; set; }

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
        // Navigation properties

    }
}
