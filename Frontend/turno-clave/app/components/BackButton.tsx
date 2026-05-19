"use client"

import { ChevronLeftIcon } from "@heroicons/react/24/outline"
import Link from "next/link"
import { useRouter } from "next/navigation"

type Props = {
  className?: string
  href?: string
}

export default function BackButton({ className = "", href }: Props) {
  const router = useRouter()

  if (href)
    return (
      <Link href={href} className={`flex items-center gap-2 ${className}`}>
        <ChevronLeftIcon className="w-6 h-6" />
      </Link>
    )
  else
    return (
      <button
        type="button"
        onClick={() => router.back()}
        className={`flex items-center gap-2 ${className}`}
      >
        <ChevronLeftIcon className="w-6 h-6" />
      </button>
    )
}
