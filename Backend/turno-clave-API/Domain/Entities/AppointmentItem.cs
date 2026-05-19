namespace turno_clave_API.Domain.Entities
{
    public class AppointmentItem
    {
        public int Id { get; set; }

        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; } = null!;

        public int ServiceId { get; set; }
        public Service Service { get; set; } = null!;

        public int ProfessionalId { get; set; }
        public Professional Professional { get; set; } = null!;

        public DateTimeOffset StartDateTime { get; set; }
        public DateTimeOffset EndDateTime { get; set; }
    }
}
