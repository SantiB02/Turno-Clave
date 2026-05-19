namespace turno_clave_API.Application.DTOs.AppointmentItem
{
    /// <summary>
    /// Representa un servicio dentro de una reserva de turno.
    /// Cada AppointmentItem es parte del bloque continuo de la reserva.
    /// </summary>
    public class CreateAppointmentItemDTO
    {
        /// <summary>
        /// ID externo del servicio a reservar.
        /// </summary>
        public required Guid ServiceExternalId { get; set; }

        /// <summary>
        /// ID externo del profesional que brindará el servicio.
        /// Se requiere ya que debe ser conocido en el momento de creación.
        /// </summary>
        public required Guid ProfessionalExternalId { get; set; }

        /// <summary>
        /// Hora de inicio del servicio dentro del bloque de la reserva (en la zona horaria del cliente).
        /// </summary>
        public required TimeOnly StartTime { get; set; }

        /// <summary>
        /// Hora de fin del servicio dentro del bloque de la reserva (en la zona horaria del cliente).
        /// </summary>
        public required TimeOnly EndTime { get; set; }

        /// <summary>
        /// Notas específicas para este servicio (ej: observaciones especiales, preferencias, etc).
        /// </summary>
        public string? Notes { get; set; }
    }
}
