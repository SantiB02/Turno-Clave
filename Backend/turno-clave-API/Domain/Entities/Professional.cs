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

        public ICollection<AppointmentItem> AppointmentItems { get; set; } = [];
        public ICollection<ProfessionalAvailability> Availabilities { get; set; } = [];
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
                    .Select(ProfessionalAvailability.ToProfessionalAvailabilityDTO)
                    .ToList(),
                Services = p.ProfessionalServices
                    .Select(ps => new Application.DTOs.Service.MinimalServiceDTO
                    {
                        ExternalId = ps.Service.ExternalId,
                        Name = ps.Service.Name,
                        Description = ps.Service.Description,
                        Price = ps.Service.Price,
                        DurationMinutes = ps.Service.DurationMinutes,
                    })
                    .ToList(),
            };
        }
    }
}
