namespace turno_clave_API.Application.DTOs.Client
{
    /// <summary>
    /// DTO para capturar información básica del cliente durante el flujo de reserva.
    /// Se utiliza cuando el cliente no tiene login y sus datos se reciben al final del flujo.
    /// 
    /// MVP approach: El cliente ingresa nombre, email y teléfono solo al confirmar la reserva.
    /// Si el email ya existe, se reutiliza el cliente existente (deduplicación).
    /// </summary>
    public class ClientInfoDTO
    {
        /// <summary>
        /// Nombre completo del cliente.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Email del cliente. Se usa como identificador único por negocio para deduplicación.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Teléfono de contacto del cliente.
        /// </summary>
        public required string Phone { get; set; }

        /// <summary>
        /// Notas adicionales del cliente (opcional).
        /// </summary>
        public string? Notes { get; set; }
    }
}
