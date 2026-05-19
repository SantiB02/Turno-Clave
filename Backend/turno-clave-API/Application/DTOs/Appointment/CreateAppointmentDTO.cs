using turno_clave_API.Application.DTOs.AppointmentItem;
using turno_clave_API.Application.DTOs.Client;

namespace turno_clave_API.Application.DTOs.Appointment
{
    /// <summary>
    /// DTO para crear una nueva reserva con múltiples servicios en un bloque continuo.
    /// Flujo de 3 pasos:
    /// 1. Cliente selecciona servicios (SelectionRequestDTO)
    /// 2. Cliente ve slots disponibles (AvailabilitySlotDTO)
    /// 3. Cliente confirma con datos personales (CreateAppointmentDTO) ← AQUÍ
    /// 
    /// Los datos del cliente se capturan al final, sin necesidad de login.
    /// Si el email ya existe, se reutiliza el cliente existente.
    /// </summary>
    public class CreateAppointmentDTO
    {
        /// <summary>
        /// ID externo del negocio donde se realiza la reserva.
        /// </summary>
        public required Guid BusinessExternalId { get; set; }

        /// <summary>
        /// Información del cliente que realiza la reserva.
        /// Se captura al final del flujo (no requiere login).
        /// </summary>
        public required ClientInfoDTO Client { get; set; }

        /// <summary>
        /// Hora de inicio del bloque completo (en UTC).
        /// Todas las reservas serán continuas desde esta hora.
        /// </summary>
        public required DateTimeOffset StartDateTime { get; set; }

        /// <summary>
        /// Hora de fin del bloque completo (en UTC).
        /// Calculada como la suma de todas las duraciones de servicios.
        /// </summary>
        public required DateTimeOffset EndDateTime { get; set; }

        /// <summary>
        /// Lista de servicios que se reservan en este bloque continuo.
        /// El orden y los horarios son secuenciales sin interrupciones.
        /// </summary>
        public required List<CreateAppointmentItemDTO> Items { get; set; }

        /// <summary>
        /// Notas generales sobre la reserva.
        /// </summary>
        public string? Notes { get; set; }
    }
}
