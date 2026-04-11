import { signIn } from "@/auth"

export default function SignInButton() {
  return (
    <form
      action={async () => {
        "use server"
        await signIn("google", { redirectTo: "/dashboard" })
      }}
    >
      <button
        type="submit"
        className="bg-primary-orange cursor-pointer hover:bg-primary-orange/80 text-white px-6 py-2 rounded-lg transition"
      >
        Iniciar Sesión
      </button>
    </form>
  )
}
