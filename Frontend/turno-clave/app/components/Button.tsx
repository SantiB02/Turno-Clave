import Link from "next/link"

interface ButtonProps {
  label: string
  onClick?: () => void
  disabled?: boolean
  type?: "button" | "submit" | "reset"
  href?: string
}

export default function Button({
  label,
  onClick,
  disabled,
  type,
  href,
}: ButtonProps) {
  if (href) {
    return (
      <Link href={href}>
        <button
          type={type || "button"}
          onClick={onClick}
          disabled={disabled}
          className="bg-primary-orange cursor-pointer hover:bg-primary-orange/80 text-white px-6 py-2 rounded-lg transition"
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
        className="bg-primary-orange cursor-pointer hover:bg-primary-orange/80 text-white px-6 py-2 rounded-lg transition"
      >
        {label}
      </button>
    )
  }
}
