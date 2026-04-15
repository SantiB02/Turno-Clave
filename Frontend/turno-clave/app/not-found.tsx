import Link from "next/link"

export default function NotFound() {
  return (
    <div className="min-h-screen flex flex-col items-center justify-center">
      <h1 className="text-4xl font-bold">Error 404 - Página no encontrada</h1>
      <p className="text-gray-600 mt-4">
        Lo sentimos, la página que estás buscando no existe.
      </p>
      <Link
        href="/"
        className="mt-6 px-4 hover:bg-primary-orange/80 py-2 bg-primary-orange text-white rounded transition"
      >
        Volver al Inicio
      </Link>
    </div>
  )
}
