"use client"

import Link from "next/link"
import { useRouter } from "next/navigation"
import { useState } from "react"
import Accordion from "@/app/components/Accordion"
import AddItemButton from "@/app/components/AddItemButton"
import Button from "@/app/components/Button"
import ModalForm from "@/app/components/ModalForm"
import {
  formatAvailabilityTime,
  getDayLabel,
  getDayOrder,
} from "@/lib/availabilityLabels"
import {
  createProfessional,
  updateProfessional,
} from "@/services/professionalService"
import type {
  NestedProfessionalAvailability,
  Professional,
} from "@/types/professional"
import type { Service } from "@/types/service"

function groupAvailabilitiesByDay(
  availabilities: NestedProfessionalAvailability[],
) {
  const grouped = new Map<number | string, string[]>()
  const seenRanges = new Set<string>()

  availabilities
    .slice()
    .sort(
      (a, b) =>
        getDayOrder(a.dayOfWeek) - getDayOrder(b.dayOfWeek) ||
        a.startTime.localeCompare(b.startTime),
    )
    .forEach((availability) => {
      const timeRange = `${formatAvailabilityTime(availability.startTime)} - ${formatAvailabilityTime(availability.endTime)}`
      const uniqueKey = `${availability.dayOfWeek}-${timeRange}`

      if (seenRanges.has(uniqueKey)) {
        return
      }

      seenRanges.add(uniqueKey)

      grouped.set(availability.dayOfWeek, [
        ...(grouped.get(availability.dayOfWeek) ?? []),
        timeRange,
      ])
    })

  return Array.from(grouped.entries())
}

type ProfessionalModalMode = "create" | "edit-services" | null

type ProfesionalesTabProps = {
  professionals: Professional[]
  setProfessionals: React.Dispatch<React.SetStateAction<Professional[]>>
  businessServices: Service[]
  loadingProfessionals: boolean
}

export default function ProfesionalesTab({
  professionals,
  setProfessionals,
  businessServices,
  loadingProfessionals,
}: ProfesionalesTabProps) {
  const router = useRouter()

  const [modalMode, setModalMode] = useState<ProfessionalModalMode>(null)
  const [selectedProfessional, setSelectedProfessional] =
    useState<Professional | null>(null)
  const [professionalName, setProfessionalName] = useState("")
  const [selectedServiceIds, setSelectedServiceIds] = useState<string[]>([])
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const resetModalState = () => {
    setModalMode(null)
    setSelectedProfessional(null)
    setProfessionalName("")
    setSelectedServiceIds([])
    setError(null)
  }

  const openCreateModal = () => {
    setModalMode("create")
    setSelectedProfessional(null)
    setProfessionalName("")
    setSelectedServiceIds([])
    setError(null)
  }

  const openEditServicesModal = (professional: Professional) => {
    setModalMode("edit-services")
    setSelectedProfessional(professional)
    setProfessionalName(professional.name)
    setSelectedServiceIds(
      professional.services.map((service) => service.externalId),
    )
    setError(null)
  }

  const closeModal = () => {
    if (loading) return

    resetModalState()
  }

  const toggleService = (serviceExternalId: string) => {
    setSelectedServiceIds((currentIds) =>
      currentIds.includes(serviceExternalId)
        ? currentIds.filter((id) => id !== serviceExternalId)
        : [...currentIds, serviceExternalId],
    )
  }

  const handleSubmit = async (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault()
    setLoading(true)
    setError(null)

    try {
      if (modalMode === "create") {
        const createdProfessional = await createProfessional({
          name: professionalName.trim(),
          serviceExternalIds: selectedServiceIds,
        })

        setProfessionals((currentProfessionals) => [
          ...currentProfessionals,
          createdProfessional,
        ])
      }

      if (modalMode === "edit-services" && selectedProfessional) {
        const updatedProfessional = await updateProfessional(
          selectedProfessional.externalId,
          {
            name: selectedProfessional.name,
            serviceExternalIds: selectedServiceIds,
          },
        )

        setProfessionals((currentProfessionals) =>
          currentProfessionals.map((professional) =>
            professional.externalId === updatedProfessional.externalId
              ? updatedProfessional
              : professional,
          ),
        )
      }

      resetModalState()
      router.refresh()
    } catch (submitError) {
      setError(
        submitError instanceof Error ? submitError.message : "Unknown error",
      )
    } finally {
      setLoading(false)
    }
  }

  const modalTitle =
    modalMode === "create"
      ? "Agregar profesional"
      : `Servicios de ${selectedProfessional?.name ?? ""}`

  const submitLabel =
    modalMode === "create" ? "Crear profesional" : "Guardar servicios"

  const loadingLabel =
    modalMode === "create" ? "Creando profesional..." : "Guardando servicios..."

  const isSubmitDisabled =
    loading ||
    (modalMode === "create" && selectedServiceIds.length === 0) ||
    (modalMode === "create" && professionalName.trim().length < 3)

  return (
    <div>
      <h1 className="mb-6 text-xl underline">Profesionales</h1>
      {loadingProfessionals ? (
        <div>Cargando profesionales...</div>
      ) : (
        <div>
          {professionals.length === 0 ? (
            <p className="text-gray-500">
              No tienes profesionales registrados.
            </p>
          ) : (
            <div className="flex flex-col gap-4">
              {professionals.map((professional) => (
                <Accordion
                  key={professional.externalId}
                  title={professional.name}
                >
                  <div className="px-6 py-4">
                    <div>
                      <p className="text-lg font-bold text-primary-orange">
                        Servicios que ofrece:
                      </p>
                      {professional.services.length === 0 ? (
                        <p className="text-gray-500">
                          No tiene servicios asignados.
                        </p>
                      ) : (
                        <ul>
                          {professional.services.map((service) => (
                            <li key={service.externalId}>{service.name}</li>
                          ))}
                        </ul>
                      )}
                      <div className="my-2">
                        <Button
                          label="Editar servicios"
                          onClick={() => openEditServicesModal(professional)}
                        />
                      </div>
                    </div>
                    <div className="my-3 border-t border-gray-300" />
                    <div>
                      <p className="text-lg font-bold text-primary-orange">
                        Horarios:
                      </p>
                      <ul>
                        {professional.availabilities.length === 0 ? (
                          <li>No tiene horarios configurados.</li>
                        ) : (
                          groupAvailabilitiesByDay(
                            professional.availabilities,
                          ).map(([dayOfWeek, timeRanges]) => (
                            <li key={`${professional.externalId}-${dayOfWeek}`}>
                              {getDayLabel(dayOfWeek)}: {timeRanges.join(", ")}
                            </li>
                          ))
                        )}
                      </ul>
                      <p className="text-gray-500">
                        Para editar los horarios, dirígete a la pestaña{" "}
                        <Link
                          href={`/dashboard/mi-negocio?tab=horarios&professional=${professional.externalId}`}
                          className="text-primary-orange underline transition hover:text-orange-400"
                        >
                          Horarios
                        </Link>
                      </p>
                    </div>
                  </div>
                </Accordion>
              ))}
            </div>
          )}
        </div>
      )}

      {businessServices.length === 0 && (
        <p className="my-4 text-sm text-gray-500">
          Primero necesitas crear servicios en la sección "Mis Servicios" para
          poder asignarlos a un profesional.
        </p>
      )}

      <div className="my-4">
        <AddItemButton
          label="Agregar profesional"
          loadingLabel="Agregando profesional..."
          onClick={openCreateModal}
          disabled={businessServices.length === 0}
        />
      </div>

      <ModalForm
        open={modalMode !== null}
        onClose={closeModal}
        title={modalTitle}
        onSubmit={handleSubmit}
        submitLabel={submitLabel}
        loading={loading}
        loadingLabel={loadingLabel}
        submitDisabled={isSubmitDisabled}
        width="lg"
      >
        {modalMode === "create" && (
          <div>
            <label htmlFor="professional-name" className="mb-1 block">
              Nombre del profesional
            </label>
            <input
              id="professional-name"
              name="professionalName"
              type="text"
              placeholder="Nombre del profesional"
              className="w-full rounded-lg border border-gray-300 px-3 py-2 focus:outline-none focus:ring-2 focus:ring-orange-500"
              value={professionalName}
              onChange={(e) => setProfessionalName(e.target.value)}
            />
          </div>
        )}

        <div>
          <p className="mb-2 font-medium text-gray-700">Servicios que ofrece</p>

          <div className="flex flex-wrap gap-2">
            {businessServices.map((service) => {
              const isSelected = selectedServiceIds.includes(service.externalId)

              return (
                <label
                  key={service.externalId}
                  htmlFor={service.externalId}
                  className={`flex cursor-pointer items-center gap-2 rounded-full border px-3 py-1 transition ${
                    isSelected
                      ? "border-primary-orange bg-orange-100 text-orange-800"
                      : "border-gray-300 text-gray-700 hover:border-primary-orange"
                  }`}
                >
                  <input
                    id={service.externalId}
                    type="checkbox"
                    checked={isSelected}
                    onChange={() => toggleService(service.externalId)}
                    className="h-4 w-4"
                  />
                  {service.name}
                </label>
              )
            })}
          </div>
        </div>

        {error && (
          <div className="rounded border border-red-400 bg-red-200 px-4 py-3 text-red-700">
            <p>
              {modalMode === "create"
                ? "Ocurrió un error al crear el profesional:"
                : "Ocurrió un error al actualizar los servicios:"}
            </p>
            <p>{error}</p>
          </div>
        )}
      </ModalForm>
    </div>
  )
}
