import ErrorMessage from "@/app/components/ErrorMessage"
import { getActiveBusiness } from "@/services/businessService"
import type { BusinessAvailabilityDTO } from "@/types/business"
import MisTurnosCalendar from "./MisTurnosCalendar"

export const dynamic = "force-dynamic"

export default async function MisTurnos() {
  let businessAvailabilities: BusinessAvailabilityDTO[] = []
  let businessError: string | null = null

  const businessResult = await getActiveBusiness()

  if (businessResult.ok) {
    businessAvailabilities = businessResult.data.availabilities
  } else {
    businessError = businessResult.message
  }

  return (
    <div>
      <h1 className="font-bold text-4xl mb-9">Mis Turnos</h1>
      <div className="mx-0 sm:mx-6">
        {businessError && (
          <div className="mb-3">
            <ErrorMessage
              title="Ocurrió un error al cargar el horario del negocio:"
              message={businessError}
            />
          </div>
        )}
        <MisTurnosCalendar businessAvailabilities={businessAvailabilities} />
      </div>
    </div>
  )
}
