namespace turno_clave_API.Domain.Entities
{
    public class Availability
    {
        public int Id { get; set; }

        public int BusinessId { get; set; }
        public required Business Business { get; set; }

        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public bool IsActive { get; set; }
    }
}
