namespace turno_clave_API.Application.DTOs.Professional
{
    public class UpdateProfessionalDTO
    {
        public required string Name { get; set; }
        public List<Guid> ServiceExternalIds { get; set; } = [];
    }
}
