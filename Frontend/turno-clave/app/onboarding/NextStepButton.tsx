import { ArrowRightIcon } from "@heroicons/react/20/solid"
import Link from "next/link"

type NextStepButtonProps = {
  onClick?: () => void
  disabled?: boolean
  label?: string
  href?: string
}

export default function NextStepButton({
  onClick,
  disabled = false,
  label = "Continuar",
  href,
}: NextStepButtonProps) {
  if (href) {
    return (
      <Link href={href}>
        <button
          type="button"
          onClick={onClick}
          disabled={disabled}
          className={`rounded-xl cursor-pointer hover:bg-primary-orange/80 hover:shadow-sm transition inline-flex items-center px-4 py-2 mt-4 border border-transparent text-md font-medium rounded-md shadow-sm text-white ${
            disabled ? "bg-gray-400 cursor-not-allowed" : "bg-primary-orange"
          }`}
        >
          <ArrowRightIcon className="h-6 w-6 mr-2" />
          {label}
        </button>
      </Link>
    )
  } else {
    return (
      <button
        type="button"
        onClick={onClick}
        disabled={disabled}
        className={`rounded-xl cursor-pointer hover:bg-primary-orange/80 hover:shadow-sm transition inline-flex items-center px-4 py-2 mt-4 border border-transparent text-md font-medium rounded-md shadow-sm text-white ${
          disabled ? "bg-gray-400 cursor-not-allowed" : "bg-primary-orange"
        }`}
      >
        <ArrowRightIcon className="h-6 w-6 mr-2" />
        {label}
      </button>
    )
  }
}
