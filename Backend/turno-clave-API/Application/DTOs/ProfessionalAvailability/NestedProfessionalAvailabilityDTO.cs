namespace turno_clave_API.Application.DTOs.Availability
{
    public class NestedProfessionalAvailabilityDTO
    {
        public Guid ExternalId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
