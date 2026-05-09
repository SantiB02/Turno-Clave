"use client"

import { useRouter } from "next/navigation"
import { useState } from "react"
import AddItemButton from "@/app/components/AddItemButton"
import type { Professional } from "@/types/professional"
import type { Service } from "@/types/service"
import ServiceFormModal from "./ServiceFormModal"

type AddServiceProps = {
  professionals: Professional[]
  onCreateSuccess?: (service: Service) => void
}

export default function AddService({
  professionals,
  onCreateSuccess,
}: AddServiceProps) {
  const router = useRouter()
  const [openModal, setOpenModal] = useState(false)

  return (
    <div className="my-4">
      <AddItemButton
        label="Agregar servicio"
        loadingLabel="Creando servicio..."
        onClick={() => setOpenModal(true)}
      />

      <ServiceFormModal
        open={openModal}
        onClose={() => setOpenModal(false)}
        professionals={professionals}
        mode="create"
        onSuccess={(service) => {
          onCreateSuccess?.(service)
          setOpenModal(false)
          router.refresh()
        }}
      />
    </div>
  )
}
