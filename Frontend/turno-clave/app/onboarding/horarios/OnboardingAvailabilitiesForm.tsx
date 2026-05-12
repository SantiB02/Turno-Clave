"use client"

import { useRouter } from "next/navigation"
import { useEffect, useState } from "react"
import { createBusiness } from "@/services/businessService"
import type {
  CreateBusinessAvailabilityDTO,
  CreateBusinessDTO,
  WeekAvailability,
} from "@/types/business"
import NextStepButton from "../NextStepButton"

export default function OnboardingAvailabilitiesForm() {
  const router = useRouter()
  const [loading, setLoading] = useState(false)
  const [availabilities, setAvailabilities] = useState<WeekAvailability>({
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
  })

  const days = [
    { key: "monday", label: "Lunes" },
    { key: "tuesday", label: "Martes" },
    { key: "wednesday", label: "Miércoles" },
    { key: "thursday", label: "Jueves" },
    { key: "friday", label: "Viernes" },
    { key: "saturday", label: "Sábado" },
    { key: "sunday", label: "Domingo" },
  ] as const

  const shifts = {
    morning: { label: "Mañana", start: "08:00", end: "12:00" },
    afternoon: { label: "Tarde", start: "14:00", end: "18:00" },
  } as const

  useEffect(() => {
    const storedData = localStorage.getItem("onboardingData")
    if (!storedData) {
      router.replace("/onboarding/negocio")
    }
  }, [router])

  const toggleDay = (day: keyof WeekAvailability) => {
    setAvailabilities((prev) => ({
      ...prev,
      [day]: {
        ...prev[day],
        enabled: !prev[day].enabled,
      },
    }))
  }

  const toggleShift = (
    day: keyof WeekAvailability,
    shift: keyof typeof shifts,
  ) => {
    setAvailabilities((prev) => ({
      ...prev,
      [day]: {
        ...prev[day],
        enabled: true,
        [shift]: {
          ...prev[day][shift],
          enabled: !prev[day][shift].enabled,
        },
      },
    }))
  }

  const updateTime = (
    day: keyof WeekAvailability,
    shift: keyof typeof shifts,
    field: "start" | "end",
    value: string,
  ) => {
    setAvailabilities((prev) => ({
      ...prev,
      [day]: {
        ...prev[day],
        [shift]: {
          ...prev[day][shift],
          [field]: value,
        },
      },
    }))
  }

  const isSubmitDisabled =
    !Object.values(availabilities).some(
      (day) => day.enabled && (day.morning.enabled || day.afternoon.enabled),
    ) ||
    Object.values(availabilities).some(
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

  const dayToNumber: Record<string, number> = {
    sunday: 0,
    monday: 1,
    tuesday: 2,
    wednesday: 3,
    thursday: 4,
    friday: 5,
    saturday: 6,
  }

  const handleSubmit = async (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault()
    setLoading(true)
    const onboardingData: CreateBusinessDTO = JSON.parse(
      localStorage.getItem("onboardingData") || "{}",
    )

    if (!onboardingData) {
      router.push("/onboarding/negocio")
    }

    onboardingData.availabilities = []

    const result: CreateBusinessAvailabilityDTO[] = Object.entries(
      availabilities,
    ).flatMap(([day, value]) => {
      if (!value.enabled) return []

      return (["morning", "afternoon"] as const)
        .filter((shift) => value[shift].enabled)
        .map((shift) => ({
          day: dayToNumber[day],
          startTime: value[shift].start,
          endTime: value[shift].end,
        }))
    })

    const creationData: CreateBusinessDTO = {
      ...onboardingData,
      availabilities: result,
    }

    await createBusiness(creationData)

    localStorage.removeItem("onboardingData")
    router.push("/onboarding/listo")
  }

  return (
    <div className="mb-30">
      <form onSubmit={handleSubmit}>
        <div>
          <div className="space-y-4">
            {days.map(({ key, label }) => {
              const day = availabilities[key]

              return (
                <div
                  key={key}
                  className="unselectable rounded-3xl border-2 border-primary-orange px-4 py-3"
                >
                  <label
                    htmlFor={key}
                    className="flex items-center cursor-pointer"
                  >
                    <input
                      type="checkbox"
                      id={key}
                      className="sr-only peer"
                      checked={day.enabled}
                      onChange={() => toggleDay(key)}
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
                      Object.entries(shifts) as Array<
                        [
                          keyof typeof shifts,
                          (typeof shifts)[keyof typeof shifts],
                        ]
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
                            onClick={() => toggleShift(key, shiftKey)}
                            disabled={!day.enabled}
                            className={`w-20 shrink-0 rounded-full border px-2 py-1 text-sm transition disabled:opacity-40 ${
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
                              updateTime(key, shiftKey, "start", e.target.value)
                            }
                            disabled={
                              !day.enabled || !shiftAvailability.enabled
                            }
                            className="min-w-0 flex-1 rounded-full border px-2 py-1 text-sm disabled:opacity-40"
                          />

                          <span className="shrink-0 text-sm text-gray-500">
                            -
                          </span>

                          <input
                            type="time"
                            value={shiftAvailability.end}
                            onChange={(e) =>
                              updateTime(key, shiftKey, "end", e.target.value)
                            }
                            disabled={
                              !day.enabled || !shiftAvailability.enabled
                            }
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

          <div className="flex justify-center">
            <NextStepButton
              type="submit"
              className="mt-6"
              disabled={isSubmitDisabled}
              loading={loading}
            />
          </div>
        </div>
      </form>
    </div>
  )
}
