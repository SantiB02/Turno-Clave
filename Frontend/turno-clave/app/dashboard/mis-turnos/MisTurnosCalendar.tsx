"use client"

import type {
  EventClickArg,
  EventHoveringArg,
  EventInput,
  EventSourceFuncArg,
} from "@fullcalendar/core"
import dayGridPlugin from "@fullcalendar/daygrid"
import interactionPlugin from "@fullcalendar/interaction"
import FullCalendar from "@fullcalendar/react"
import timeGridPlugin from "@fullcalendar/timegrid"
import { useCallback, useState } from "react"
import ErrorMessage from "@/app/components/ErrorMessage"
import { getMyAppointments } from "@/services/appointmentService"
import type { Appointment } from "@/types/appointment"
import type { BusinessAvailabilityDTO } from "@/types/business"

type MisTurnosCalendarProps = {
  businessAvailabilities: BusinessAvailabilityDTO[]
}

type AppointmentTooltip = {
  appointment: Appointment
  x: number
  y: number
  pinned: boolean
}

const dayNameToNumber: Record<string, number> = {
  sunday: 0,
  monday: 1,
  tuesday: 2,
  wednesday: 3,
  thursday: 4,
  friday: 5,
  saturday: 6,
}

function normalizeTime(time: string) {
  const [hours = "00", minutes = "00", seconds = "00"] = time.split(":")

  return `${hours.padStart(2, "0")}:${minutes.padStart(2, "0")}:${seconds.padStart(2, "0")}`
}

function getBusinessTimeRange(availabilities: BusinessAvailabilityDTO[]) {
  if (availabilities.length === 0) {
    return {
      slotMinTime: "00:00:00",
      slotMaxTime: "24:00:00",
    }
  }

  const startTimes = availabilities.map((availability) =>
    normalizeTime(availability.startTime),
  )
  const endTimes = availabilities.map((availability) =>
    normalizeTime(availability.endTime),
  )

  return {
    slotMinTime: startTimes.sort()[0],
    slotMaxTime: endTimes.sort().at(-1) ?? "24:00:00",
  }
}

function getDayOfWeek(dayOfWeek: number | string) {
  if (typeof dayOfWeek === "number") {
    return dayOfWeek
  }

  const parsedDay = Number(dayOfWeek)

  if (!Number.isNaN(parsedDay)) {
    return parsedDay
  }

  return dayNameToNumber[dayOfWeek.toLowerCase()]
}

function mapAvailabilitiesToBusinessHours(
  availabilities: BusinessAvailabilityDTO[],
) {
  return availabilities
    .map((availability) => {
      const dayOfWeek = getDayOfWeek(availability.dayOfWeek)

      if (dayOfWeek === undefined) {
        return null
      }

      return {
        daysOfWeek: [dayOfWeek],
        startTime: normalizeTime(availability.startTime),
        endTime: normalizeTime(availability.endTime),
      }
    })
    .filter((businessHour) => businessHour !== null)
}

function getAppointmentTitle(appointment: Appointment) {
  const services = appointment.items
    .map((item) => item.service.name)
    .filter(Boolean)
    .join(", ")

  return services || `Turno ${appointment.reservationCode}`
}

function mapAppointmentToEvent(appointment: Appointment): EventInput {
  return {
    id: appointment.externalId,
    title: getAppointmentTitle(appointment),
    start: appointment.startDateTime,
    end: appointment.endDateTime,
    extendedProps: {
      appointment,
      status: appointment.status,
      reservationCode: appointment.reservationCode,
      clientName: appointment.client?.name,
    },
  }
}

function getEventAppointment(
  extendedProps: Record<string, unknown>,
): Appointment | null {
  const appointment = extendedProps.appointment

  if (!appointment) {
    return null
  }

  return appointment as Appointment
}

function formatAppointmentDateRange(appointment: Appointment) {
  const startDate = new Date(appointment.startDateTime)
  const endDate = new Date(appointment.endDateTime)

  return new Intl.DateTimeFormat("es-AR", {
    dateStyle: "short",
    timeStyle: "short",
  }).formatRange(startDate, endDate)
}

function getTooltipPosition(anchorElement: HTMLElement) {
  const rect = anchorElement.getBoundingClientRect()
  const tooltipWidth = 320
  const tooltipHeight = 260
  const gap = 12
  const hasRoomOnRight = rect.right + gap + tooltipWidth <= window.innerWidth

  return {
    x: hasRoomOnRight
      ? rect.right + gap
      : Math.max(gap, rect.left - tooltipWidth - gap),
    y: Math.max(gap, Math.min(rect.top, window.innerHeight - tooltipHeight)),
  }
}

export default function MisTurnosCalendar({
  businessAvailabilities,
}: MisTurnosCalendarProps) {
  const [error, setError] = useState<string | null>(null)
  const [loading, setLoading] = useState(false)
  const [tooltip, setTooltip] = useState<AppointmentTooltip | null>(null)
  const { slotMinTime, slotMaxTime } = getBusinessTimeRange(
    businessAvailabilities,
  )
  const businessHours = mapAvailabilitiesToBusinessHours(businessAvailabilities)

  const loadAppointments = useCallback(
    async (
      info: EventSourceFuncArg,
      successCallback: (events: EventInput[]) => void,
      failureCallback: (error: Error) => void,
    ) => {
      setLoading(true)
      setError(null)

      const result = await getMyAppointments(info.startStr, info.endStr)

      if (!result.ok) {
        setError(result.message)
        successCallback([])
        failureCallback(new Error(result.message))
        setLoading(false)
        return
      }

      successCallback(result.data.map(mapAppointmentToEvent))
      setLoading(false)
    },
    [],
  )

  const showTooltip = useCallback(
    ({ event, el }: EventHoveringArg | EventClickArg, pinned = false) => {
      const appointment = getEventAppointment(event.extendedProps)

      if (!appointment) {
        return
      }

      const { x, y } = getTooltipPosition(el)

      setTooltip({
        appointment,
        x,
        y,
        pinned,
      })
    },
    [],
  )

  const hideHoverTooltip = useCallback(() => {
    setTimeout(() => {
      setTooltip((currentTooltip) =>
        currentTooltip?.pinned ? currentTooltip : null,
      )
    }, 120)
  }, [])

  return (
    <div className="mis-turnos-calendar space-y-3">
      {error && (
        <ErrorMessage
          title="Ocurrió un error al cargar los turnos:"
          message={error}
        />
      )}

      <div className="relative">
        {loading && (
          <div className="absolute inset-0 z-10 overflow-hidden rounded-xl border border-gray-200 bg-white">
            <div className="border-b border-gray-200 p-4">
              <div className="h-6 w-48 animate-pulse rounded bg-gray-200" />
            </div>
            <div className="grid grid-cols-7 gap-px bg-gray-200">
              {Array.from({ length: 35 }).map((_, index) => (
                <div key={index} className="h-28 bg-white p-2">
                  <div className="mb-2 h-3 w-8 animate-pulse rounded bg-gray-200" />
                  <div className="space-y-1">
                    <div className="h-4 w-full animate-pulse rounded bg-gray-200" />
                    <div className="h-4 w-3/4 animate-pulse rounded bg-gray-200" />
                  </div>
                </div>
              ))}
            </div>
          </div>
        )}
        <div className="mis-turnos-calendar-scroll">
          <FullCalendar
            locale="es"
            buttonText={{
              today: "Hoy",
              month: "Mes",
              week: "Semana",
              day: "Día",
            }}
            dayHeaderFormat={{
              weekday: "short",
              day: "numeric",
            }}
            plugins={[dayGridPlugin, timeGridPlugin, interactionPlugin]}
            initialView="timeGridWeek"
            editable={false}
            selectable={true}
            events={loadAppointments}
            eventContent={(eventInfo) => {
              const clientName = eventInfo.event.extendedProps.clientName
              return (
                <div className="h-full overflow-hidden p-1 leading-tight">
                  {clientName && (
                    <div className="font-semibold break-words text-sm md:text-xl">
                      {clientName}
                    </div>
                  )}
                  <div className="break-words text-xs md:text-lg">
                    {eventInfo.event.title}
                  </div>
                </div>
              )
            }}
            eventMouseEnter={(mouseEnterInfo) => showTooltip(mouseEnterInfo)}
            eventMouseLeave={hideHoverTooltip}
            eventClick={(clickInfo) => showTooltip(clickInfo, true)}
            businessHours={businessHours}
            slotMinTime={slotMinTime}
            slotMaxTime={slotMaxTime}
            allDaySlot={false}
            nowIndicator={true}
            height="auto"
          />
        </div>
      </div>

      {tooltip && (
        <div
          role="dialog"
          className={`appointment-tooltip ${
            tooltip.pinned ? "appointment-tooltip-pinned" : ""
          }`}
          style={{
            left: tooltip.x,
            top: tooltip.y,
          }}
          onMouseEnter={() => {
            setTooltip((currentTooltip) =>
              currentTooltip
                ? {
                    ...currentTooltip,
                    pinned: true,
                  }
                : null,
            )
          }}
        >
          <div className="flex items-start justify-between gap-3">
            <div className="text-lg">
              <p className="font-semibold uppercase text-primary-orange">
                Turno
              </p>
              <h3 className="font-bold text-dark-blue">
                {getAppointmentTitle(tooltip.appointment)}
              </h3>
            </div>
            {tooltip.pinned && (
              <button
                type="button"
                className="rounded px-2 text-4xl leading-none text-gray-500 transition hover:bg-gray-100 hover:text-dark-blue"
                onClick={() => setTooltip(null)}
                aria-label="Cerrar detalle del turno"
              >
                ×
              </button>
            )}
          </div>

          <dl className="mt-3 space-y-2 text-md">
            <div>
              <dt className="font-semibold text-gray-500">Fecha y hora</dt>
              <dd className="text-dark-blue">
                {formatAppointmentDateRange(tooltip.appointment)}
              </dd>
            </div>
            {/* <div>
              <dt className="font-semibold text-gray-500">Estado</dt>
              <dd className="text-dark-blue">{tooltip.appointment.status}</dd>
            </div> */}
            {tooltip.appointment.items.length > 0 && (
              <div>
                <dt className="font-semibold text-gray-500">Servicios</dt>
                <dd className="space-y-1 text-dark-blue">
                  {tooltip.appointment.items.map((item) => (
                    <p key={`${item.service.externalId}-${item.startDateTime}`}>
                      {item.service.name}
                      {item.professional?.name
                        ? ` con ${item.professional.name}`
                        : ""}
                    </p>
                  ))}
                </dd>
              </div>
            )}
            {tooltip.appointment.client && (
              <div>
                <dt className="font-semibold text-gray-500">Cliente</dt>
                <dd className="text-dark-blue">
                  {tooltip.appointment.client.name} (
                  {tooltip.appointment.client.email})
                </dd>
              </div>
            )}
            {tooltip.appointment.notes && (
              <div>
                <dt className="font-semibold text-gray-500">Notas</dt>
                <dd className="text-dark-blue">{tooltip.appointment.notes}</dd>
              </div>
            )}
            <div>
              <dt className="font-semibold text-gray-500">Código</dt>
              <dd className="text-dark-blue">
                {tooltip.appointment.reservationCode}
              </dd>
            </div>
          </dl>
        </div>
      )}
    </div>
  )
}
