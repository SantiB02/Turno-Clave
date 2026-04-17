"use client"

import { ArrowRightStartOnRectangleIcon } from "@heroicons/react/24/outline"
import { handleSignOut } from "@/lib/actions/auth"

export default function SignOutIcon() {
  return (
    <form action={handleSignOut} className="w-full">
      <button
        type="submit"
        title="Cerrar Sesión"
        className="flex items-center cursor-pointer justify-center py-2 rounded hover:bg-orange-400 w-full text-white"
      >
        <ArrowRightStartOnRectangleIcon className="h-10 w-10" />
      </button>
    </form>
  )
}
