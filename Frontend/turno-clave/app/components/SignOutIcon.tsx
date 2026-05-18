"use client"

import { ArrowRightStartOnRectangleIcon } from "@heroicons/react/24/outline"
import { useState } from "react"
import { handleSignOut } from "@/lib/actions/auth"
import ModalForm from "./ModalForm"

type SignOutIconProps = {
  variant?: "desktop" | "mobile"
  onOpenChange?: () => void
}

export default function SignOutIcon({
  variant = "desktop",
  onOpenChange,
}: SignOutIconProps) {
  const [openSignoutModal, setOpenSignoutModal] = useState<boolean>(false)
  const [loading, setLoading] = useState<boolean>(false)
  const isMobile = variant === "mobile"

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

  const handleOpen = () => {
    setOpenSignoutModal(true)
    onOpenChange?.()
  }

  return (
    <>
      <button
        type="button"
        onClick={handleOpen}
        aria-label="Cerrar sesion"
        title="Cerrar sesion"
        className={
          isMobile
            ? "flex cursor-pointer w-full items-center gap-3 rounded-xl bg-dark-blue px-4 py-3 text-left text-white transition hover:bg-orange-400/70"
            : "flex cursor-pointer w-full items-center justify-center rounded py-2 text-white transition hover:bg-orange-400"
        }
      >
        <ArrowRightStartOnRectangleIcon
          className={isMobile ? "h-6 w-6 shrink-0" : "h-10 w-10"}
        />
        {isMobile ? <span className="font-semibold">Cerrar sesión</span> : null}
      </button>
      <ModalForm
        open={openSignoutModal}
        onClose={() => setOpenSignoutModal(false)}
        title="Cerrar sesión"
        onSubmit={handleSubmit}
        loading={loading}
        loadingLabel="Cerrando Sesión..."
        submitLabel="Cerrar Sesión"
        submitDisabled={false}
      >
        <p>¿Estas seguro de que quieres cerrar sesión?</p>
      </ModalForm>
    </>
  )
}
