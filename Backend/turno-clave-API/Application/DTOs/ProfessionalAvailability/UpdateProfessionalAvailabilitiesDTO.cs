using turno_clave_API.Application.DTOs.Availability;

namespace turno_clave_API.Application.DTOs.ProfessionalAvailability
{
    public class UpdateProfessionalAvailabilitiesDTO
    {
        public List<UpdateProfessionalAvailabilityDTO> Availabilities { get; set; } = [];
    }
}
