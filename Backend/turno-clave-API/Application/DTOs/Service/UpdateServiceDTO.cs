using turno_clave_API.Application.DTOs.Professional;

namespace turno_clave_API.Application.DTOs.Service
{
    public class UpdateServiceDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? DurationMinutes { get; set; }

        public List<Guid> ProfessionalExternalIds { get; set; } = [];
    }
}
