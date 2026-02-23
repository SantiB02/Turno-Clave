namespace turno_clave_API.Application.DTOs.Service
{
    public class UpdateServiceDTO
    {
        public required Guid ExternalId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? DurationMinutes { get; set; }
    }
}
