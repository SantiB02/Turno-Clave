namespace turno_clave_API.Domain.Entities
{
    public class AvailabilityRange
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
