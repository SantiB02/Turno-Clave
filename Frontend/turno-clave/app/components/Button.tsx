import Link from "next/link"

interface ButtonProps {
  label: string
  onClick?: () => void
  disabled?: boolean
  type?: "button" | "submit" | "reset"
  href?: string
  backgroundColor?: string
  hoverBackgroundColor?: string
}

export default function Button({
  label,
  onClick,
  disabled,
  type,
  href,
  backgroundColor,
  hoverBackgroundColor,
}: ButtonProps) {
  if (href) {
    return (
      <Link href={href}>
        <button
          type={type || "button"}
          onClick={onClick}
          disabled={disabled}
          className={`
            ${backgroundColor ?? "bg-primary-orange"}
            ${hoverBackgroundColor ?? "hover:bg-primary-orange"}
            cursor-pointer text-white px-4 py-2 rounded-lg transition
          `}
        >
          {label}
        </button>
      </Link>
    )
  } else {
    return (
      <button
        type={type || "button"}
        onClick={onClick}
        disabled={disabled}
        className={`
            ${backgroundColor ?? "bg-primary-orange"}
            ${hoverBackgroundColor ?? "hover:bg-primary-orange"}
            cursor-pointer text-white px-4 py-2 rounded-lg transition
          `}
      >
        {label}
      </button>
    )
  }
}
