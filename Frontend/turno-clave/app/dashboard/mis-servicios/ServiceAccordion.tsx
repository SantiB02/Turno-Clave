"use client"

import {
  ChevronDownIcon,
  PencilIcon,
  TrashIcon,
} from "@heroicons/react/24/outline"
import { useState } from "react"
import type { Service } from "@/types/service"

interface Props {
  service: Service
  onEdit?: (id: string) => void
  onDelete?: (id: string) => void
}

export default function ServiceAccordion({ service, onEdit, onDelete }: Props) {
  const [open, setOpen] = useState(false)

  return (
    <div className="max-w-xl rounded-xl overflow-hidden shadow-sm border border-gray-200">
      {/* Header */}
      <button
        type="button"
        onClick={() => setOpen(!open)}
        className="w-full cursor-pointer flex items-center justify-between bg-primary-orange text-white px-4 py-3"
      >
        <span className="text-xl">{service.name}</span>
        <ChevronDownIcon
          className={`w-5 h-5 transition-transform ${open ? "rotate-180" : ""}`}
        />
      </button>

      {/* Content */}
      {open && (
        <div className="px-4 py-3 text-md text-gray-700 space-y-1">
          <p>
            <span className="text-primary-orange font-bold">Precio:</span> $
            {service.price.toLocaleString("es-AR")}
          </p>
          <p>
            <span className="text-primary-orange font-bold">Duración:</span>{" "}
            {service.durationMinutes} minutos
          </p>
          {/* {service.professional && <p>- Profesional: {service.professional}</p>} */}
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

          {/* Actions */}
          <div className="flex justify-end gap-2 mt-3">
            <button
              type="button"
              onClick={() => onEdit?.(service.externalId)}
              className="flex cursor-pointer items-center gap-1 text-md px-3 py-1 rounded-md bg-gray-200 hover:bg-gray-300 transition"
            >
              <PencilIcon className="w-4 h-4" />
              Editar
            </button>

            <button
              type="button"
              onClick={() => onDelete?.(service.externalId)}
              className="flex cursor-pointer items-center gap-1 text-md px-3 py-1 rounded-md bg-red-100 text-red-600 hover:bg-red-200 transition"
            >
              <TrashIcon className="w-4 h-4" />
              Eliminar
            </button>
          </div>
        </div>
      )}
    </div>
  )
}
