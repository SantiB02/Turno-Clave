import Image from "next/image"
import Link from "next/link"

export default function Header() {
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
            <a
              href="#features"
              className="text-gray-700 hover:text-primary-orange transition"
            >
              Funcionalidades
            </a>
            <a
              href="#benefits"
              className="text-gray-700 hover:text-orange-500 transition"
            >
              Beneficios
            </a>
          </div>
          <button
            type="button"
            className="bg-primary-orange hover:bg-primary-orange/80 text-white px-6 py-2 rounded-lg transition"
          >
            Empezar
          </button>
        </div>
      </div>
    </nav>
  )
}
