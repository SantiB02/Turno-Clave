import Link from "next/link"
import React from "react"

export default function Header() {
  return (
    <nav className="fixed top-0 w-full bg-white shadow-sm z-50">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex justify-between items-center h-16">
          <div className="flex items-center gap-2">
            <div className="w-8 h-8 bg-orange-500 rounded-lg flex items-center justify-center">
              <span className="text-white font-bold text-lg">T</span>
            </div>
            <Link href="/" className="text-2xl font-bold text-gray-900">
              Turno Clave
            </Link>
          </div>
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
