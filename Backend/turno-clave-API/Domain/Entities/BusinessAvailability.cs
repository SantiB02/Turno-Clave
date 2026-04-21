using System;

namespace turno_clave_API.Domain.Entities
{
    public class BusinessAvailability
    {
        public int Id { get; set; }
        public Guid ExternalId { get; set; } = Guid.NewGuid();

        // The owning business
        public int BusinessId { get; set; }
        public Business? Business { get; set; }

        // Weekly template: which day of week this entry applies to
        public DayOfWeek Day { get; set; }

        // Time range (local to the business timezone). Use TimeSpan to represent time-of-day.
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
