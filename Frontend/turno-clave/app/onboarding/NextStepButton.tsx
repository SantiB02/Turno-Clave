import { ArrowRightIcon } from "@heroicons/react/20/solid"
import Link from "next/link"

type NextStepButtonProps = {
  onClick?: () => void
  disabled?: boolean
  label?: string
  href?: string
  type?: "button" | "submit" | "reset"
  className?: string
}

export default function NextStepButton({
  onClick,
  disabled = false,
  label = "Continuar",
  href,
  type = "button",
  className,
}: NextStepButtonProps) {
  if (href) {
    return (
      <Link href={href}>
        <button
          type={type}
          onClick={onClick}
          disabled={disabled}
          className={`rounded-xl inline-flex items-center px-4 py-2 mt-4 border border-transparent text-md font-medium rounded-md shadow-sm text-white ${className || ""} ${
            disabled
              ? "bg-gray-400 cursor-not-allowed"
              : "bg-primary-orange hover:bg-primary-orange/80 hover:shadow-sm transition cursor-pointer"
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
        type={type}
        onClick={onClick}
        disabled={disabled}
        className={`rounded-xl inline-flex items-center px-4 py-2 mt-4 border border-transparent text-md font-medium rounded-md shadow-sm text-white ${className || ""} ${
          disabled
            ? "bg-gray-400"
            : "bg-primary-orange hover:bg-primary-orange/80 hover:shadow-sm transition cursor-pointer"
        }`}
      >
        <ArrowRightIcon className="h-6 w-6 mr-2" />
        {label}
      </button>
    )
  }
}
