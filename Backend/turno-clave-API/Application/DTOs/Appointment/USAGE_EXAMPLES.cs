namespace turno_clave_API.Application.DTOs.Appointment
{
    /// <summary>
    /// Archivo de referencia con ejemplos de uso del flujo de reservas multiservicio.
    /// Este archivo NO es una clase de producción, solo documentación de referencia.
    /// 
    /// FLUJO DE RESERVA EN 2 PASOS:
    /// 
    /// PASO 1: Cliente selecciona servicios y profesionales
    /// POST /api/appointments/availability
    /// Body: SelectionRequestDTO
    /// {
    ///   "businessExternalId": "123e4567-e89b-12d3-a456-426614174000",
    ///   "services": [
    ///     {
    ///       "serviceExternalId": "323e4567-e89b-12d3-a456-426614174000",  // Depilación
    ///       "professionalExternalId": null  // Sin preferencia, se autoasigna
    ///     },
    ///     {
    ///       "serviceExternalId": "423e4567-e89b-12d3-a456-426614174000",  // Masaje
    ///       "professionalExternalId": "523e4567-e89b-12d3-a456-426614174000"  // Profesional específico
    ///     }
    ///   ],
    ///   "searchFromDate": "2025-01-15",
    ///   "searchToDate": "2025-01-31"
    /// }
    /// 
    /// RESPUESTA: AvailabilitySlotsResponseDTO
    /// {
    ///   "availableSlots": [
    ///     {
    ///       "date": "2025-01-20",
    ///       "startTime": "10:00",
    ///       "endTime": "11:45",  // 45 min depilación + 60 min masaje
    ///       "totalDurationMinutes": 105,
    ///       "serviceDetails": [
    ///         {
    ///           "serviceExternalId": "323e4567-e89b-12d3-a456-426614174000",
    ///           "serviceName": "Depilación",
    ///           "durationMinutes": 45,
    ///           "price": 50.00,
    ///           "assignedProfessionalExternalId": "623e4567-e89b-12d3-a456-426614174000",
    ///           "assignedProfessionalName": "Paula",
    ///           "serviceStartTime": "10:00",
    ///           "serviceEndTime": "10:45"
    ///         },
    ///         {
    ///           "serviceExternalId": "423e4567-e89b-12d3-a456-426614174000",
    ///           "serviceName": "Masaje Relajante",
    ///           "durationMinutes": 60,
    ///           "price": 80.00,
    ///           "assignedProfessionalExternalId": "523e4567-e89b-12d3-a456-426614174000",
    ///           "assignedProfessionalName": "Carlos",
    ///           "serviceStartTime": "10:45",  // Comienza donde terminó el anterior
    ///           "serviceEndTime": "11:45"
    ///         }
    ///       ],
    ///       "availableMinutesAfter": 30  // Hay 30 minutos hasta el siguiente compromiso
    ///     },
    ///     {
    ///       "date": "2025-01-20",
    ///       "startTime": "14:00",
    ///       "endTime": "15:45",
    ///       ...
    ///     }
    ///   ],
    ///   "searchFromDate": "2025-01-15",
    ///   "searchToDate": "2025-01-31",
    ///   "totalSlotsFound": 12
    /// }
    /// 
    /// PASO 2: Cliente selecciona un slot y confirma
    /// POST /api/appointments
    /// Body: CreateAppointmentDTO
    /// {
    ///   "businessExternalId": "123e4567-e89b-12d3-a456-426614174000",
    ///   "clientExternalId": "223e4567-e89b-12d3-a456-426614174000",
    ///   "startDateTime": "2025-01-20T10:00:00-03:00",  // Convertir a zona horaria del negocio
    ///   "endDateTime": "2025-01-20T11:45:00-03:00",
    ///   "items": [
    ///     {
    ///       "serviceExternalId": "323e4567-e89b-12d3-a456-426614174000",
    ///       "professionalExternalId": "623e4567-e89b-12d3-a456-426614174000",
    ///       "startTime": "10:00",  // TimeOnly en zona horaria local
    ///       "endTime": "10:45",
    ///       "notes": null
    ///     },
    ///     {
    ///       "serviceExternalId": "423e4567-e89b-12d3-a456-426614174000",
    ///       "professionalExternalId": "523e4567-e89b-12d3-a456-426614174000",
    ///       "startTime": "10:45",
    ///       "endTime": "11:45",
    ///       "notes": "Preferencia: masaje firme en espalda"
    ///     }
    ///   ],
    ///   "notes": "Primera cita, cliente nuevo"
    /// }
    /// 
    /// RESPUESTA: AppointmentDTO
    /// {
    ///   "externalId": "823e4567-e89b-12d3-a456-426614174000",
    ///   "businessExternalId": "123e4567-e89b-12d3-a456-426614174000",
    ///   "clientExternalId": "223e4567-e89b-12d3-a456-426614174000",
    ///   "startDateTime": "2025-01-20T10:00:00-03:00",
    ///   "endDateTime": "2025-01-20T11:45:00-03:00",
    ///   "status": "Pending",
    ///   "createdAt": "2025-01-15T14:32:00Z",
    ///   "items": [
    ///     {
    ///       "service": {
    ///         "externalId": "323e4567-e89b-12d3-a456-426614174000",
    ///         "name": "Depilación",
    ///         "durationMinutes": 45,
    ///         "price": 50.00
    ///       },
    ///       "professional": {
    ///         "externalId": "623e4567-e89b-12d3-a456-426614174000",
    ///         "name": "Paula"
    ///       },
    ///       "startDateTime": "2025-01-20T10:00:00-03:00",
    ///       "endDateTime": "2025-01-20T10:45:00-03:00"
    ///     }
    ///   ]
    /// }
    /// 
    /// VENTAJAS DE ESTA ESTRUCTURA:
    /// 1. Los turnos son continuos/bloques (sin gaps)
    /// 2. Soporta profesionales diferentes por servicio
    /// 3. Permite búsqueda de disponibilidad inteligente
    /// 4. Separación clara entre selección y confirmación
    /// 5. Notas a nivel de servicio y turno general
    /// 
    /// FLUJO EN EL FRONTEND:
    /// 1. Usuario selecciona servicios + profesionales (SelectionRequestDTO)
    /// 2. App llama a GET /api/appointments/availability con SelectionRequestDTO
    /// 3. Muestra calendario con AvailabilitySlotDTO
    /// 4. Usuario selecciona un slot
    /// 5. App crea CreateAppointmentDTO a partir del slot seleccionado
    /// 6. Usuario confirma y envía POST /api/appointments
    /// 
    /// LÓGICA EN EL BACKEND:
    /// - AvailabilityService.GetAvailableSlots(SelectionRequestDTO)
    ///   - Obtiene duraciones de servicios
    ///   - Valida que profesionales puedan hacer esos servicios
    ///   - Calcula bloques continuos de disponibilidad
    ///   - Asigna profesionales si es necesario
    ///   - Retorna AvailabilitySlotsResponseDTO
    /// 
    /// - AppointmentService.CreateAppointment(CreateAppointmentDTO)
    ///   - Valida que el slot siga disponible
    ///   - Crea Appointment (padre)
    ///   - Crea AppointmentItems (hijos)
    ///   - Actualiza disponibilidad de profesionales
    /// </summary>
    internal class USAGE_EXAMPLES
    {
        // Este archivo es solo para documentación. Ver ejemplos JSON arriba.
    }
}
