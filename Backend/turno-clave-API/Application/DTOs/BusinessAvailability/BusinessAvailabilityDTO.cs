using System.ComponentModel.DataAnnotations;

namespace turno_clave_API.Application.DTOs.BusinessAvailability
{
    public class BusinessAvailabilityDTO : IValidatableObject
    {
        [Required]
        public Guid ExternalId { get; set; }

        [Required]
        public DayOfWeek DayOfWeek { get; set; }

        [Required]
        public TimeOnly StartTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartTime >= EndTime)
                yield return new ValidationResult("StartTime must be earlier than EndTime.", new[] { nameof(StartTime), nameof(EndTime) });
        }
    }
}
