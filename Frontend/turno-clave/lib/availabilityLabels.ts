export type AvailabilityDayKey =
  | "monday"
  | "tuesday"
  | "wednesday"
  | "thursday"
  | "friday"
  | "saturday"
  | "sunday"

export const DAYS: Array<{ key: AvailabilityDayKey; label: string }> = [
  { key: "monday", label: "Lunes" },
  { key: "tuesday", label: "Martes" },
  { key: "wednesday", label: "Miércoles" },
  { key: "thursday", label: "Jueves" },
  { key: "friday", label: "Viernes" },
  { key: "saturday", label: "Sábado" },
  { key: "sunday", label: "Domingo" },
]

const DAY_LABELS_BY_KEY: Record<string, string> = {
  sunday: "Domingo",
  domingo: "Domingo",
  monday: "Lunes",
  lunes: "Lunes",
  tuesday: "Martes",
  martes: "Martes",
  wednesday: "Miércoles",
  miercoles: "Miércoles",
  miércoles: "Miércoles",
  thursday: "Jueves",
  jueves: "Jueves",
  friday: "Viernes",
  viernes: "Viernes",
  saturday: "Sábado",
  sabado: "Sábado",
  sábado: "Sábado",
}

const DAY_LABELS_BY_NUMBER: Record<number, string> = {
  0: "Domingo",
  1: "Lunes",
  2: "Martes",
  3: "Miércoles",
  4: "Jueves",
  5: "Viernes",
  6: "Sábado",
}

export function getDayLabel(dayOfWeek: number | string) {
  if (typeof dayOfWeek === "number") {
    return DAY_LABELS_BY_NUMBER[dayOfWeek] ?? `Día ${dayOfWeek}`
  }

  const normalizedDay = dayOfWeek.trim().toLowerCase()
  const numericDay = Number(normalizedDay)

  if (!Number.isNaN(numericDay)) {
    return DAY_LABELS_BY_NUMBER[numericDay] ?? `Día ${dayOfWeek}`
  }

  return DAY_LABELS_BY_KEY[normalizedDay] ?? dayOfWeek
}

export function formatAvailabilityTime(time: string) {
  return time.slice(0, 5)
}

export function getDayOrder(dayOfWeek: number | string) {
  if (typeof dayOfWeek === "number") {
    return dayOfWeek >= 0 && dayOfWeek <= 6
      ? dayOfWeek
      : Number.MAX_SAFE_INTEGER
  }

  const normalizedDay = dayOfWeek.trim().toLowerCase()
  const numericDay = Number(normalizedDay)

  if (!Number.isNaN(numericDay)) {
    return numericDay >= 0 && numericDay <= 6
      ? numericDay
      : Number.MAX_SAFE_INTEGER
  }

  const dayIndex = DAYS.findIndex(({ key, label }) => {
    const normalizedLabel = label.toLowerCase()

    return (
      key === normalizedDay ||
      normalizedLabel === normalizedDay ||
      normalizedLabel.normalize("NFD").replace(/[\u0300-\u036f]/g, "") ===
        normalizedDay
    )
  })

  return dayIndex === -1 ? Number.MAX_SAFE_INTEGER : dayIndex + 1
}
