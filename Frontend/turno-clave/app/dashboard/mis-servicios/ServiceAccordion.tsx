"use client"

import { PencilIcon, TrashIcon } from "@heroicons/react/24/outline"
import { useRouter } from "next/navigation"
import { useState } from "react"
import Accordion from "@/app/components/Accordion"
import ModalForm from "@/app/components/ModalForm"
import { deleteService } from "@/services/serviceService"
import type { Professional } from "@/types/professional"
import type { Service } from "@/types/service"
import ServiceFormModal from "./ServiceFormModal"

interface Props {
  service: Service
  professionals: Professional[]
  onDeleteSuccess?: (serviceExternalId: string) => void
  onUpdateSuccess?: (service: Service) => void
}

export default function ServiceAccordion({
  service,
  professionals,
  onDeleteSuccess,
  onUpdateSuccess,
}: Props) {
  const router = useRouter()

  const [openEditModal, setOpenEditModal] = useState(false)
  const [openDeleteModal, setOpenDeleteModal] = useState(false)
  const [loading, setLoading] = useState(false)

  const handleDelete = async (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault()
    setLoading(true)

    try {
      await deleteService(service.externalId)
      onDeleteSuccess?.(service.externalId)
      router.refresh()
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : "Unknown error"
      console.error("[ServiceAccordion] Error:", errorMessage)
    } finally {
      setLoading(false)
      setOpenDeleteModal(false)
    }
  }

  return (
    <div>
      <Accordion title={service.name}>
        <div className="px-4 py-3 text-md text-gray-700 space-y-1">
          <p>
            <span className="text-primary-orange font-bold">Precio:</span> $
            {service.price.toLocaleString("es-AR")}
          </p>
          <p>
            <span className="text-primary-orange font-bold">Duración:</span>{" "}
            {service.durationMinutes} minutos
          </p>
          {service.description && (
            <p>
              <span className="text-primary-orange font-bold">
                Descripción:
              </span>{" "}
              {service.description}
            </p>
          )}
          {service.professionals.length > 0 && (
            <p>
              <span className="text-primary-orange font-bold">
                {service.professionals.length === 1
                  ? "Profesional:"
                  : "Profesionales:"}
              </span>{" "}
              {service.professionals.map((p) => p.name).join(", ")}
            </p>
          )}

          <div className="flex justify-end gap-2 mt-3">
            <button
              type="button"
              onClick={() => setOpenEditModal(true)}
              className="flex cursor-pointer items-center gap-1 text-md px-3 py-1 rounded-md bg-gray-200 hover:bg-gray-300 transition"
            >
              <PencilIcon className="w-4 h-4" />
              Editar
            </button>

            <button
              type="button"
              onClick={() => setOpenDeleteModal(true)}
              className="flex cursor-pointer items-center gap-1 text-md px-3 py-1 rounded-md bg-red-100 text-red-600 hover:bg-red-200 transition"
            >
              <TrashIcon className="w-4 h-4" />
              Eliminar
            </button>
          </div>
        </div>
      </Accordion>

      <ServiceFormModal
        open={openEditModal}
        onClose={() => setOpenEditModal(false)}
        professionals={professionals}
        mode="edit"
        initialService={service}
        onSuccess={(updatedService) => {
          onUpdateSuccess?.(updatedService)
          setOpenEditModal(false)
          router.refresh()
        }}
      />

      <ModalForm
        open={openDeleteModal}
        onClose={() => setOpenDeleteModal(false)}
        title="Eliminar servicio"
        onSubmit={handleDelete}
        submitLabel="Eliminar"
        loading={loading}
        loadingLabel="Eliminando Servicio..."
      >
        <p>¿Estás seguro de que quieres eliminar este servicio?</p>
      </ModalForm>
    </div>
  )
}
