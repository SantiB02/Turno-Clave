import Link from "next/link"

export default function Home() {
  return (
    <div className="pt-32">
      {/* Hero Section */}
      <section className="pb-20 px-4 sm:px-6 lg:px-8">
        <div className="max-w-7xl mx-auto">
          <div className="grid md:grid-cols-2 gap-12 items-center">
            <div>
              <h1 className="text-5xl md:text-6xl font-bold text-gray-900 mb-6 leading-tight">
                Administra tus turnos con{" "}
                <span className="text-orange-500">Turno Clave</span>
              </h1>
              <p className="text-xl text-gray-600 mb-8 leading-relaxed">
                La poderosa plataforma de gestión de turnos diseñada para
                negocios. Agenda, organiza y optimiza tus reservas sin esfuerzo.
              </p>
              <div className="flex flex-col sm:flex-row gap-4">
                <Link
                  href="/iniciar-prueba-gratuita"
                  className="bg-orange-500 hover:bg-orange-600 text-white font-semibold px-8 py-3 rounded-lg transition text-lg"
                >
                  Iniciar Prueba Gratuita
                </Link>
                <button className="border-2 border-orange-500 text-orange-500 hover:bg-orange-50 font-semibold px-8 py-3 rounded-lg transition text-lg">
                  Ver Demo
                </button>
              </div>
            </div>
            <div className="bg-blue-50 rounded-2xl h-96 flex items-center justify-center">
              <div className="text-center text-gray-400">
                <svg
                  className="w-24 h-24 mx-auto mb-4 opacity-50"
                  fill="none"
                  stroke="currentColor"
                  viewBox="0 0 24 24"
                >
                  <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    strokeWidth={2}
                    d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z"
                  />
                </svg>
                <p>Calendario de turnos visual</p>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Features Section */}
      <section id="features" className="py-20 px-4 sm:px-6 lg:px-8 bg-blue-50">
        <div className="max-w-7xl mx-auto">
          <div className="text-center mb-16">
            <h2 className="text-4xl font-bold text-gray-900 mb-4">
              Potentes Funcionalidades
            </h2>
            <p className="text-xl text-gray-600 max-w-2xl mx-auto">
              Todo lo que necesitas para gestionar tus turnos de manera
              eficiente y sin complicaciones
            </p>
          </div>
          <div className="grid md:grid-cols-3 gap-8">
            {[
              {
                icon: "📅",
                title: "Agendamiento Inteligente",
                description:
                  "Interfaz intuitiva para programar, reprogramar y cancelar turnos fácilmente",
              },
              {
                icon: "🔔",
                title: "Recordatorios Automáticos",
                description:
                  "Notificaciones automáticas para reducir ausencias y mantener a tus clientes informados",
              },
              {
                icon: "👥",
                title: "Soporte Multiusuario",
                description:
                  "Colaboración en equipo con roles y permisos personalizados",
              },
              {
                icon: "📱",
                title: "Compatibilidad Móvil",
                description:
                  "Administra tus turnos desde cualquier lugar con nuestra interfaz móvil optimizada",
              },
              {
                icon: "⚙️",
                title: "Ajustes Personalizables",
                description:
                  "Configura tus preferencias de negocio para adaptarse a tus necesidades específicas",
              },
              {
                icon: "📊",
                title: "Reportes y Análisis",
                description:
                  "Rastrea el rendimiento de tu negocio con reportes detallados y análisis de datos",
              },
            ].map((feature, idx) => (
              <div
                key={idx}
                className="bg-white p-8 rounded-xl shadow-sm hover:shadow-md transition"
              >
                <div className="text-4xl mb-4">{feature.icon}</div>
                <h3 className="text-xl font-bold text-gray-900 mb-2">
                  {feature.title}
                </h3>
                <p className="text-gray-600">{feature.description}</p>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Benefits Section */}
      <section id="benefits" className="py-20 px-4 sm:px-6 lg:px-8">
        <div className="max-w-7xl mx-auto">
          <div className="grid md:grid-cols-2 gap-12 items-center">
            <div className="bg-orange-500 bg-opacity-10 rounded-2xl h-96 flex items-center justify-center">
              <div className="text-center text-gray-400">
                <svg
                  className="w-24 h-24 mx-auto mb-4 opacity-50"
                  fill="none"
                  stroke="currentColor"
                  viewBox="0 0 24 24"
                >
                  <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    strokeWidth={2}
                    d="M13 10V3L4 14h7v7l9-11h-7z"
                  />
                </svg>
                <p>Potenciar Eficiencia</p>
              </div>
            </div>
            <div>
              <h2 className="text-4xl font-bold text-gray-900 mb-8">
                ¿Por qué elegir Turno Clave?
              </h2>
              <ul className="space-y-4">
                <li className="flex items-start gap-4">
                  <span className="text-orange-500 text-2xl mt-1">✓</span>
                  <div>
                    <h3 className="font-bold text-gray-900 mb-1">
                      Ahorra Tiempo
                    </h3>
                    <p className="text-gray-600">
                      Automatiza la gestión de turnos y reduce las tareas
                      manuales
                    </p>
                  </div>
                </li>
                <li className="flex items-start gap-4">
                  <span className="text-orange-500 text-2xl mt-1">✓</span>
                  <div>
                    <h3 className="font-bold text-gray-900 mb-1">
                      Reduce Ausencias
                    </h3>
                    <p className="text-gray-600">
                      Recordatorios automáticos para mantener a tus clientes
                      informados
                    </p>
                  </div>
                </li>
                <li className="flex items-start gap-4">
                  <span className="text-orange-500 text-2xl mt-1">✓</span>
                  <div>
                    <h3 className="font-bold text-gray-900 mb-1">
                      Integración Perfecta
                    </h3>
                    <p className="text-gray-600">
                      Conecta con tus herramientas favoritas para un flujo de
                      trabajo sin interrupciones
                    </p>
                  </div>
                </li>
                <li className="flex items-start gap-4">
                  <span className="text-orange-500 text-2xl mt-1">✓</span>
                  <div>
                    <h3 className="font-bold text-gray-900 mb-1">
                      Soporte 24/7
                    </h3>
                    <p className="text-gray-600">
                      Nuestro equipo está aquí para ayudarte en cualquier
                      momento
                    </p>
                  </div>
                </li>
              </ul>
            </div>
          </div>
        </div>
      </section>

      {/* CTA Section */}
      <section className="py-20 px-4 sm:px-6 lg:px-8 bg-gradient-to-r from-orange-500 to-orange-600">
        <div className="max-w-4xl mx-auto text-center">
          <h2 className="text-4xl md:text-5xl font-bold text-white mb-6">
            ¿Listo para transformar la gestión de tus turnos?
          </h2>
          <p className="text-xl text-orange-50 mb-8 max-w-2xl mx-auto">
            Sumate a cientos de negocios que ya están optimizando su tiempo con
            Turno Clave.
          </p>
          <button className="bg-white text-orange-500 hover:bg-gray-50 font-bold px-10 py-4 rounded-lg transition text-lg">
            Empezá tu prueba gratuita de 14 días
          </button>
        </div>
      </section>
    </div>
  )
}
