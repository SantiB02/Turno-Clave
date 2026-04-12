import Image from "next/image"
import Link from "next/link"
import { auth } from "@/auth"
import Button from "./Button"
import SignInButton from "./SignInButton"

export default async function Header() {
  const session = await auth()

  return (
    <nav className="fixed top-0 w-full bg-white shadow-sm z-50">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex justify-between items-center h-16">
          <Link
            href="/"
            className="flex items-center text-2xl font-bold text-gray-900"
          >
            <Image
              src="/header-logo-300x100.png"
              alt="Turno Clave Logo"
              width={150}
              height={150}
              className="mr-2"
            />
          </Link>

          <div className="hidden md:flex gap-8">
            <Link
              href="#features"
              className="text-gray-700 hover:text-primary-orange transition"
            >
              Funcionalidades
            </Link>
            <Link
              href="#benefits"
              className="text-gray-700 hover:text-orange-500 transition"
            >
              Beneficios
            </Link>
            <Link
              href="#"
              className="text-gray-700 hover:text-orange-500 transition"
            >
              Planes
            </Link>
          </div>

          {session?.user ? (
            <Button label="Ir al Panel de Control" href="/dashboard" />
          ) : (
            <SignInButton />
          )}
        </div>
      </div>
    </nav>
  )
}
