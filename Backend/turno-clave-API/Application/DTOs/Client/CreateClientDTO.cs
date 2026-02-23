namespace turno_clave_API.Application.DTOs.Client
{
    public class CreateClientDTO
    {
        public required Guid BusinessExternalId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public string? Notes { get; set; }
    }
}
