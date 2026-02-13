import React from "react"

export default function Footer() {
  return (
    <footer className="bg-gray-900 text-white py-12 px-4 sm:px-6 lg:px-8">
      <div className="max-w-7xl mx-auto">
        <div className="grid md:grid-cols-4 gap-8 mb-8">
          <div>
            <div className="flex items-center gap-2 mb-4">
              <div className="w-8 h-8 bg-orange-500 rounded-lg flex items-center justify-center">
                <span className="text-white font-bold">T</span>
              </div>
              <span className="text-xl font-bold">Turno Clave</span>
            </div>
            <p className="text-gray-400">
              Ordena tu tiempo, potencia tu negocio
            </p>
          </div>
          <div>
            <h4 className="font-bold mb-4">Producto</h4>
            <ul className="space-y-2 text-gray-400">
              <li>
                <a href="#" className="hover:text-orange-500 transition">
                  Funcionalidades
                </a>
              </li>
              <li>
                <a href="#" className="hover:text-orange-500 transition">
                  Precios
                </a>
              </li>
              <li>
                <a href="#" className="hover:text-orange-500 transition">
                  Demo
                </a>
              </li>
            </ul>
          </div>
          <div>
            <h4 className="font-bold mb-4">Nosotros</h4>
            <ul className="space-y-2 text-gray-400">
              <li>
                <a href="#" className="hover:text-orange-500 transition">
                  Acerca de
                </a>
              </li>
              <li>
                <a href="#" className="hover:text-orange-500 transition">
                  Blog
                </a>
              </li>
              <li>
                <a href="#" className="hover:text-orange-500 transition">
                  Contacto
                </a>
              </li>
            </ul>
          </div>
          <div>
            <h4 className="font-bold mb-4">Legal</h4>
            <ul className="space-y-2 text-gray-400">
              <li>
                <a href="#" className="hover:text-orange-500 transition">
                  Privacidad
                </a>
              </li>
              <li>
                <a href="#" className="hover:text-orange-500 transition">
                  Términos de Servicio
                </a>
              </li>
              <li>
                <a href="#" className="hover:text-orange-500 transition">
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
