"use client"

import { ExclamationTriangleIcon } from "@heroicons/react/20/solid"
import { useRouter } from "next/navigation"
import { useEffect, useState } from "react"
import AvailabilityEditor from "@/app/components/AvailabilityEditor"
import {
  hasAvailabilitiesWithoutShift,
  hasInvalidAvailabilityRanges,
  hasNoEnabledAvailabilities,
  mapBusinessAvailabilitiesToWeek,
  mapWeekToCreateBusinessAvailabilities,
} from "@/lib/businessAvailability"
import { updateBusinessAvailabilities } from "@/services/businessAvailabilityService"
import type {
  BusinessDetail,
  ShiftKey,
  UpdateBusinessAvailabilitiesDTO,
  WeekAvailability,
} from "@/types/business"

type HorariosTabProps = {
  business: BusinessDetail
}

export default function HorariosTab({ business }: HorariosTabProps) {
  const router = useRouter()

  const [loading, setLoading] = useState<boolean>(false)
  const [error, setError] = useState<string | null>(null)
  const [savedAvailabilities, setSavedAvailabilities] =
    useState<WeekAvailability>(
      mapBusinessAvailabilitiesToWeek(business.availabilities),
    )

  const [availabilities, setAvailabilities] =
    useState<WeekAvailability>(savedAvailabilities)

  useEffect(() => {
    const mapped = mapBusinessAvailabilitiesToWeek(business.availabilities)

    setAvailabilities(mapped)
    setSavedAvailabilities(mapped)
  }, [business.availabilities])

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

  const hasChanges =
    JSON.stringify(availabilities) !== JSON.stringify(savedAvailabilities)

  const isSubmitDisabled =
    loading ||
    !hasChanges ||
    hasNoEnabledAvailabilities(availabilities) ||
    hasInvalidAvailabilityRanges(availabilities) ||
    hasAvailabilitiesWithoutShift(availabilities)

  const handleSubmit = async (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault()
    setLoading(true)
    setError(null)

    try {
      const data: UpdateBusinessAvailabilitiesDTO = {
        availabilities: mapWeekToCreateBusinessAvailabilities(availabilities),
      }

      await updateBusinessAvailabilities(business.externalId, data)
      router.refresh()
    } catch (submitError) {
      setError(
        submitError instanceof Error ? submitError.message : "Unknown error",
      )
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="max-w-6xl">
      <div className="mt-1 flex underline">
        <ExclamationTriangleIcon className="mr-1 h-6 w-6 text-yellow-400" />
        <p>Estos horarios se mostrarán a tus clientes para reservar turnos.</p>
      </div>

      <form onSubmit={handleSubmit} className="mt-4 space-y-6">
        <AvailabilityEditor
          availabilities={availabilities}
          onToggleDay={toggleDay}
          onToggleShift={toggleShift}
          onUpdateTime={updateTime}
        />

        {error && (
          <div className="rounded border border-red-400 bg-red-200 px-4 py-3 text-red-700">
            <p>Ocurrió un error al editar los horarios:</p>
            <p>{error}</p>
          </div>
        )}

        <div className="flex justify-end">
          <button
            type="submit"
            className={`rounded-lg px-4 py-2 font-medium text-white transition ${
              isSubmitDisabled
                ? "bg-orange-300"
                : "cursor-pointer bg-orange-500 hover:bg-orange-600"
            }`}
            disabled={isSubmitDisabled}
          >
            {loading ? "Guardando cambios..." : "Guardar horarios"}
          </button>
        </div>
      </form>
    </div>
  )
}
