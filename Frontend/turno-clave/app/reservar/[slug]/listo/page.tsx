import ReservationHeader from "../../ReservationHeader"
import ConfirmationDetails from "./ConfirmationDetails"

export default function ReservaListo() {
  return (
    <div className="flex flex-col items-center justify-center h-full text-center gap-6 m-4">
      <ReservationHeader title="¡Listo, tu turno está confirmado!" />
      <ConfirmationDetails />
    </div>
  )
}
