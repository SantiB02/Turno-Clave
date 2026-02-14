import { ClockIcon } from "@heroicons/react/24/outline"
import { Lexend_Deca } from "next/font/google"
import Link from "next/link"
import React from "react"

const lexendDeca = Lexend_Deca({
  weight: ["400"],
  subsets: ["latin"],
})

export default function Header() {
  return (
    <nav className="fixed top-0 w-full bg-white shadow-sm z-50">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex justify-between items-center h-16">
          <Link
            href="/"
            className={`${lexendDeca.className} flex items-center text-2xl font-bold text-gray-900"`}
          >
            <ClockIcon className="w-7 h-7 text-orange-500" />
            Turno <span className="text-orange-500">Clave</span>
          </Link>

          <div className="hidden md:flex gap-8">
            <a
              href="#features"
              className="text-gray-700 hover:text-orange-500 transition"
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
            className="bg-orange-500 hover:bg-orange-600 text-white px-6 py-2 rounded-lg transition"
          >
            Empezar
          </button>
        </div>
      </div>
    </nav>
  )
}
