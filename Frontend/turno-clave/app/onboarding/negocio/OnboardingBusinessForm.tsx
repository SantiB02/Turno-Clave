"use client"

import { useState } from "react"

export default function OnboardingBusinessForm() {
  const [error, setError] = useState<string | null>(null)

  const handleSubmit = (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault()
    setError(null)

    try {
      const formData = new FormData(e.currentTarget)

      const name = formData.get("name") as string

      if (!name || name.trim() === "") {
        setError("El nombre del negocio es obligatorio.")
        return
      }
    } catch (error) {
      setError("Ocurrió un error al enviar el formulario.")
      console.error(
        "[OnboardingBusinessForm] Error:",
        error instanceof Error ? error.message : error,
      )
    }
  }

  return (
    <div>
      <h2 className="text-2xl">¿Cómo se llama tu negocio?</h2>
      {error && (
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded">
          <p className="font-bold">Error:</p>
          <p>{error}</p>
        </div>
      )}
      <form onSubmit={handleSubmit}>
        <div>
          <input
            type="text"
            name="name"
            placeholder="Nombre"
            className="border rounded-2xl border-primary-orange focus:outline-primary-orange focus:ring-primary-orange p-2 w-full mt-2"
          />
        </div>
      </form>
    </div>
  )
}
