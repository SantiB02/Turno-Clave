"use client"
import { XMarkIcon } from "@heroicons/react/24/outline"
import type { ReactNode } from "react"

interface ModalFormProps {
  open: boolean
  onClose: () => void
  title: string
  onSubmit: (e: React.FormEvent) => void
  children: ReactNode
  submitLabel?: string
  loading?: boolean
  loadingLabel?: string
  submitDisabled?: boolean
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
}: ModalFormProps) {
  if (!open) return null

  return (
    <div className="fixed inset-0 z-50 text-gray-800 flex items-center justify-center">
      {/* Overlay */}
      <div className="absolute inset-0 bg-black/40" />

      {/* Modal */}
      <div className="relative bg-white w-full max-w-md rounded-2xl shadow-lg p-6 z-10 animate-fadeIn">
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
        <form onSubmit={onSubmit}>
          <div className="space-y-4">{children}</div>

          {/* Actions */}
          <div className="flex justify-end gap-2 mt-6">
            <button
              type="button"
              onClick={onClose}
              className="px-4 py-2 cursor-pointer rounded-lg text-gray-600 hover:bg-gray-100 transition"
            >
              Cancelar
            </button>

            <button
              type="submit"
              className={`px-4 py-2 rounded-lg bg-orange-500 text-white font-medium hover:bg-orange-600 transition ${loading || submitDisabled ? "opacity-50 cursor-not-allowed" : "cursor-pointer"}`}
              disabled={loading || submitDisabled}
            >
              {loading ? loadingLabel : submitLabel}
            </button>
          </div>
        </form>
      </div>
    </div>
  )
}
