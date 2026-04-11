import { signOut } from "@/auth"

export default function SignOutButton() {
  return (
    <form
      action={async () => {
        "use server"
        await signOut()
      }}
    >
      <button
        type="submit"
        className="bg-primary-orange cursor-pointer hover:bg-primary-orange/80 text-white px-6 py-2 rounded-lg transition"
      >
        Cerrar Sesión
      </button>
    </form>
  )
}
