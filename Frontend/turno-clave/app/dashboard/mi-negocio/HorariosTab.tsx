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
import { updateProfessionalAvailabilities } from "@/services/professionalAvailabilityService"
import { getProfessionalsByActiveBusiness } from "@/services/professionalService"
import type {
  BusinessDetail,
  ShiftKey,
  WeekAvailability,
} from "@/types/business"
import type { Professional } from "@/types/professional"

type HorariosTabProps = {
  business: BusinessDetail
}

export default function HorariosTab({ business }: HorariosTabProps) {
  const router = useRouter()

  const [professionals, setProfessionals] = useState<Professional[]>([])
  const [selectedEntity, setSelectedEntity] = useState<string>("business")
  const [loading, setLoading] = useState<boolean>(false)
  const [error, setError] = useState<string | null>(null)
  const [savedAvailabilities, setSavedAvailabilities] =
    useState<WeekAvailability>(
      mapBusinessAvailabilitiesToWeek(business.availabilities),
    )

  const [availabilities, setAvailabilities] =
    useState<WeekAvailability>(savedAvailabilities)

  useEffect(() => {
    async function loadProfessionals() {
      const professionals: Professional[] =
        await getProfessionalsByActiveBusiness()
      setProfessionals(professionals)
    }
    loadProfessionals()
  }, [])

  useEffect(() => {
    function getSelectedAvailabilities(): WeekAvailability {
      if (selectedEntity === "business") {
        return mapBusinessAvailabilitiesToWeek(business.availabilities)
      }

      const professional = professionals.find(
        (p) => p.externalId === selectedEntity,
      )

      console.log("PROFESSIONAL AVAILABILITIES:", professional?.availabilities)

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

  const handleSubmit = async (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault()

    setLoading(true)
    setError(null)

    try {
      const updatedAvailabilities =
        mapWeekToCreateBusinessAvailabilities(availabilities)

      const data = {
        availabilities: updatedAvailabilities,
      }

      if (selectedEntity === "business") {
        await updateBusinessAvailabilities(business.externalId, data)
      } else {
        const updatedAvailabilities = await updateProfessionalAvailabilities(
          selectedEntity,
          data,
        )

        setProfessionals((prev) =>
          prev.map((professional) =>
            professional.externalId === selectedEntity
              ? {
                  ...professional,
                  availabilities: updatedAvailabilities,
                }
              : professional,
          ),
        )
      }

      setSavedAvailabilities(availabilities)

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
      <div className="flex items-center gap-2 px-4 py-2 rounded">
        <p>Horarios de:</p>
        <select
          value={selectedEntity}
          onChange={(e) => setSelectedEntity(e.target.value)}
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
