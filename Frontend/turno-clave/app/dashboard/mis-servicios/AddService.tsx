"use client"
import { useRouter } from "next/navigation"
import { useState } from "react"
import AddItemButton from "@/app/components/AddItemButton"
import ModalForm from "@/app/components/ModalForm"
import { createService } from "@/services/serviceService"
import type { CreateServiceDTO } from "@/types/service"

export default function AddService() {
  const router = useRouter()

  const [openModal, setOpenModal] = useState(false)
  const [name, setName] = useState("")
  const [description, setDescription] = useState("")
  const [price, setPrice] = useState("")
  const [durationMinutes, setDurationMinutes] = useState("")
  const [isSubmitting, setIsSubmitting] = useState(false)

  const handlePriceChange = (value: string) => {
    // replace non-numeric characters
    const numeric = value.replace(/\D/g, "")

    // avoid leading zeros
    const cleaned = numeric.replace(/^0+/, "")

    // format with Argentine thousand separator
    const formatted = new Intl.NumberFormat("es-AR").format(
      Number(cleaned || 0),
    )

    // limit to 9 digits (up to 999.999.999)
    if (cleaned.length > 9) return

    setPrice(cleaned ? formatted : "")
  }

  const handleDurationChange = (value: string) => {
    const numeric = value.replace(/\D/g, "")

    // limit to 3 digits (up to 999 minutes)
    if (numeric.length > 3) return

    setDurationMinutes(numeric)
  }

  const isSubmitDisabled = !name || !price || !durationMinutes || isSubmitting

  const handleCloseModal = () => {
    setOpenModal(false)
    setName("")
    setDescription("")
    setPrice("")
    setDurationMinutes("")
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setIsSubmitting(true)

    const numericPrice = Number(price.replace(/\./g, ""))

    const data: CreateServiceDTO = {
      name,
      description,
      price: numericPrice,
      durationMinutes: Number(durationMinutes),
    }

    await createService(data)
    router.refresh()

    console.log(data)

    setOpenModal(false)
    setIsSubmitting(false)
  }

  return (
    <div className="my-4">
      <AddItemButton
        label="Agregar servicio"
        loadingLabel="Creando servicio..."
        onClick={() => setOpenModal(true)}
      />

      <ModalForm
        open={openModal}
        onClose={handleCloseModal}
        title="Nuevo servicio"
        onSubmit={handleSubmit}
        submitLabel="Crear servicio"
        loading={isSubmitting}
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
    </div>
  )
}
