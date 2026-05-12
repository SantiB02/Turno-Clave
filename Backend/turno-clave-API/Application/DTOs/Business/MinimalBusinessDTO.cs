using System.ComponentModel.DataAnnotations;

namespace turno_clave_API.Application.DTOs.Business
{
    public class MinimalBusinessDTO
    {
        [Required]
        public Guid ExternalId { get; set; } = default!;
        [Required]
        public string Name { get; set; } = default!;
        [Required]
        public string Slug { get; set; } = default!;
    }
}
