"use client"

import { create } from "domain"
import { useRouter } from "next/navigation"
import { useEffect, useState } from "react"
import { createBusiness } from "@/services/businessService"
import type {
  BusinessAvailability,
  CreateBusinessDTO,
  WeekAvailability,
} from "@/types/business"
import NextStepButton from "../NextStepButton"

export default function OnboardingAvailabilitiesForm() {
  const router = useRouter()
  const [availabilities, setAvailabilities] = useState<WeekAvailability>({
    monday: { enabled: false, start: "", end: "" },
    tuesday: { enabled: false, start: "", end: "" },
    wednesday: { enabled: false, start: "", end: "" },
    thursday: { enabled: false, start: "", end: "" },
    friday: { enabled: false, start: "", end: "" },
    saturday: { enabled: false, start: "", end: "" },
    sunday: { enabled: false, start: "", end: "" },
  })

  const days = [
    { key: "monday", label: "Lunes" },
    { key: "tuesday", label: "Martes" },
    { key: "wednesday", label: "Miércoles" },
    { key: "thursday", label: "Jueves" },
    { key: "friday", label: "Viernes" },
    { key: "saturday", label: "Sábado" },
    { key: "sunday", label: "Domingo" },
  ]

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

  const updateTime = (
    day: keyof WeekAvailability,
    field: "start" | "end",
    value: string,
  ) => {
    setAvailabilities((prev) => ({
      ...prev,
      [day]: {
        ...prev[day],
        [field]: value,
      },
    }))
  }

  // submit is disabled if no day and time is selected
  const isSubmitDisabled =
    !Object.values(availabilities).some((day) => day.enabled) ||
    Object.values(availabilities).some(
      (day) =>
        day.enabled &&
        (day.start === "" || day.end === "" || day.end <= day.start),
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

    const onboardingData: CreateBusinessDTO = JSON.parse(
      localStorage.getItem("onboardingData") || "{}",
    )

    if (!onboardingData) {
      router.push("/onboarding/negocio")
    }

    onboardingData.availabilities = []

    const result: BusinessAvailability[] = Object.entries(availabilities)
      .filter(([_, value]) => value.enabled)
      .map(([day, value]) => ({
        dayOfWeek: dayToNumber[day],
        startTime: value.start,
        endTime: value.end,
      }))

    onboardingData.availabilities = result

    await createBusiness(onboardingData)

    localStorage.removeItem("onboardingData")
    router.push("/onboarding/listo")
  }

  return (
    <div className="mb-30">
      <form onSubmit={handleSubmit}>
        <div>
          {days.map(({ key, label }) => {
            const day = availabilities[key]

            return (
              <div
                key={key}
                className="flex mb-4 unselectable items-center justify-between py-2 px-4 mb-2 border-2 border-primary-orange rounded-3xl"
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
                    className="w-5 h-5 border-2 border-primary-orange rounded-full 
          flex items-center justify-center
          peer-checked:bg-primary-orange peer-checked:border-primary-orange
          relative
          
          after:content-['✓'] 
          after:text-white 
          after:text-xs
          after:absolute 
          after:inset-0 
          after:flex 
          after:items-center 
          after:justify-center
          after:opacity-0
          
          peer-checked:after:opacity-100"
                  />
                  <span className="ml-5 mr-2 md:mr-4">{label}</span>
                </label>
                <div className="flex items-center gap-2">
                  <input
                    type="time"
                    value={day.start}
                    onChange={(e) => updateTime(key, "start", e.target.value)}
                    disabled={!day.enabled}
                    className="px-2 py-1 border rounded-full text-sm disabled:opacity-40"
                  />

                  <span className="text-sm text-gray-500">-</span>

                  <input
                    type="time"
                    value={day.end}
                    onChange={(e) => updateTime(key, "end", e.target.value)}
                    disabled={!day.enabled}
                    className="px-2 py-1 border rounded-full text-sm disabled:opacity-40"
                  />
                </div>
              </div>
            )
          })}

          <div className="flex justify-center">
            <NextStepButton
              type="submit"
              className="mt-6"
              disabled={isSubmitDisabled}
            />
          </div>
        </div>
      </form>
    </div>
  )
}
