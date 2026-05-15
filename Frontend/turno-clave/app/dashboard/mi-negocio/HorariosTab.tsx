"use client"

import { ExclamationTriangleIcon } from "@heroicons/react/20/solid"
import { useRouter, useSearchParams } from "next/navigation"
import { useEffect, useState } from "react"
import AvailabilityEditor from "@/app/components/AvailabilityEditor"
import ErrorMessage from "@/app/components/ErrorMessage"
import {
  hasAvailabilitiesWithoutShift,
  hasInvalidAvailabilityRanges,
  hasNoEnabledAvailabilities,
  mapBusinessAvailabilitiesToWeek,
  mapWeekToCreateBusinessAvailabilities,
} from "@/lib/businessAvailability"
import { updateBusinessAvailabilities } from "@/services/businessAvailabilityService"
import { updateProfessionalAvailabilities } from "@/services/professionalAvailabilityService"
import type {
  BusinessDetail,
  ShiftKey,
  WeekAvailability,
} from "@/types/business"
import type { Professional } from "@/types/professional"

type HorariosTabProps = {
  business: BusinessDetail
  professionals: Professional[]
  setProfessionals: React.Dispatch<React.SetStateAction<Professional[]>>
  professionalsError: string | null
}

const SKELETON_DAY_KEYS = [
  "monday",
  "tuesday",
  "wednesday",
  "thursday",
  "friday",
  "saturday",
] as const

const SKELETON_SHIFT_KEYS = ["first", "second"] as const

function HorariosTabSkeleton() {
  return (
    <div className="max-w-6xl animate-pulse">
      <div className="flex items-center gap-2 px-4 py-2 rounded">
        <div className="h-5 w-24 rounded bg-gray-200" />
        <div className="h-9 w-48 rounded border border-gray-200 bg-gray-100" />
      </div>

      <div className="mt-1 flex items-center gap-2">
        <div className="h-6 w-6 rounded bg-yellow-100" />
        <div className="h-5 w-80 max-w-full rounded bg-gray-200" />
      </div>

      <div className="mt-4 grid grid-cols-1 justify-items-center gap-4 lg:grid-cols-2 xl:grid-cols-3">
        {SKELETON_DAY_KEYS.map((dayKey) => (
          <div
            key={dayKey}
            className="min-h-full w-full max-w-xl rounded-3xl border-2 border-primary-orange/30 px-4 py-3"
          >
            <div className="flex items-center">
              <div className="h-5 w-5 rounded-full bg-gray-200" />
              <div className="ml-5 h-5 w-28 rounded bg-gray-200" />
            </div>

            <div className="mt-3 space-y-2">
              {SKELETON_SHIFT_KEYS.map((shiftKey) => (
                <div key={shiftKey} className="flex w-full items-center gap-2">
                  <div className="h-8 w-20 shrink-0 rounded-full bg-gray-200" />
                  <div className="h-8 min-w-0 flex-1 rounded-full bg-gray-100" />
                  <div className="h-4 w-3 shrink-0 rounded bg-gray-200" />
                  <div className="h-8 min-w-0 flex-1 rounded-full bg-gray-100" />
                </div>
              ))}
            </div>
          </div>
        ))}
      </div>
    </div>
  )
}

export default function HorariosTab({
  business,
  professionals,
  setProfessionals,
  professionalsError,
}: HorariosTabProps) {
  const router = useRouter()
  const searchParams = useSearchParams()

  const selectedEntity = searchParams.get("professional") ?? "business"

  const professional =
    selectedEntity === "business"
      ? null
      : professionals.find((p) => p.externalId === selectedEntity)

  const isLoadingProfessional = selectedEntity !== "business" && !professional

  const [loading, setLoading] = useState<boolean>(false)
  const [error, setError] = useState<string | null>(null)
  const [savedAvailabilities, setSavedAvailabilities] =
    useState<WeekAvailability>(
      mapBusinessAvailabilitiesToWeek(business?.availabilities ?? []),
    )

  const [availabilities, setAvailabilities] =
    useState<WeekAvailability>(savedAvailabilities)

  useEffect(() => {
    function getSelectedAvailabilities(): WeekAvailability {
      if (selectedEntity === "business") {
        return mapBusinessAvailabilitiesToWeek(business.availabilities)
      }

      const professional = professionals.find(
        (p) => p.externalId === selectedEntity,
      )

      return mapBusinessAvailabilitiesToWeek(professional?.availabilities ?? [])
    }
    const mapped = getSelectedAvailabilities()

    setAvailabilities(mapped)
    setSavedAvailabilities(mapped)
  }, [selectedEntity, business.availabilities, professionals])

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

  const handleOnChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const value = e.target.value

    setError(null)

    const params = new URLSearchParams(searchParams.toString())

    params.set("tab", "horarios")

    if (value === "business") {
      params.delete("professional")
    } else {
      params.set("professional", value)
    }

    router.replace(`?${params.toString()}`)
  }

  const handleSubmit = async (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault()

    setLoading(true)
    setError(null)

    try {
      const payload = {
        availabilities: mapWeekToCreateBusinessAvailabilities(availabilities),
      }

      if (selectedEntity === "business") {
        const result = await updateBusinessAvailabilities(
          business.externalId,
          payload,
        )

        if (!result.ok) {
          setError(result.message)
          return
        }

        setSavedAvailabilities(availabilities)
        router.refresh()
        return
      }

      const result = await updateProfessionalAvailabilities(
        selectedEntity,
        payload,
      )

      if (!result.ok) {
        setError(result.message)
        return
      }

      setProfessionals((prev) =>
        prev.map((p) =>
          p.externalId === selectedEntity
            ? {
                ...p,
                availabilities: result.data,
              }
            : p,
        ),
      )

      setSavedAvailabilities(availabilities)
      router.refresh()
    } catch (err) {
      setError(err instanceof Error ? err.message : "Error inesperado")
    } finally {
      setLoading(false)
    }
  }

  if (isLoadingProfessional) {
    return <HorariosTabSkeleton />
  }

  return (
    <div className="max-w-6xl">
      <div className="flex items-center gap-2 px-4 py-2 rounded">
        <p>Horarios de:</p>
        <select
          value={selectedEntity}
          onChange={(e) => handleOnChange(e)}
          className="border border-primary-orange focus:outline-primary-orange focus:ring-primary-orange p-1 rounded"
        >
          <option value="business">Negocio</option>

          {professionals.map((professional) => (
            <option
              key={professional.externalId}
              value={professional.externalId}
            >
              {professional.name}
            </option>
          ))}
        </select>
      </div>
      {professionalsError && (
        <ErrorMessage
          title="Ocurrió un error al cargar los profesionales:"
          message={professionalsError}
        />
      )}

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
          <ErrorMessage
            title="Ocurrió un error al cargar los horarios:"
            message={error}
          />
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
