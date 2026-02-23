namespace turno_clave_API.Application.DTOs.Professional
{
    public class ProfessionalDTO
    {
        public int Id { get; set; }
        public Guid ExternalId { get; set; }

        public Guid BusinessExternalId { get; set; }
        public string BusinessName { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
