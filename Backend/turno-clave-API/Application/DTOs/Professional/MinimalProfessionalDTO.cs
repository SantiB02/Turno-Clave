
namespace turno_clave_API.Application.DTOs.Professional
{
    public class MinimalProfessionalDTO
    {
        public Guid ExternalId { get; set; }
        public string Name { get; set; } = string.Empty;

        internal static ProfessionalDTO FromProfessional(Domain.Entities.Professional professional)
        {
            return new ProfessionalDTO
            {
                ExternalId = professional.ExternalId,
                Name = professional.Name
            };
        }
    }
}
