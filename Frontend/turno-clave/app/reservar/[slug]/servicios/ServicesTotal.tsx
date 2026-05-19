import { ShoppingCartIcon } from "@heroicons/react/24/outline"
import type { Service } from "@/types/service"

type Props = {
  services: Service[]
}

export default function ServicesTotal({ services }: Props) {
  const totalDuration = services.reduce(
    (acc, service) => acc + service.durationMinutes,
    0,
  )

  const hours = Math.floor(totalDuration / 60)
  const minutes = totalDuration % 60

  let durationLabel = ""

  if (hours > 0 && minutes > 0) {
    durationLabel = `${hours} h ${minutes} min`
  } else if (hours > 0) {
    durationLabel = `${hours} h`
  } else {
    durationLabel = `${minutes} min`
  }

  return (
    <div
      className={`flex ${services.length === 0 ? "justify-center items-center" : "justify-between"} h-15 border border-gray-300 rounded-lg px-2 py-3`}
    >
      {services.length === 0 ? (
        <p className="text-gray-500 italic">Seleccione al menos un servicio.</p>
      ) : (
        <>
          <div className="flex items-center text-gray-600 gap-2">
            <ShoppingCartIcon width={20} height={20} />
            <p>
              {services.length} servicio{services.length > 1 ? "s" : ""} -{" "}
              {durationLabel}
            </p>
          </div>
          <p className="font-bold text-dark-blue text-xl">
            Total $
            {services
              .reduce((acc, service) => acc + service.price, 0)
              .toLocaleString("es-AR")}
          </p>
        </>
      )}
    </div>
  )
}
