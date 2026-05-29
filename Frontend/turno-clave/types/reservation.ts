/**
 * API Types - Frontend TypeScript
 *
 * Estos tipos corresponden a los DTOs del backend .NET
 * Mapeo:
 * - SelectionRequestDTO → ISelectionRequest
 * - AvailabilitySlotDTO → IAvailabilitySlot
 * - AvailableServiceDetailDTO → IAvailableServiceDetail
 * - AvailabilitySlotsResponseDTO → IAvailabilitySlotsResponse
 * - CreateAppointmentDTO → ICreateAppointment
 * - CreateAppointmentItemDTO → ICreateAppointmentItem
 * - ClientInfoDTO → IClientInfo
 */

// ============================================
// PASO 1: SELECCIÓN DE SERVICIOS
// ============================================

/**
 * Servicio seleccionado con profesional opcional
 */
export interface IServiceSelection {
  serviceExternalId: string // UUID
  professionalExternalId?: string | null // UUID o null si sin preferencia
}

/**
 * Request para buscar slots disponibles
 *
 * Ejemplo:
 * {
 *   "businessExternalId": "123e4567-e89b-12d3-a456-426614174000",
 *   "services": [
 *     { "serviceExternalId": "...", "professionalExternalId": null },
 *     { "serviceExternalId": "...", "professionalExternalId": "..." }
 *   ],
 *   "searchFromDate": "2025-01-15T00:00:00",
 *   "searchToDate": "2025-01-31T23:59:59"
 * }
 */
export interface ISelectionRequest {
  businessExternalId: string
  services: IServiceSelection[]
  searchFromDate: Date | string // ISO string o Date object
  searchToDate: Date | string
}

// ============================================
// PASO 2: VER SLOTS DISPONIBLES
// ============================================

/**
 * Detalle de un servicio dentro de un slot
 *
 * Ejemplo:
 * {
 *   "serviceExternalId": "323e4567-e89b-12d3-a456-426614174000",
 *   "serviceName": "Depilación",
 *   "durationMinutes": 45,
 *   "price": 50.00,
 *   "assignedProfessionalExternalId": "623e4567-e89b-12d3-a456-426614174000",
 *   "assignedProfessionalName": "Paula",
 *   "serviceStartTime": "10:00",
 *   "serviceEndTime": "10:45"
 * }
 */
export interface IAvailableServiceDetail {
  serviceExternalId: string
  serviceName: string
  durationMinutes: number
  price: number
  assignedProfessionalExternalId?: string | null
  assignedProfessionalName?: string | null
  serviceStartTime: string // HH:mm format
  serviceEndTime: string
}

/**
 * Slot de horario disponible
 *
 * Ejemplo:
 * {
 *   "date": "2025-01-20",
 *   "startTime": "10:00",
 *   "endTime": "11:45",
 *   "totalDurationMinutes": 105,
 *   "serviceDetails": [...],
 *   "availableMinutesAfter": 30
 * }
 */
export interface IAvailabilitySlot {
  date: string // YYYY-MM-DD format
  startTime: string // HH:mm format
  endTime: string
  totalDurationMinutes: number
  serviceDetails: IAvailableServiceDetail[]
  availableMinutesAfter: number
}

/**
 * Response con slots disponibles
 *
 * Ejemplo:
 * {
 *   "availableSlots": [...],
 *   "searchFromDate": "2025-01-15",
 *   "searchToDate": "2025-01-31",
 *   "totalSlotsFound": 12
 * }
 */
export interface IAvailabilitySlotsResponse {
  availableSlots: IAvailabilitySlot[]
  searchFromDate: string // YYYY-MM-DD format
  searchToDate: string
  totalSlotsFound: number
}

// ============================================
// PASO 3: DATOS DEL CLIENTE
// ============================================

/**
 * Información del cliente (MVP - sin login)
 *
 * Ejemplo:
 * {
 *   "name": "Juan Pérez",
 *   "email": "juan@example.com",
 *   "phone": "+54911234567",
 *   "notes": null
 * }
 */
export interface IClientInfo {
  name: string
  email: string
  phone: string
  notes?: string | null
}

/**
 * Detalle de un servicio en la reserva
 *
 * Ejemplo:
 * {
 *   "serviceExternalId": "323e4567-e89b-12d3-a456-426614174000",
 *   "professionalExternalId": "623e4567-e89b-12d3-a456-426614174000",
 *   "startTime": "10:00",
 *   "endTime": "10:45",
 *   "notes": null
 * }
 */
export interface ICreateAppointmentItem {
  serviceExternalId: string
  professionalExternalId: string
  startTime: string // HH:mm format
  endTime: string
  notes?: string | null
}

/**
 * Request para crear una reserva
 *
 * Ejemplo:
 * {
 *   "businessExternalId": "123e4567-e89b-12d3-a456-426614174000",
 *   "client": {
 *     "name": "Juan Pérez",
 *     "email": "juan@example.com",
 *     "phone": "+54911234567",
 *     "notes": null
 *   },
 *   "startDateTime": "2025-01-20T10:00:00-03:00",
 *   "endDateTime": "2025-01-20T11:45:00-03:00",
 *   "items": [
 *     { "serviceExternalId": "...", "professionalExternalId": "...", "startTime": "10:00", "endTime": "10:45" },
 *     { "serviceExternalId": "...", "professionalExternalId": "...", "startTime": "10:45", "endTime": "11:45" }
 *   ],
 *   "notes": "Primera visita"
 * }
 */
export interface ICreateAppointment {
  businessExternalId: string
  client: IClientInfo
  startDateTime: Date | string // ISO string o Date object
  endDateTime: Date | string
  items: ICreateAppointmentItem[]
  notes?: string | null
}

export interface IReservationConfirmationServiceDetail {
  serviceExternalId: string
  serviceName: string
  professionalName?: string | null
  startTime: string
  endTime: string
}

export interface IReservationConfirmationDetails {
  clientEmail: string
  reservationCode: string | null
  date: string
  startTime: string
  endTime: string
  services: IReservationConfirmationServiceDetail[]
}

// ============================================
// HELPER TYPES
// ============================================

/**
 * Tipo para mapear un slot seleccionado con su información completa
 * Usado internamente para pasar datos entre pasos
 */
export interface ISelectedSlot extends IAvailabilitySlot {
  totalPrice: number // Suma de precios de servicios
}

/**
 * Estado del flujo de reserva
 */
export type BookingStep =
  | "selection"
  | "calendar"
  | "confirmation"
  | "loading"
  | "success"
  | "error"

/**
 * Estado completo de la aplicación de reservas
 */
export interface IBookingState {
  step: BookingStep
  businessExternalId: string
  selectedServices: IServiceSelection[]
  availabilityResponse: IAvailabilitySlotsResponse | null
  selectedSlot: ISelectedSlot | null
  clientInfo: IClientInfo | null
  error: string | null
  loading: boolean
}
