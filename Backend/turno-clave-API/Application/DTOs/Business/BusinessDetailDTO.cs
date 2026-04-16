using System.ComponentModel.DataAnnotations;

namespace turno_clave_API.Application.DTOs.Business
{
    public class BusinessDetailDTO
    {
        [Required]
        public Guid ExternalId { get; set; } = default!;
        [Required]
        public string Name { get; set; } = default!;
        [Required]
        public string Slug { get; set; } = default!;
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }

        [Required]
        public string Email { get; set; } = default!;
        [Required]
        public string Phone { get; set; } = default!;
        [Required]
        public string Address { get; set; } = default!;
        [Required]
        public string City { get; set; } = default!;
        [Required]
        public string State { get; set; } = default!;
        [Required]
        public string Country { get; set; } = default!;
    }
}
