"use client"

import { useReservationBusiness } from "../ReservationBusinessProvider"

export default function ReservaServicios() {
  const business = useReservationBusiness()

  return (
    <div className="m-4">
      <div className="max-w-xl mx-auto">
        <h1 className="text-2xl font-bold">Servicios de {business.name}</h1>
        <p className="text-gray-600">
          Esta pantalla ya puede reutilizar el negocio desde el provider del
          flujo.
        </p>
      </div>
    </div>
  )
}
