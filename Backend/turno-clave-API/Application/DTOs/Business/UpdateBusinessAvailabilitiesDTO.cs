namespace turno_clave_API.Application.DTOs.Business
{
    public class UpdateBusinessAvailabilitiesDTO
    {
        public required List<UpdateBusinessAvailabilityDTO> Availabilities { get; set; } = [];
    }
}
