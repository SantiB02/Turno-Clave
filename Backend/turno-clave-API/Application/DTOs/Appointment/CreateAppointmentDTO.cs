namespace turno_clave_API.Application.DTOs.Appointment
{
    public class CreateAppointmentDTO
    {
        public required Guid BusinessExternalId { get; set; }
        public required Guid ProfessionalExternalId { get; set; }
        public required Guid ClientExternalId { get; set; }
        public required Guid ServiceExternalId { get; set; }
        public DateTimeOffset StartDateTime { get; set; }
        public DateTimeOffset EndDateTime { get; set; }
        public string? Notes { get; set; }
    }
}
