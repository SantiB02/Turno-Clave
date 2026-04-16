"use client"

import { ArrowRightStartOnRectangleIcon } from "@heroicons/react/24/outline"
import { handleSignOut } from "@/lib/actions/auth"

export default function SignOutIcon() {
  return (
    <form action={handleSignOut}>
      <button
        type="submit"
        title="Cerrar Sesión"
        className="block cursor-pointer mx-auto py-2 rounded hover:bg-orange-400"
      >
        <ArrowRightStartOnRectangleIcon className="h-10 w-10 inline-block mr-2" />
      </button>
    </form>
  )
}
