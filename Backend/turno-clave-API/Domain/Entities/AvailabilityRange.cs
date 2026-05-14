using System.ComponentModel.DataAnnotations;

namespace turno_clave_API.Domain.Entities
{
    public class AvailabilityRange : IValidatableObject
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartTime >= EndTime)
                yield return new ValidationResult("StartTime must be earlier than EndTime.", new[] { nameof(StartTime), nameof(EndTime) });
        }
    }
}
