namespace turno_clave_API.Application.DTOs.Service
{
    public class CreateServiceDTO
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int DurationMinutes { get; set; }
    }
}
