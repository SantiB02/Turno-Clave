import Link from "next/link"
import type { ReactNode } from "react"

interface ButtonProps {
  label: string
  onClick?: () => void
  disabled?: boolean
  type?: "button" | "submit" | "reset"
  href?: string
  backgroundColor?: string
  hoverBackgroundColor?: string
  size?:
    | "text-sm"
    | "text-md"
    | "text-lg"
    | "text-xl"
    | "text-2xl"
    | "text-3xl"
    | "text-4xl"
    | "text-5xl"
    | "text-6xl"
  icon?: ReactNode
  className?: string
}

export default function Button({
  label,
  onClick,
  disabled,
  type,
  href,
  backgroundColor,
  hoverBackgroundColor,
  size,
  icon,
  className,
}: ButtonProps) {
  if (href) {
    return (
      <Link href={href}>
        <button
          type={type || "button"}
          onClick={onClick}
          disabled={disabled}
          className={`
            ${disabled ? "bg-gray-300" : ""}
            ${className ?? ""}
            ${!disabled ? (backgroundColor ?? "bg-primary-orange") : "bg-gray-300"}
            ${!disabled ? (hoverBackgroundColor ?? "hover:bg-primary-orange") : ""}
            ${size ?? ""}
            cursor-pointer
            flex
            items-center
            gap-2
            text-white
            px-4
            py-2
            rounded-lg
            transition
          `}
        >
          {icon}
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
          ${disabled ? "bg-gray-300" : ""}
          ${className ?? ""}
          ${!disabled ? (backgroundColor ?? "bg-primary-orange") : "bg-gray-300"}
          ${!disabled ? (hoverBackgroundColor ?? "hover:bg-primary-orange") : ""}
          ${size ?? ""}
          cursor-pointer
          flex
          items-center
          gap-2
          text-white
          px-4
          py-2
          rounded-lg
          transition
        `}
      >
        {icon}
        {label}
      </button>
    )
  }
}
