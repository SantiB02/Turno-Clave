import type { Client } from "./client"
import type { Professional } from "./professional"
import type { MinimalService } from "./service"

export type AppointmentStatus =
  | "Pending"
  | "Confirmed"
  | "Cancelled"
  | "Completed"
// ajustá los valores según tu enum real en .NET

export interface Appointment {
  externalId: string
  businessExternalId: string
  reservationCode: string
  client: Client

  // UTC datetimes
  startDateTime: string
  endDateTime: string

  notes?: string | null

  status: AppointmentStatus

  createdAt: string
  updatedAt: string

  items: AppointmentItem[]
}

export interface AppointmentItem {
  service: MinimalService
  professional: Professional

  startDateTime: string
  endDateTime: string
}
