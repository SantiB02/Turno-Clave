namespace turno_clave_API.Application.DTOs.Professional
{
    public class CreateProfessionalDTO
    {
        public required Guid BusinessExternalId { get; set; }
        public required string Name { get; set; }
    }
}
