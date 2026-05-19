using turno_clave_API.Application.DTOs.Professional;
using turno_clave_API.Application.DTOs.Service;

namespace turno_clave_API.Domain.Entities
{
    public class Service
    {
        public int Id { get; set; }
        public Guid ExternalId { get; set; } = Guid.NewGuid();

        public int BusinessId { get; set; }
        public required Business Business { get; set; }

        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int DurationMinutes { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        // Navigation
        public ICollection<AppointmentItem> AppointmentItems { get; set; } = [];
        public ICollection<ProfessionalService> ProfessionalServices { get; set; } = [];

        public static ServiceDTO ToDto(Service s)
        {
            return new ServiceDTO
            {
                ExternalId = s.ExternalId,
                BusinessExternalId = s.Business?.ExternalId ?? Guid.Empty,
                BusinessName = s.Business?.Name ?? string.Empty,
                Name = s.Name,
                Description = s.Description,
                Price = s.Price,
                DurationMinutes = s.DurationMinutes,

                Professionals = s.ProfessionalServices
                    .Select(ps => new MinimalProfessionalDTO
                    {
                        ExternalId = ps.Professional.ExternalId,
                        Name = ps.Professional.Name,
                    })
                    .ToList()
            };
        } 
    }
}
