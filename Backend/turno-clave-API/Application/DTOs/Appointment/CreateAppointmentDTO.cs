namespace turno_clave_API.Application.DTOs.Appointment
{
    public class CreateAppointmentDTO
    {
        public required string BusinessExternalId { get; set; }
        public required string ProfessionalExternalId { get; set; }
        // De qué forma conviene recibir client?

    }
}
