namespace turno_clave_API.Application.DTOs.Appointment
{
    /// <summary>
    /// Representa la selección de servicios y profesionales en la primera pantalla de reserva.
    /// Usado para solicitar slots de horarios disponibles.
    /// </summary>
    public class SelectionRequestDTO
    {
        /// <summary>
        /// ID externo del negocio.
        /// </summary>
        public required Guid BusinessExternalId { get; set; }

        /// <summary>
        /// Lista de servicios seleccionados con profesionales asignados (opcional).
        /// </summary>
        public required List<ServiceSelectionDTO> Services { get; set; }

        /// <summary>
        /// Fecha desde la cual buscar disponibilidad.
        /// </summary>
        public required DateTime SearchFromDate { get; set; }

        /// <summary>
        /// Fecha hasta la cual buscar disponibilidad.
        /// </summary>
        public required DateTime SearchToDate { get; set; }
    }

    /// <summary>
    /// Representa un servicio seleccionado con su profesional asignado (opcional).
    /// </summary>
    public class ServiceSelectionDTO
    {
        /// <summary>
        /// ID externo del servicio.
        /// </summary>
        public required Guid ServiceExternalId { get; set; }

        /// <summary>
        /// ID externo del profesional que brindará el servicio (nullable si no tiene preferencia).
        /// </summary>
        public Guid? ProfessionalExternalId { get; set; }
    }
}
