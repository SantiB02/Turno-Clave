namespace turno_clave_API.Application.DTOs.Service
{
    public class ServiceDTO
    {
        public Guid ExternalId { get; set; }
        public Guid BusinessExternalId { get; set; }
        public string BusinessName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int DurationMinutes { get; set; }
    }
}
