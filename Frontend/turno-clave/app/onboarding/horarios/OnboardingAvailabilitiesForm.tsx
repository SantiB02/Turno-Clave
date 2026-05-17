"use client"

import { useRouter } from "next/navigation"
import { useEffect, useState } from "react"
import AvailabilityEditor from "@/app/components/AvailabilityEditor"
import ErrorMessage from "@/app/components/ErrorMessage"
import {
  createDefaultWeekAvailability,
  hasInvalidAvailabilityRanges,
  hasNoEnabledAvailabilities,
  mapWeekToCreateBusinessAvailabilities,
} from "@/lib/businessAvailability"
import { createBusiness } from "@/services/businessService"
import type {
  CreateBusinessDTO,
  ShiftKey,
  WeekAvailability,
} from "@/types/business"
import NextStepButton from "../NextStepButton"

export default function OnboardingAvailabilitiesForm() {
  const router = useRouter()
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [availabilities, setAvailabilities] = useState<WeekAvailability>(
    createDefaultWeekAvailability(),
  )

  useEffect(() => {
    const storedData = localStorage.getItem("onboardingData")
    if (!storedData) {
      router.replace("/onboarding")
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

  const toggleShift = (day: keyof WeekAvailability, shift: ShiftKey) => {
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
    shift: ShiftKey,
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
    hasNoEnabledAvailabilities(availabilities) ||
    hasInvalidAvailabilityRanges(availabilities)

  const handleSubmit = async (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault()
    setLoading(true)
    const onboardingData: CreateBusinessDTO = JSON.parse(
      localStorage.getItem("onboardingData") || "{}",
    )

    if (!onboardingData) {
      router.push("/onboarding")
      return
    }

    const creationData: CreateBusinessDTO = {
      ...onboardingData,
      availabilities: mapWeekToCreateBusinessAvailabilities(availabilities),
    }

    const result = await createBusiness(creationData)

    if (!result.ok) {
      setLoading(false)
      setError(result.message)
      return
    }

    localStorage.removeItem("onboardingData")
    router.push("/onboarding/listo")
  }

  return (
    <div className="mb-30">
      {error && (
        <ErrorMessage
          title="Ocurrió un error al crear el negocio:"
          message={error}
        />
      )}
      <form onSubmit={handleSubmit}>
        <div>
          <AvailabilityEditor
            availabilities={availabilities}
            onToggleDay={toggleDay}
            onToggleShift={toggleShift}
            onUpdateTime={updateTime}
          />

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
