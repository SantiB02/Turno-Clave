namespace turno_clave_API.Application.DTOs.Appointment
{
    /// <summary>
    /// Representa un slot de horario disponible para los servicios seleccionados.
    /// El slot es continuo (bloque único) para todos los servicios.
    /// </summary>
    public class AvailabilitySlotDTO
    {
        /// <summary>
        /// Fecha del slot (sin hora).
        /// </summary>
        public required DateOnly Date { get; set; }

        /// <summary>
        /// Hora de inicio del slot.
        /// </summary>
        public required TimeOnly StartTime { get; set; }

        /// <summary>
        /// Hora de fin del slot (suma de todas las duraciones de servicios).
        /// </summary>
        public required TimeOnly EndTime { get; set; }

        /// <summary>
        /// Duración total en minutos del bloque (suma de duraciones de servicios).
        /// </summary>
        public int TotalDurationMinutes { get; set; }

        /// <summary>
        /// Información sobre los servicios y profesionales que se brindarán en este slot.
        /// </summary>
        public required List<AvailableServiceDetailDTO> ServiceDetails { get; set; }

        /// <summary>
        /// Disponibilidad restante en minutos en este horario (para información del cliente).
        /// </summary>
        public int AvailableMinutesAfter { get; set; }
    }

    /// <summary>
    /// Detalle de un servicio dentro de un slot de disponibilidad.
    /// </summary>
    public class AvailableServiceDetailDTO
    {
        /// <summary>
        /// ID externo del servicio.
        /// </summary>
        public required Guid ServiceExternalId { get; set; }

        /// <summary>
        /// Nombre del servicio.
        /// </summary>
        public string ServiceName { get; set; } = string.Empty;

        /// <summary>
        /// Duración del servicio en minutos.
        /// </summary>
        public int DurationMinutes { get; set; }

        /// <summary>
        /// Precio del servicio.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// ID externo del profesional asignado. Null si se autoasignará.
        /// </summary>
        public Guid? AssignedProfessionalExternalId { get; set; }

        /// <summary>
        /// Nombre del profesional asignado. Null si se autoasignará.
        /// </summary>
        public string? AssignedProfessionalName { get; set; }

        /// <summary>
        /// Hora de inicio específica de este servicio dentro del bloque.
        /// </summary>
        public TimeOnly ServiceStartTime { get; set; }

        /// <summary>
        /// Hora de fin específica de este servicio dentro del bloque.
        /// </summary>
        public TimeOnly ServiceEndTime { get; set; }
    }

    /// <summary>
    /// Respuesta con múltiples slots disponibles para los servicios seleccionados.
    /// </summary>
    public class AvailabilitySlotsResponseDTO
    {
        /// <summary>
        /// Lista de slots disponibles. Están ordenados por fecha y hora.
        /// </summary>
        public required List<AvailabilitySlotDTO> AvailableSlots { get; set; }

        /// <summary>
        /// Rango de fechas que se consultó.
        /// </summary>
        public required DateOnly SearchFromDate { get; set; }

        /// <summary>
        /// Rango de fechas que se consultó.
        /// </summary>
        public required DateOnly SearchToDate { get; set; }

        /// <summary>
        /// Cantidad total de slots disponibles encontrados.
        /// </summary>
        public int TotalSlotsFound => AvailableSlots.Count;
    }
}
