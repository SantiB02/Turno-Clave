import Image from "next/image"
import Link from "next/link"
import Header from "./components/Header"

export default function Home() {
  return (
    <>
      <Header />
      <div className="pt-32">
        {/* Hero Section */}
        <section className="pb-20 px-4 sm:px-6 lg:px-8">
          <div className="max-w-7xl mx-auto">
            <div className="grid md:grid-cols-2 gap-12 items-center">
              <div>
                <h1 className="text-5xl md:text-6xl font-bold text-gray-900 mb-6 leading-tight">
                  Administra tus turnos con Turno{" "}
                  <span className="text-primary-orange">Clave</span>
                </h1>
                <p className="text-xl text-gray-600 mb-8 leading-relaxed">
                  La poderosa plataforma de gestión de turnos diseñada para
                  negocios. Agenda, organiza y optimiza tus reservas sin
                  esfuerzo.
                </p>
                <div className="flex flex-col sm:flex-row gap-4">
                  <Link
                    href="/iniciar-prueba-gratuita"
                    className="bg-primary-orange hover:bg-primary-orange/80 text-white text-center font-semibold px-8 py-3 rounded-lg transition text-lg"
                  >
                    Iniciar Prueba Gratuita
                  </Link>
                  <Link
                    href="/demo"
                    className="border-2 border-primary-orange text-primary-orange text-center hover:bg-primary-orange/10 font-semibold px-8 py-3 rounded-lg transition text-lg"
                  >
                    Ver Demo
                  </Link>
                </div>
              </div>
              <div className="flex justify-center">
                <div className="relative bg-blue-50 rounded-2xl h-96 w-full max-w-xs md:w-120 md:max-w-none md:w-120 flex items-center justify-center overflow-hidden">
                  <Image
                    src="/mis-turnos-screenshot.png"
                    alt="Mis Turnos screenshot"
                    fill
                    className="object-contain w-full h-full p-4 rounded-lg"
                  />
                </div>
              </div>
            </div>
          </div>
        </section>
        {/* Features Section */}
        <section
          id="features"
          className="py-20 px-4 sm:px-6 lg:px-8 bg-blue-50"
        >
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
                  id: 1,
                  icon: "📅",
                  title: "Agendamiento Inteligente",
                  description:
                    "Interfaz intuitiva para programar, reprogramar y cancelar turnos fácilmente",
                },
                {
                  id: 2,
                  icon: "🔔",
                  title: "Recordatorios Automáticos",
                  description:
                    "Notificaciones automáticas para reducir ausencias y mantener a tus clientes informados",
                },
                {
                  id: 3,
                  icon: "👥",
                  title: "Soporte Multiusuario",
                  description:
                    "Colaboración en equipo con roles y permisos personalizados",
                },
                {
                  id: 4,
                  icon: "📱",
                  title: "Compatibilidad Móvil",
                  description:
                    "Administra tus turnos desde cualquier lugar con nuestra interfaz móvil optimizada",
                },
                {
                  id: 5,
                  icon: "⚙️",
                  title: "Ajustes Personalizables",
                  description:
                    "Configura tus preferencias de negocio para adaptarse a tus necesidades específicas",
                },
                {
                  id: 6,
                  icon: "📊",
                  title: "Reportes y Análisis",
                  description:
                    "Rastrea el rendimiento de tu negocio con reportes detallados y análisis de datos",
                },
              ].map((feature) => (
                <div
                  key={feature.id}
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
              <div className="bg-primary-orange/10 rounded-2xl h-96 flex items-center justify-center">
                <div className="text-center text-gray-400">
                  <svg
                    className="w-24 h-24 mx-auto mb-4 opacity-50"
                    fill="none"
                    stroke="currentColor"
                    viewBox="0 0 24 24"
                  >
                    <title>Potenciar Eficiencia</title>
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
                  ¿Por qué elegir Turno{" "}
                  <span className="text-primary-orange">Clave</span>?
                </h2>
                <ul className="space-y-4">
                  <li className="flex items-start gap-4">
                    <span className="text-primary-orange font-bold text-2xl mt-1">
                      ✓
                    </span>
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
                    <span className="text-primary-orange font-bold text-2xl mt-1">
                      ✓
                    </span>
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
                    <span className="text-primary-orange font-bold text-2xl mt-1">
                      ✓
                    </span>
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
                    <span className="text-primary-orange font-bold text-2xl mt-1">
                      ✓
                    </span>
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
        {/*a section se le puede agregar bg-gradient-to-r from-primary-orange to-primary-orange/80 para que tenga gradiente, pero se ve un poco raro*/}
        <section className="py-20 px-4 sm:px-6 lg:px-8 bg-primary-orange">
          <div className="max-w-4xl mx-auto text-center">
            <h2 className="text-4xl md:text-5xl font-bold text-white mb-6">
              ¿Listo para transformar la gestión de tus turnos?
            </h2>
            <p className="text-xl text-orange-50 mb-8 max-w-2xl mx-auto">
              Sumate a cientos de negocios que ya están optimizando su tiempo
              con Turno Clave.
            </p>
            <Link
              href="/iniciar-prueba-gratuita"
              className="bg-white text-primary-orange hover:bg-gray-50 font-bold px-10 py-4 rounded-lg transition text-lg"
            >
              Empezá tu prueba gratuita de 14 días
            </Link>
          </div>
        </section>
      </div>
    </>
  )
}
