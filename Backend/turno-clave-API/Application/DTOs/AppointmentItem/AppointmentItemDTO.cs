using turno_clave_API.Application.DTOs.Professional;
using turno_clave_API.Application.DTOs.Service;

namespace turno_clave_API.Application.DTOs.AppointmentItem
{
    public class AppointmentItemDTO
    {
        
        public MinimalServiceDTO Service { get; set; } = null!;
        public ProfessionalDTO Professional { get; set; } = null!;
        public DateTimeOffset StartDateTime { get; set; }
        public DateTimeOffset EndDateTime { get; set; }
    }
}
