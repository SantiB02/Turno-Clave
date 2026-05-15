"use client"

import Link from "next/link"
import { useEffect, useState } from "react"
import ModalForm from "@/app/components/ModalForm"
import { createService, updateService } from "@/services/serviceService"
import type { Professional, ServiceProfessional } from "@/types/professional"
import type { CreateServiceDTO, Service } from "@/types/service"

type ServiceFormModalProps = {
  open: boolean
  onClose: () => void
  professionals: Professional[]
  mode: "create" | "edit"
  initialService?: Service
  onSuccess?: (service: Service) => void
}

function formatPrice(value: number) {
  return new Intl.NumberFormat("es-AR").format(value)
}

export default function ServiceFormModal({
  open,
  onClose,
  professionals,
  mode,
  initialService,
  onSuccess,
}: ServiceFormModalProps) {
  const [name, setName] = useState("")
  const [description, setDescription] = useState("")
  const [selectedProfessionalId, setSelectedProfessionalId] = useState("")
  const [selectedProfessionals, setSelectedProfessionals] = useState<
    ServiceProfessional[]
  >([])
  const [price, setPrice] = useState("")
  const [durationMinutes, setDurationMinutes] = useState("")
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    if (!open) return

    if (mode === "edit" && initialService) {
      setName(initialService.name)
      setDescription(initialService.description ?? "")
      setSelectedProfessionals(initialService.professionals)
      setPrice(formatPrice(initialService.price))
      setDurationMinutes(String(initialService.durationMinutes))
      setSelectedProfessionalId("")
      return
    }

    setName("")
    setDescription("")
    setSelectedProfessionals([])
    setPrice("")
    setDurationMinutes("")
    setSelectedProfessionalId("")
  }, [initialService, mode, open])

  const handlePriceChange = (value: string) => {
    const numeric = value.replace(/\D/g, "")
    const cleaned = numeric.replace(/^0+/, "")

    if (cleaned.length > 9) return

    setPrice(cleaned ? formatPrice(Number(cleaned)) : "")
  }

  const handleDurationChange = (value: string) => {
    const numeric = value.replace(/\D/g, "")

    if (numeric.length > 3) return

    setDurationMinutes(numeric)
  }

  const handleClose = () => {
    if (isSubmitting) return
    onClose()
  }

  const handleSelectProfessional = (externalId: string) => {
    setSelectedProfessionalId(externalId)

    const professional = professionals.find(
      (item) => item.externalId === externalId,
    )

    if (!professional) return

    const alreadySelected = selectedProfessionals.some(
      (item) => item.externalId === professional.externalId,
    )

    if (alreadySelected) {
      setSelectedProfessionalId("")
      return
    }

    setSelectedProfessionals((currentProfessionals) => [
      ...currentProfessionals,
      {
        externalId: professional.externalId,
        name: professional.name,
      },
    ])
    setSelectedProfessionalId("")
  }

  const handleRemoveProfessional = (externalId: string) => {
    setSelectedProfessionals((currentProfessionals) =>
      currentProfessionals.filter(
        (professional) => professional.externalId !== externalId,
      ),
    )
  }

  const handleSubmit = async (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault()
    setIsSubmitting(true)

    try {
      const data: CreateServiceDTO = {
        name,
        description,
        professionalExternalIds: selectedProfessionals.map(
          (professional) => professional.externalId,
        ),
        price: Number(price.replace(/\./g, "")),
        durationMinutes: Number(durationMinutes),
      }

      let result = null
      if (mode === "create") {
        result = await createService(data)
      } else if (mode === "edit") {
        result = await updateService(initialService!.externalId, data)
      }

      if (!result || !result.ok) {
        setError(result?.message ?? "Error inesperado")
        return
      }

      const service = result.data

      onSuccess?.(service)
      onClose()
    } finally {
      setIsSubmitting(false)
    }
  }

  const isSubmitDisabled =
    !name ||
    !price ||
    !durationMinutes ||
    !selectedProfessionals.length ||
    isSubmitting

  return (
    <ModalForm
      open={open}
      onClose={handleClose}
      title={mode === "create" ? "Nuevo servicio" : "Editar servicio"}
      onSubmit={handleSubmit}
      submitLabel={mode === "create" ? "Crear servicio" : "Guardar cambios"}
      loading={isSubmitting}
      loadingLabel={
        mode === "create" ? "Creando servicio..." : "Guardando cambios..."
      }
      submitDisabled={isSubmitDisabled}
    >
      <input
        name="name"
        type="text"
        placeholder="Nombre del servicio"
        className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-orange-500"
        required
        value={name}
        onChange={(e) => setName(e.target.value)}
      />

      <input
        name="description"
        type="text"
        placeholder="Descripción (opcional)"
        className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-orange-500"
        value={description}
        onChange={(e) => setDescription(e.target.value)}
      />

      <div>
        <select
          value={selectedProfessionalId}
          onChange={(e) => handleSelectProfessional(e.target.value)}
          className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-orange-500"
        >
          <option value="">Seleccionar profesional</option>
          {professionals.map((professional) => (
            <option
              key={professional.externalId}
              value={professional.externalId}
            >
              {professional.name}
            </option>
          ))}
        </select>
        <p className="text-sm text-gray-500 mb-2">
          Para crear un nuevo profesional, dirígete a{" "}
          <Link
            href="/dashboard/mi-negocio"
            className="text-primary-orange cursor-pointer hover:underline"
          >
            Mi Negocio
          </Link>
          .
        </p>
        <div className="flex flex-wrap gap-2">
          {selectedProfessionals.map((professional) => (
            <div
              key={professional.externalId}
              className="flex items-center gap-2 bg-orange-100 text-orange-800 px-3 py-1 rounded-full"
            >
              <span>{professional.name}</span>
              <button
                type="button"
                className="text-2xl cursor-pointer"
                onClick={() =>
                  handleRemoveProfessional(professional.externalId)
                }
              >
                ×
              </button>
            </div>
          ))}
        </div>
      </div>

      <div className="flex gap-x-6">
        <div className="flex items-center gap-x-2">
          <p>$</p>
          <input
            name="price"
            type="text"
            placeholder="Precio"
            className="w-20 border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-orange-500"
            required
            value={price}
            onChange={(e) => handlePriceChange(e.target.value)}
          />
          <p>ARS</p>
        </div>
        <div className="flex items-center gap-x-2">
          <input
            name="durationMinutes"
            type="number"
            min="1"
            max="999"
            placeholder="Duración"
            className="w-26 border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-orange-500"
            required
            value={durationMinutes}
            onChange={(e) => handleDurationChange(e.target.value)}
          />
          <p>minutos</p>
        </div>
      </div>
    </ModalForm>
  )
}
