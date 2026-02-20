using turno_clave_API.Domain.Enums;

namespace turno_clave_API.Domain.Entities
{
    public class AvailabilityException
    {
        public int Id { get; set; }

        public int BusinessId { get; set; }
        public required Business Business { get; set; }

        public int? ProfessionalId { get; set; }
        public Professional? Professional { get; set; } // If null, applies to all professionals in the business (global exception)

        public DateOnly Date { get; set; } // The specific date of the exception (e.g., 2026-12-25 for Christmas)

        public TimeOnly? StartDateTime { get; set; } // Optional: If null, the exception applies to the entire day. Otherwise, it applies to the specified time range on that date.
        public TimeOnly? EndDateTime { get; set; }
        public AvailabilityExceptionType Type { get; set; }
        public string? Reason { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
