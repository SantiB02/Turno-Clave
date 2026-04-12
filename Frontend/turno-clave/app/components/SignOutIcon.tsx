import { PowerIcon } from "@heroicons/react/24/outline"
import { signOut } from "@/auth"

export default function SignOutIcon() {
  return (
    <form
      action={async () => {
        "use server"
        await signOut({ redirectTo: "/" })
      }}
    >
      <button
        type="submit"
        title="Cerrar Sesión"
        className="block cursor-pointer mx-auto py-2 rounded hover:bg-orange-400"
      >
        <PowerIcon className="h-10 w-10 inline-block mr-2" />
      </button>
    </form>
  )
}
