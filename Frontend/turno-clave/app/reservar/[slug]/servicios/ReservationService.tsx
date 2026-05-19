"use client"

import type { Professional } from "@/types/professional"
import type { Service } from "@/types/service"

type Props = {
  service: Service
  professionals: Professional[]
  selected: boolean
  onClick: (service: Service) => void
}

export default function ReservationService({
  service,
  professionals,
  selected,
  onClick,
}: Props) {
  return (
    <label
      className={`relative block border ${selected ? "border-orange-500" : "border-gray-300"} rounded-lg cursor-pointer`}
    >
      <input
        type="checkbox"
        checked={selected}
        onChange={() => onClick(service)}
        className="
          peer
          absolute
          appearance-none
          w-5 h-5
          rounded-full
          border-2 border-gray-300
          cursor-pointer
          checked:bg-orange-500
          checked:border-orange-500
          top-2
          left-2

          after:content-['✔']
          after:text-white
          after:text-md
          after:absolute
          after:top-1/2
          after:left-1/2
          after:-translate-x-1/2
          after:-translate-y-1/2
          after:hidden
          checked:after:block
        "
      />

      <div className="pb-4 pt-5 px-6 unselectable">
        <p className="font-bold text-dark-blue text-xl">{service.name}</p>

        <div className="flex items-center justify-between">
          <p>{service.durationMinutes} minutos</p>

          <p className="font-bold text-dark-blue text-xl">
            ${service.price.toLocaleString("es-AR")}
          </p>
        </div>

        <div className="flex gap-1 mt-1 items-center">
          <p>Brindado por</p>

          <select
            className="border border-gray-300 rounded-full px-2"
            onClick={(e) => e.stopPropagation()}
          >
            <option>Cualquier profesional</option>

            {professionals.map((professional) => (
              <option
                key={professional.externalId}
                value={professional.externalId}
              >
                {professional.name}
              </option>
            ))}
          </select>
        </div>
      </div>
    </label>
  )
}
