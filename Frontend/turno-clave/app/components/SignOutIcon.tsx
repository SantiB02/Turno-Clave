"use client"

import { ArrowRightStartOnRectangleIcon } from "@heroicons/react/24/outline"
import { useState } from "react"
import { handleSignOut } from "@/lib/actions/auth"
import ModalForm from "./ModalForm"

export default function SignOutIcon() {
  const [openSignoutModal, setOpenSignoutModal] = useState<boolean>(false)
  const [loading, setLoading] = useState<boolean>(false)

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setLoading(true)

    try {
      await handleSignOut()
    } catch (err) {
      console.error(err)
    } finally {
      setLoading(false)
    }
  }

  return (
    <>
      <button
        type="button"
        onClick={() => setOpenSignoutModal(true)}
        aria-label="Cerrar Sesión"
        title="Cerrar Sesión"
        className="flex items-center cursor-pointer justify-center py-2 rounded hover:bg-orange-400 w-full text-white"
      >
        <ArrowRightStartOnRectangleIcon className="h-10 w-10" />
      </button>
      <ModalForm
        open={openSignoutModal}
        onClose={() => setOpenSignoutModal(false)}
        title="Cerrar Sesión"
        onSubmit={handleSubmit}
        loading={loading}
        loadingLabel="Cerrando Sesión..."
        submitLabel="Cerrar Sesión"
        submitDisabled={false}
      >
        <p>¿Estás seguro de que quieres cerrar sesión?</p>
      </ModalForm>
    </>
  )
}
