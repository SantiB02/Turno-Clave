namespace turno_clave_API.Application.DTOs.Professional
{
    public class UpdateProfessionalDTO
    {
        public required Guid ExternalId { get; set; }
        public required string Name { get; set; }
    }
}
