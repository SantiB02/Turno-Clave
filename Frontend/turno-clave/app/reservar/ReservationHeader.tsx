import BackButton from "../components/BackButton"
import ReservationStepper from "./ReservationStepper"

type Props = {
  title: string
  backButtonUrl?: string
  currentStep?: number
}

export default function ReservationHeader({
  title,
  backButtonUrl,
  currentStep,
}: Props) {
  return (
    <div className="flex flex-col justify-center mb-4">
      <div className="flex relative justify-center items-center mb-4">
        {backButtonUrl && (
          <BackButton href={backButtonUrl} className="absolute left-0" />
        )}
        <h1 className="text-2xl md:text-3xl font-bold text-dark-blue text-center">
          {title}
        </h1>
      </div>
      {currentStep !== undefined && (
        <ReservationStepper currentStep={currentStep} />
      )}
    </div>
  )
}
