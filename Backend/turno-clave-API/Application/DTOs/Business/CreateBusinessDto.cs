using System.ComponentModel.DataAnnotations;
using turno_clave_API.Application.DTOs.BusinessAvailability;

namespace turno_clave_API.Application.DTOs.Business
{
    public class CreateBusinessDTO
    {
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters.")]
        public required string Name { get; set; }
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string Country { get; set; }
        // Time zone identifier for the business (IANA or Windows). Example: "America/Argentina/Buenos_Aires" or "UTC"
        public required string TimeZone { get; set; }
        [MinLength(1, ErrorMessage = "At least one availability must be provided.")]
        public required CreateBusinessAvailabilityDTO[] Availabilities { get; set; }
    }
}
