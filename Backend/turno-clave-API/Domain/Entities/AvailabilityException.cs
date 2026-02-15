using turno_clave_API.Domain.Enums;

namespace turno_clave_API.Domain.Entities
{
    public class AvailabilityException
    {
        public int Id { get; set; }

        public int BusinessId { get; set; }
        public required Business Business { get; set; }

        public DateTimeOffset StartDateTime { get; set; }
        public DateTimeOffset EndDateTime { get; set; }
        public AvailabilityExceptionType Type { get; set; }
        public string? Reason { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
