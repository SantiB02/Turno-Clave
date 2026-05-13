using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace turno_clave_API.Application.DTOs.BusinessAvailability
{
    public class CreateBusinessAvailabilityDTO : IValidatableObject
    {
        [Required]
        public DayOfWeek DayOfWeek { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartTime < TimeSpan.Zero || StartTime >= TimeSpan.FromDays(1))
                yield return new ValidationResult("StartTime must be within a single day.", new[] { nameof(StartTime) });

            if (EndTime <= TimeSpan.Zero || EndTime > TimeSpan.FromDays(1))
                yield return new ValidationResult("EndTime must be within a single day.", new[] { nameof(EndTime) });

            if (StartTime >= EndTime)
                yield return new ValidationResult("StartTime must be earlier than EndTime.", new[] { nameof(StartTime), nameof(EndTime) });
        }
    }
}
