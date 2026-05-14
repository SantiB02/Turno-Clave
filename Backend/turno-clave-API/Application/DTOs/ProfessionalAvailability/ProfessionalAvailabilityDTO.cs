using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Application.DTOs.ProfessionalAvailability
{
    public class ProfessionalAvailabilityDTO : AvailabilityRange
    {
        public Guid ExternalId { get; set; }
        public Guid ProfessionalExternalId { get; set; }
    }
}
