namespace turno_clave_API.Domain.Entities
{
    public class Service
    {
        public int Id { get; set; }

        public int BusinessId { get; set; }
        public required Business Business { get; set; }

        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int DurationMinutes { get; set; }
        public bool IsActive { get; set; } 
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        // Navigation properties

    }
}
