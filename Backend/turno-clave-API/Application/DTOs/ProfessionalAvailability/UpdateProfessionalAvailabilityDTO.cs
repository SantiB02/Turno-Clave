namespace turno_clave_API.Application.DTOs.Availability
{
    public class UpdateProfessionalAvailabilityDTO
    {
        public required DayOfWeek DayOfWeek { get; set; }
        public required TimeOnly StartTime { get; set; }
        public required TimeOnly EndTime { get; set; }
    }
}
