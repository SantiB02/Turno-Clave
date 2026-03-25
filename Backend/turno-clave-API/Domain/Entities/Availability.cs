using turno_clave_API.Application.DTOs.Availability;

namespace turno_clave_API.Domain.Entities
{
    public class Availability
    {
        public int Id { get; set; }
        public Guid ExternalId { get; set; } = Guid.NewGuid();

        public int ProfessionalId { get; set; }
        public required Professional Professional { get; set; }

        public DayOfWeek DayOfWeek { get; set; } // 0-6 (Sunday to Saturday)
        public TimeOnly StartTime { get; set; } // 17:45, 18:00, 13:30:20, etc.
        public TimeOnly EndTime { get; set; }

        public static AvailabilityDTO ToDto(Availability av)
        {
            return new AvailabilityDTO
            {
                ExternalId = av.ExternalId,
                ProfessionalExternalId = av.Professional.ExternalId,
                DayOfWeek = av.DayOfWeek,
                StartTime = av.StartTime,
                EndTime = av.EndTime
            };
        }
    }
}
