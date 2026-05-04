"use client"
import { PlusIcon } from "@heroicons/react/24/solid"

type AddServiceButtonProps = {
  label: string
  loadingLabel?: string
  onClick: () => void
  disabled?: boolean
  loading?: boolean
}

export default function AddItemButton({
  label,
  loadingLabel = "Cargando...",
  onClick,
  disabled = false,
  loading = false,
}: AddServiceButtonProps) {
  return (
    <button
      disabled={disabled}
      type="button"
      className={`flex cursor-pointer items-center gap-3 border-2 border-orange-500 text-gray-800 rounded-full pr-4  hover:bg-orange-50 transition ${loading ? "opacity-50 cursor-not-allowed" : ""}`}
      onClick={onClick}
    >
      <span className="flex items-center justify-center w-10 h-10 bg-gradient-to-r from-orange-400 to-orange-600 rounded-full">
        <PlusIcon className="w-7 h-7 text-white" />
      </span>

      <span className="text-xl">{loading ? loadingLabel : label}</span>
    </button>
  )
}
