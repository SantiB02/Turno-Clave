"use client"

import { useState } from "react"
import { createBusiness } from "@/services/businessService"

// This is an old form
export default function CreateBusinessForm() {
  const [error, setError] = useState<string | null>(null)
  const [loading, setLoading] = useState(false)

  const handleSubmit = async (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault()
    setError(null)
    setLoading(true)

    try {
      const formData = new FormData(e.currentTarget)

      /* await createBusiness({
        name: formData.get("name") as string,
        description: formData.get("description") as string,
        logoUrl: formData.get("logoUrl") as string,
        email: formData.get("email") as string,
        phone: formData.get("phone") as string,
        address: formData.get("address") as string,
        city: formData.get("city") as string,
        state: formData.get("state") as string,
        country: formData.get("country") as string,
        timeZone: formData.get("timeZone") as string,
      }) */

      // Success - opcional: hacer algo aquí
      window.location.href = "/dashboard"
    } catch (err) {
      const errorMessage =
        err instanceof Error ? err.message : "Error desconocido"
      setError(errorMessage)
      console.error("[CreateBusinessForm] Error:", errorMessage)
    } finally {
      setLoading(false)
    }
  }

  return (
    <div>
      {error && (
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
          <p className="font-bold">Error:</p>
          <p>{error}</p>
        </div>
      )}
      <form onSubmit={handleSubmit} className="space-y-4">
        <div>
          <label
            htmlFor="name"
            className="block text-sm font-medium text-gray-700"
          >
            Nombre del negocio <span className="text-red-500 text-xl">*</span>
          </label>
          <input
            type="text"
            name="name"
            id="name"
            className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm p-2 focus:ring-primary-orange focus:border-primary-orange"
            required
          />
        </div>
        <div>
          <label
            htmlFor="description"
            className="block text-sm font-medium text-gray-700"
          >
            Descripción del negocio
          </label>
          <textarea
            name="description"
            id="description"
            rows={3}
            className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm p-2 focus:ring-primary-orange focus:border-primary-orange"
          />
        </div>
        {/* <div>
        <label
          htmlFor="logoUrl"
          className="block text-sm font-medium text-gray-700"
        >
          URL del logo{" "}
        </label>
        <input
          type="text"
          name="logoUrl"
          id="logoUrl"
          className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm p-2 focus:ring-primary-orange focus:border-primary-orange"
        />
      </div> */}
        <div>
          <label
            htmlFor="email"
            className="block text-sm font-medium text-gray-700"
          >
            Correo electrónico <span className="text-red-500 text-xl">*</span>
          </label>
          <input
            type="email"
            name="email"
            id="email"
            className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm p-2 focus:ring-primary-orange focus:border-primary-orange"
            required
          />
        </div>
        <div>
          <label
            htmlFor="phone"
            className="block text-sm font-medium text-gray-700"
          >
            Teléfono <span className="text-red-500 text-xl">*</span>
          </label>
          <input
            type="text"
            name="phone"
            id="phone"
            className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm p-2 focus:ring-primary-orange focus:border-primary-orange"
            required
          />
        </div>
        <div>
          <label
            htmlFor="address"
            className="block text-sm font-medium text-gray-700"
          >
            Dirección <span className="text-red-500 text-xl">*</span>
          </label>
          <input
            type="text"
            name="address"
            id="address"
            className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm p-2 focus:ring-primary-orange focus:border-primary-orange"
            required
          />
        </div>
        <div>
          <label
            htmlFor="city"
            className="block text-sm font-medium text-gray-700"
          >
            Ciudad <span className="text-red-500 text-xl">*</span>
          </label>
          <input
            type="text"
            name="city"
            id="city"
            className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm p-2 focus:ring-primary-orange focus:border-primary-orange"
            required
          />
        </div>
        <div>
          <label
            htmlFor="state"
            className="block text-sm font-medium text-gray-700"
          >
            Provincia <span className="text-red-500 text-xl">*</span>
          </label>
          <input
            type="text"
            name="state"
            id="state"
            className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm p-2 focus:ring-primary-orange focus:border-primary-orange"
            required
          />
        </div>
        <div>
          <label
            htmlFor="country"
            className="block text-sm font-medium text-gray-700"
          >
            País <span className="text-red-500 text-xl">*</span>
          </label>
          <input
            type="text"
            name="country"
            id="country"
            className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm p-2 focus:ring-primary-orange focus:border-primary-orange"
            required
          />
        </div>
        <div>
          <label
            htmlFor="timeZone"
            className="block text-sm font-medium text-gray-700"
          >
            Zona horaria <span className="text-red-500 text-xl">*</span>
          </label>
          <input
            type="text"
            name="timeZone"
            id="timeZone"
            className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm p-2 focus:ring-primary-orange focus:border-primary-orange"
          />
        </div>
        <div>
          {loading ? (
            <button
              type="button"
              className="inline-flex items-center px-4 py-2 mt-2 border border-transparent cursor-not-allowed text-md font-medium rounded-md shadow-sm text-white bg-gray-400"
              disabled
            >
              Creando Negocio...
            </button>
          ) : (
            <button
              type="submit"
              className="inline-flex items-center px-4 py-2 mt-2 border border-transparent cursor-pointer text-md font-medium rounded-md shadow-sm text-white bg-primary-orange hover:bg-orange-500 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-orange"
            >
              Crear Negocio
            </button>
          )}
        </div>
      </form>
    </div>
  )
}
