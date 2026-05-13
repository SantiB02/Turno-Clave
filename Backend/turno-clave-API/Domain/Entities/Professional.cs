using turno_clave_API.Application.DTOs.Professional;

namespace turno_clave_API.Domain.Entities
{
    public class Professional
    {
        public int Id { get; set; }
        public Guid ExternalId { get; set; } = Guid.NewGuid();

        public int BusinessId { get; set; }
        public required Business Business { get; set; }

        public required string Name { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<Appointment> Appointments { get; set; } = [];
        public ICollection<Availability> Availabilities { get; set; } = [];
        public ICollection<ProfessionalService> ProfessionalServices { get; set; } = [];

        public static ProfessionalDTO ToDto(Professional p)
        {
            return new ProfessionalDTO
            {
                ExternalId = p.ExternalId,
                BusinessExternalId = p.Business?.ExternalId ?? Guid.Empty,
                BusinessName = p.Business?.Name ?? string.Empty,
                Name = p.Name,
                IsActive = p.IsActive,
                Availabilities = p.Availabilities
                    .Select(Availability.ToProfessionalAvailabilityDTO)
                    .ToList(),
            };
        }
    }
}
