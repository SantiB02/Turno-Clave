import type {
  BusinessAvailabilityDTO,
  ShiftKey,
  UpdateBusinessAvailabilityDTO,
  WeekAvailability,
} from "@/types/business"
import type { UpdateProfessionalAvailabilityDTO } from "@/types/professional"

type DayKey = keyof WeekAvailability

export const dayToNumber: Record<keyof WeekAvailability, number> = {
  sunday: 0,
  monday: 1,
  tuesday: 2,
  wednesday: 3,
  thursday: 4,
  friday: 5,
  saturday: 6,
}

const numberToDay: Record<number, DayKey> = {
  0: "sunday",
  1: "monday",
  2: "tuesday",
  3: "wednesday",
  4: "thursday",
  5: "friday",
  6: "saturday",
}

const stringToDay: Record<string, DayKey> = {
  sunday: "sunday",
  domingo: "sunday",
  monday: "monday",
  lunes: "monday",
  tuesday: "tuesday",
  martes: "tuesday",
  wednesday: "wednesday",
  miercoles: "wednesday",
  miércoles: "wednesday",
  thursday: "thursday",
  jueves: "thursday",
  friday: "friday",
  viernes: "friday",
  saturday: "saturday",
  sabado: "saturday",
  sábado: "saturday",
}

function normalizeDayKey(
  day: BusinessAvailabilityDTO["dayOfWeek"],
): DayKey | null {
  if (typeof day === "number") {
    return numberToDay[day] ?? null
  }

  if (typeof day === "string") {
    const numericDay = Number(day)

    if (!Number.isNaN(numericDay)) {
      return numberToDay[numericDay] ?? null
    }

    return stringToDay[day.trim().toLowerCase()] ?? null
  }

  return null
}

export function createDefaultWeekAvailability(): WeekAvailability {
  return {
    monday: {
      enabled: false,
      morning: { enabled: false, start: "08:00", end: "12:00" },
      afternoon: { enabled: false, start: "14:00", end: "18:00" },
    },
    tuesday: {
      enabled: false,
      morning: { enabled: false, start: "08:00", end: "12:00" },
      afternoon: { enabled: false, start: "14:00", end: "18:00" },
    },
    wednesday: {
      enabled: false,
      morning: { enabled: false, start: "08:00", end: "12:00" },
      afternoon: { enabled: false, start: "14:00", end: "18:00" },
    },
    thursday: {
      enabled: false,
      morning: { enabled: false, start: "08:00", end: "12:00" },
      afternoon: { enabled: false, start: "14:00", end: "18:00" },
    },
    friday: {
      enabled: false,
      morning: { enabled: false, start: "08:00", end: "12:00" },
      afternoon: { enabled: false, start: "14:00", end: "18:00" },
    },
    saturday: {
      enabled: false,
      morning: { enabled: false, start: "08:00", end: "12:00" },
      afternoon: { enabled: false, start: "14:00", end: "18:00" },
    },
    sunday: {
      enabled: false,
      morning: { enabled: false, start: "08:00", end: "12:00" },
      afternoon: { enabled: false, start: "14:00", end: "18:00" },
    },
  }
}

export function mapBusinessAvailabilitiesToWeek(
  availabilities: BusinessAvailabilityDTO[],
): WeekAvailability {
  const weekAvailability = createDefaultWeekAvailability()

  const groupedAvailabilities = availabilities.reduce<
    Partial<Record<keyof WeekAvailability, BusinessAvailabilityDTO[]>>
  >((acc, availability) => {
    const dayKey = normalizeDayKey(availability.dayOfWeek)

    if (!dayKey) {
      return acc
    }

    acc[dayKey] = [...(acc[dayKey] ?? []), availability].sort((a, b) =>
      a.startTime.localeCompare(b.startTime),
    )

    return acc
  }, {})

  Object.entries(groupedAvailabilities).forEach(
    ([dayKey, dayAvailabilities]) => {
      if (!dayAvailabilities || dayAvailabilities.length === 0) return

      const typedDayKey = dayKey as keyof WeekAvailability
      const shifts: ShiftKey[] = ["morning", "afternoon"]

      weekAvailability[typedDayKey].enabled = true

      dayAvailabilities
        .slice(0, shifts.length)
        .forEach((availability, index) => {
          const shift = shifts[index]

          weekAvailability[typedDayKey][shift] = {
            enabled: true,
            start: availability.startTime.slice(0, 5),
            end: availability.endTime.slice(0, 5),
          }
        })
    },
  )

  return weekAvailability
}

export function mapWeekToCreateBusinessAvailabilities(
  availabilities: WeekAvailability,
): UpdateBusinessAvailabilityDTO[] | UpdateProfessionalAvailabilityDTO[] {
  return (
    Object.entries(availabilities) as Array<
      [keyof WeekAvailability, WeekAvailability[keyof WeekAvailability]]
    >
  ).flatMap(([day, value]) => {
    if (!value.enabled) return []

    return (["morning", "afternoon"] as const)
      .filter((shift) => value[shift].enabled)
      .map((shift) => ({
        dayOfWeek: dayToNumber[day],
        startTime: value[shift].start,
        endTime: value[shift].end,
      }))
  })
}

export function hasNoEnabledAvailabilities(availabilities: WeekAvailability) {
  return !Object.values(availabilities).some(
    (day) => day.enabled && (day.morning.enabled || day.afternoon.enabled),
  )
}

export function hasInvalidAvailabilityRanges(availabilities: WeekAvailability) {
  return Object.values(availabilities).some(
    (day) =>
      day.enabled &&
      ((day.morning.enabled &&
        (day.morning.start === "" ||
          day.morning.end === "" ||
          day.morning.end <= day.morning.start)) ||
        (day.afternoon.enabled &&
          (day.afternoon.start === "" ||
            day.afternoon.end === "" ||
            day.afternoon.end <= day.afternoon.start))),
  )
}

export function hasAvailabilitiesWithoutShift(
  availabilities: WeekAvailability,
) {
  return Object.values(availabilities).some(
    (day) => day.enabled && !day.morning.enabled && !day.afternoon.enabled,
  )
}
