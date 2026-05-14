using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Application.DTOs.Availability
{
    public class NestedProfessionalAvailabilityDTO : AvailabilityRange
    {
        public Guid ExternalId { get; set; }
    }
}
