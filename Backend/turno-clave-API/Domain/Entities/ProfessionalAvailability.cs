using turno_clave_API.Application.DTOs.Availability;
using turno_clave_API.Application.DTOs.ProfessionalAvailability;

namespace turno_clave_API.Domain.Entities
{
    public class ProfessionalAvailability
    {
        public int Id { get; set; }
        public Guid ExternalId { get; set; } = Guid.NewGuid();

        public int ProfessionalId { get; set; }
        public Professional Professional { get; set; } = null!;

        public DayOfWeek DayOfWeek { get; set; } // 0-6 (Sunday to Saturday)
        public TimeOnly StartTime { get; set; } // 17:45, 18:00, 13:30:20, etc.
        public TimeOnly EndTime { get; set; }

        public static ProfessionalAvailabilityDTO ToDto(ProfessionalAvailability av)
        {
            return new ProfessionalAvailabilityDTO
            {
                ExternalId = av.ExternalId,
                ProfessionalExternalId = av.Professional.ExternalId,
                DayOfWeek = av.DayOfWeek,
                StartTime = av.StartTime,
                EndTime = av.EndTime
            };
        }

        public static NestedProfessionalAvailabilityDTO ToProfessionalAvailabilityDTO(ProfessionalAvailability av)
        {
            return new NestedProfessionalAvailabilityDTO
            {
                ExternalId = av.ExternalId,
                DayOfWeek = av.DayOfWeek,
                StartTime = av.StartTime,
                EndTime = av.EndTime
            };
        }
    }
}
