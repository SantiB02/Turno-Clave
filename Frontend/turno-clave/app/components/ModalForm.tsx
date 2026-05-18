"use client"
import { XMarkIcon } from "@heroicons/react/24/outline"
import type { ReactNode } from "react"
import { useEffect, useState } from "react"
import { createPortal } from "react-dom"

interface ModalFormProps {
  open: boolean
  onClose: () => void
  title: string
  onSubmit: (e: React.SubmitEvent<HTMLFormElement>) => void
  children: ReactNode
  submitLabel?: string
  loading?: boolean
  loadingLabel?: string
  submitDisabled?: boolean
  submitButtonBgColor?: string
  submitButtonBgHoverColor?: string
  width?: "xs" | "sm" | "md" | "lg" | "xl" | "2xl" | "3xl" | "full"
}

export default function ModalForm({
  open,
  onClose,
  title,
  onSubmit,
  children,
  submitLabel = "Guardar",
  loading = false,
  loadingLabel = "Cargando...",
  submitDisabled = false,
  submitButtonBgColor,
  submitButtonBgHoverColor,
  width,
}: ModalFormProps) {
  const [mounted, setMounted] = useState(false)

  useEffect(() => {
    setMounted(true)

    return () => {
      setMounted(false)
    }
  }, [])

  useEffect(() => {
    if (!open) return

    const previousBodyOverflow = document.body.style.overflow
    const previousBodyPaddingRight = document.body.style.paddingRight
    const scrollbarWidth =
      window.innerWidth - document.documentElement.clientWidth

    document.body.style.overflow = "hidden"

    if (scrollbarWidth > 0) {
      document.body.style.paddingRight = `${scrollbarWidth}px`
    }

    return () => {
      document.body.style.overflow = previousBodyOverflow
      document.body.style.paddingRight = previousBodyPaddingRight
    }
  }, [open])

  if (!open || !mounted) return null

  const widthClass = {
    xs: "max-w-xs",
    sm: "max-w-sm",
    md: "max-w-md",
    lg: "max-w-lg",
    xl: "max-w-xl",
    "2xl": "max-w-2xl",
    "3xl": "max-w-3xl",
    full: "max-w-full",
  }[width ?? "md"]

  return createPortal(
    <div className="fixed inset-0 z-50 text-gray-800">
      {/* Overlay */}
      <div className="absolute inset-0 bg-black/40" />

      <div className="relative z-10 h-full overflow-y-auto p-4">
        <div className="mx-auto flex min-h-full w-full items-center py-8">
          {/* Modal */}
          <div
            className={`relative mx-auto flex w-full ${widthClass} flex-col rounded-2xl bg-white p-6 shadow-lg animate-fadeIn`}
          >
            {/* Header */}
            <div className="flex items-center justify-between mb-4">
              <h2 className="text-lg font-semibold text-gray-800">{title}</h2>

              <button
                type="button"
                onClick={onClose}
                className="p-1 rounded-full hover:bg-gray-100 transition"
              >
                <XMarkIcon className="w-5 h-5 cursor-pointer text-gray-500" />
              </button>
            </div>

            {/* Form */}
            <form
              onSubmit={onSubmit}
              className="flex flex-col overflow-visible"
            >
              <div className="space-y-4 overflow-visible">{children}</div>

              {/* Actions */}
              <div className="flex justify-end gap-4 mt-6">
                <button
                  type="button"
                  onClick={onClose}
                  className="px-4 py-2 bg-gray-200 cursor-pointer rounded-lg text-gray-700 hover:bg-gray-300 transition"
                >
                  Cancelar
                </button>

                <button
                  type="submit"
                  className={`px-4 py-2 rounded-lg ${submitButtonBgColor ? `${submitButtonBgColor}` : "bg-orange-500"} text-white font-medium ${submitButtonBgHoverColor ? `${submitButtonBgHoverColor}` : "hover:bg-orange-600"} transition ${loading || submitDisabled ? "opacity-50 cursor-loading" : "cursor-pointer"}`}
                  disabled={loading || submitDisabled}
                >
                  {loading ? loadingLabel : submitLabel}
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>,
    document.body,
  )
}
