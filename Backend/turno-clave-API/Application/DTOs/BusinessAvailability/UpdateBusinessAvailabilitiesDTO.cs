namespace turno_clave_API.Application.DTOs.BusinessAvailability
{
    public class UpdateBusinessAvailabilitiesDTO
    {
        public required List<UpdateBusinessAvailabilityDTO> Availabilities { get; set; } = [];
    }
}
