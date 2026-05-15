"use client"

import { DAYS } from "@/lib/availabilityLabels"
import type { ShiftKey, WeekAvailability } from "@/types/business"

const SHIFTS = {
  morning: { label: "Horario 1" },
  afternoon: { label: "Horario 2" },
} as const

type AvailabilityEditorProps = {
  availabilities: WeekAvailability
  onToggleDay: (day: keyof WeekAvailability) => void
  onToggleShift: (day: keyof WeekAvailability, shift: ShiftKey) => void
  onUpdateTime: (
    day: keyof WeekAvailability,
    shift: ShiftKey,
    field: "start" | "end",
    value: string,
  ) => void
}

export default function AvailabilityEditor({
  availabilities,
  onToggleDay,
  onToggleShift,
  onUpdateTime,
}: AvailabilityEditorProps) {
  return (
    <div className="grid grid-cols-1 justify-items-center gap-4 lg:grid-cols-2 xl:grid-cols-3">
      {DAYS.map(({ key, label }) => {
        const day = availabilities[key]

        return (
          <div
            key={key}
            className="unselectable min-h-full w-full max-w-xl rounded-3xl border-2 border-primary-orange px-4 py-3"
          >
            <label htmlFor={key} className="flex cursor-pointer items-center">
              <input
                type="checkbox"
                id={key}
                className="peer sr-only"
                checked={day.enabled}
                onChange={() => onToggleDay(key as keyof WeekAvailability)}
              />

              <span
                className="relative flex h-5 w-5 items-center justify-center rounded-full border-2 border-primary-orange
                peer-checked:border-primary-orange peer-checked:bg-primary-orange
                after:absolute after:inset-0 after:flex after:items-center after:justify-center
                after:text-xs after:text-white after:opacity-0 after:content-['✓']
                peer-checked:after:opacity-100"
              />
              <span className="ml-5 mr-2 md:mr-4">{label}</span>
            </label>

            <div className="mt-3 space-y-2">
              {(
                Object.entries(SHIFTS) as Array<
                  [ShiftKey, (typeof SHIFTS)[ShiftKey]]
                >
              ).map(([shiftKey, shift]) => {
                const shiftAvailability = day[shiftKey]

                return (
                  <div
                    key={shiftKey}
                    className="flex w-full items-center gap-2"
                  >
                    <button
                      type="button"
                      onClick={() =>
                        onToggleShift(key as keyof WeekAvailability, shiftKey)
                      }
                      disabled={!day.enabled}
                      className={`w-20 shrink-0 cursor-pointer rounded-full border px-2 py-1 text-sm transition disabled:opacity-40 ${
                        shiftAvailability.enabled
                          ? "border-primary-orange bg-primary-orange text-white"
                          : "border-primary-orange text-primary-orange"
                      }`}
                    >
                      {shift.label}
                    </button>

                    <input
                      type="time"
                      value={shiftAvailability.start}
                      onChange={(e) =>
                        onUpdateTime(
                          key as keyof WeekAvailability,
                          shiftKey,
                          "start",
                          e.target.value,
                        )
                      }
                      disabled={!day.enabled || !shiftAvailability.enabled}
                      className="min-w-0 flex-1 rounded-full border px-2 py-1 text-sm disabled:opacity-40"
                    />

                    <span className="shrink-0 text-sm font-bold text-gray-500">
                      -
                    </span>

                    <input
                      type="time"
                      value={shiftAvailability.end}
                      onChange={(e) =>
                        onUpdateTime(
                          key as keyof WeekAvailability,
                          shiftKey,
                          "end",
                          e.target.value,
                        )
                      }
                      disabled={!day.enabled || !shiftAvailability.enabled}
                      className="min-w-0 flex-1 rounded-full border px-2 py-1 text-sm disabled:opacity-40"
                    />
                  </div>
                )
              })}
            </div>
          </div>
        )
      })}
    </div>
  )
}
