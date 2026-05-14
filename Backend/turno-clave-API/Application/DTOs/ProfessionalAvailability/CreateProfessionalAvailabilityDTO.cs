using System.ComponentModel.DataAnnotations;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Application.DTOs.Availability
{
    public class CreateProfessionalAvailabilityDTO : AvailabilityRange
    {
        [Required]
        public Guid ProfessionalExternalId { get; set; }
    }
}
