import {
  CalendarDaysIcon,
  CheckIcon,
  UserCircleIcon,
  WrenchScrewdriverIcon,
} from "@heroicons/react/24/outline"

interface Step {
  label: string
  icon: React.ReactNode
}

interface ReservationStepperProps {
  currentStep: number
}

const steps: Step[] = [
  {
    label: "Servicios",
    icon: <WrenchScrewdriverIcon width={28} height={28} />,
  },
  {
    label: "Fecha",
    icon: <CalendarDaysIcon width={28} height={28} />,
  },
  {
    label: "Datos",
    icon: <UserCircleIcon width={28} height={28} />,
  },
  {
    label: "Confirmación",
    icon: <CheckIcon width={28} height={28} />,
  },
]

export default function ReservationStepper({
  currentStep,
}: ReservationStepperProps) {
  return (
    <div className="w-full px-2 py-3 sm:px-4 sm:py-4">
      <div className="mx-auto flex max-w-xl items-center justify-center">
        {steps.map((step, index) => {
          const isCompleted = index < currentStep
          const isCurrent = index === currentStep

          return (
            <div key={step.label} className="flex items-center">
              <div className="flex flex-col items-center">
                <div
                  className={`
                flex h-9 w-9 sm:h-12 sm:w-12 shrink-0
                items-center justify-center rounded-full
                border-2 transition-all duration-200
                ${
                  isCompleted
                    ? "border-green-600 bg-green-600 text-white"
                    : isCurrent
                      ? "border-primary-orange bg-primary-orange text-white"
                      : "border-gray-300 bg-gray-200 text-gray-500"
                }
              `}
                >
                  <div className="scale-90 sm:scale-100">{step.icon}</div>
                </div>

                <span
                  className={`
                mt-1 hidden text-center text-xs font-medium
                whitespace-nowrap sm:block
                ${isCompleted || isCurrent ? "text-gray-900" : "text-gray-400"}
              `}
                >
                  {step.label}
                </span>
              </div>

              {index < steps.length - 1 && (
                <div className="mx-1 sm:mx-2 w-7 sm:w-16">
                  <div className="h-1 rounded bg-gray-300">
                    <div
                      className={`
                    h-1 rounded transition-all duration-300
                    ${index < currentStep ? "bg-green-600" : "bg-gray-300"}
                  `}
                    />
                  </div>
                </div>
              )}
            </div>
          )
        })}
      </div>
    </div>
  )
}
