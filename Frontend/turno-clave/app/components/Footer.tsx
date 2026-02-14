import Image from "next/image"
import Link from "next/link"
import React from "react"

export default function Footer() {
  return (
    <footer className="bg-dark-blue text-white py-12 px-4 sm:px-6 lg:px-8">
      <div className="max-w-7xl mx-auto">
        <div className="grid md:grid-cols-4 gap-8 mb-8">
          <div>
            <div className="flex items-center gap-2">
              <Link
                href="/"
                className="flex items-center text-2xl font-bold text-gray-900"
              >
                <Image
                  src="/logo-300x100-dark-mode.png"
                  alt="Turno Clave Logo"
                  width={150}
                  height={150}
                  className="mr-2"
                />
              </Link>
            </div>
            <p className="text-gray-400 pl-1.5">
              Ordena tu tiempo, potencia tu negocio
            </p>
          </div>
          <div>
            <h4 className="font-bold mb-4">Producto</h4>
            <ul className="space-y-2 text-gray-400">
              <li>
                <a href="#" className="hover:text-secondary-orange transition">
                  Funcionalidades
                </a>
              </li>
              <li>
                <a href="#" className="hover:text-secondary-orange transition">
                  Precios
                </a>
              </li>
              <li>
                <a href="#" className="hover:text-secondary-orange transition">
                  Demo
                </a>
              </li>
            </ul>
          </div>
          <div>
            <h4 className="font-bold mb-4">Nosotros</h4>
            <ul className="space-y-2 text-gray-400">
              <li>
                <a href="#" className="hover:text-secondary-orange transition">
                  Acerca de
                </a>
              </li>
              <li>
                <a href="#" className="hover:text-secondary-orange transition">
                  Blog
                </a>
              </li>
              <li>
                <a href="#" className="hover:text-secondary-orange transition">
                  Contacto
                </a>
              </li>
            </ul>
          </div>
          <div>
            <h4 className="font-bold mb-4">Legal</h4>
            <ul className="space-y-2 text-gray-400">
              <li>
                <a href="#" className="hover:text-secondary-orange transition">
                  Privacidad
                </a>
              </li>
              <li>
                <a href="#" className="hover:text-secondary-orange transition">
                  Términos de Servicio
                </a>
              </li>
              <li>
                <a href="#" className="hover:text-secondary-orange transition">
                  Seguridad
                </a>
              </li>
            </ul>
          </div>
        </div>
        <div className="border-t border-gray-800 pt-8 text-center text-gray-400">
          <p>
            &copy; {new Date().getFullYear()} Turno Clave. Todos los derechos
            reservados.
          </p>
        </div>
      </div>
    </footer>
  )
}
