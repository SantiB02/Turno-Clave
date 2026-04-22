import Image from "next/image"
import Footer from "../components/Footer"
import Header from "../components/Header"

export default function IniciarPruebaGratuita() {
  return (
    <>
      <Header />
      <div className="pt-32">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center mb-16">
            <h2 className="text-4xl md:text-5xl font-bold text-gray-900 mb-4">
              Empieza tu Prueba Gratuita
            </h2>
            <p className="text-xl text-gray-600 max-w-2xl mx-auto">
              Descubre cómo Turno Clave puede transformar la gestión de tus
              turnos con nuestra prueba gratuita de 14 días. Sin compromiso, sin
              tarjeta de crédito.
            </p>
          </div>
          <div className="grid md:grid-cols-2 max-w-7xl mx-auto gap-12 items-start mb-16">
            <div className="flex items-center">
              <Image
                width={600}
                height={600}
                src="/demo-screenshot.png"
                alt="Demo Screenshot"
                className="w-full mt-1 h-auto rounded-lg "
              />
            </div>
            <div>
              <h3 className="text-2xl font-bold text-gray-900 mb-6">
                ¿Qué Incluye la Prueba Gratuita?
              </h3>
              <ul className="space-y-6">
                <li className="flex items-start gap-4">
                  <span className="text-primary-orange font-bold text-2xl mt-1">
                    ✓
                  </span>
                  <div>
                    <h4 className="font-bold text-gray-900 mb-1">
                      Acceso Completo a Funcionalidades
                    </h4>
                    <p className="text-gray-600">
                      Explora todas las herramientas y características sin
                      restricciones
                    </p>
                  </div>
                </li>
                <li className="flex items-start gap-4">
                  <span className="text-primary-orange font-bold text-2xl mt-1">
                    ✓
                  </span>
                  <div>
                    <h3 className="font-bold text-gray-900 mb-1">
                      Gestión de Turnos Simplificada
                    </h3>
                    <p className="text-gray-600">
                      Administra tus turnos de manera eficiente y sin
                      complicaciones
                    </p>
                  </div>
                </li>
                <li className="flex items-start gap-4">
                  <span className="text-primary-orange font-bold text-2xl mt-1">
                    ✓
                  </span>
                  <div>
                    <h3 className="font-bold text-gray-900 mb-1">
                      Recordatorios Automáticos
                    </h3>
                    <p className="text-gray-600">
                      Notificaciones automáticas para reducir ausencias y
                      mantener a tus clientes informados
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
      </div>
      <Footer />
    </>
  )
}
