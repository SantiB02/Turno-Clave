namespace turno_clave_API.Application.DTOs.Availability
{
    public class CreateProfessionalAvailabilityDTO
    {
        public required Guid ProfessionalExternalId { get; set; }
        public required DayOfWeek DayOfWeek { get; set; }
        public required TimeOnly StartTime { get; set; }
        public required TimeOnly EndTime { get; set; } //TODO: validate that end time is after start time
    }
}
