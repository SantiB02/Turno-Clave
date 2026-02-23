namespace turno_clave_API.Application.DTOs.Client
{
    public class ClientDTO
    {
        public Guid ExternalId { get; set; }
        public Guid BusinessExternalId { get; set; }
        public string BusinessName { get; set; } = string.Empty;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? Notes { get; set; }
    }
}
