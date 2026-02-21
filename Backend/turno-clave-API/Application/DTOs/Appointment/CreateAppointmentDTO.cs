namespace turno_clave_API.Application.DTOs.Appointment
{
    public class CreateAppointmentDTO
    {
        public required string BusinessExternalId { get; set; }
        public required string ProfessionalExternalId { get; set; }
        public required string ClientExternalId { get; set; }
        public required string ServiceExternalId { get; set; }
        public DateTimeOffset StartDateTime { get; set; }
        public DateTimeOffset EndDateTime { get; set; }
        public string? Notes { get; set; }
    }
}
