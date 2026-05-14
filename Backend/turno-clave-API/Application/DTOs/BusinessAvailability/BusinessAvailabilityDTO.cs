using System.ComponentModel.DataAnnotations;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Application.DTOs.BusinessAvailability
{
    public class BusinessAvailabilityDTO : AvailabilityRange
    {
        [Required]
        public Guid ExternalId { get; set; }
    }
}
